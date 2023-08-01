using Markup.Interface;

namespace Markup.RenderElement;

internal class ContentRender : IRenderElement<Content>
{
    public int Width { get; set; }
    public Content ViewComponent { get; }
    public Dimension Dimension { get; set; }
    
    public ContentRender(int allowedWidth, Content content)
    {
        Width = allowedWidth;
        ViewComponent = content;
        Dimension = GetDimension();
    }
    
    private Dimension GetDimension()
    {
        var totalRenderElementHeight = GetValueHeight(Width);
        var dimension = new Dimension(Width, totalRenderElementHeight);

        return dimension;
    }

    private int GetValueHeight(int outputRenderElementWidth)
    {
        var value = ViewComponent.Value;
        int height = value.Length <= outputRenderElementWidth || ViewComponent.IsMultiline == false
            ? 1
            : (int)Math.Ceiling((double)value.Length / outputRenderElementWidth);

        return height;
    }

    public IEnumerable<string> Build()
    {
        var body = GetValue();
        
        return body;
    }

    private IEnumerable<string> GetValue()
    {
        var result = new List<string>();
        
        var elementValue = ViewComponent.Value;
        
        var outputWidthLimit = Width;

        if (elementValue.Length <= outputWidthLimit)
        {
            var value = elementValue.PadRight(outputWidthLimit);
            result.Add(value);
        }
        else
        {
            if (ViewComponent.IsMultiline)
            {
                result = GetSplitValue().ToList();
            }
            else
            {
                var value = elementValue[..(outputWidthLimit - 1)] + "\u2026";
                result.Add(value);
            }
        }

        return result;
    }

    private IEnumerable<string> GetSplitValue()
    {
        var index = 0;
        var value = ViewComponent.Value;

        var outputRenderElementWidth = Width;

        int substringCount = GetValueHeight(outputRenderElementWidth);
        if (substringCount > 1) outputRenderElementWidth = outputRenderElementWidth - 1;

        var result = new List<string>();
                
        for (int i = 0; i < substringCount; i++)
        {
            var sbString = value.Skip(index).Take(outputRenderElementWidth).ToArray();
            index += outputRenderElementWidth;
            var isLast = i + 1 == substringCount;
            var str = isLast ? new string(sbString).PadRight(Width) : new string(sbString) + "⤸";
            result.Add(str);
        }

        return result;
    }
}