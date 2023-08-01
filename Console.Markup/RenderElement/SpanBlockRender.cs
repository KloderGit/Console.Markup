using System.Text;
using ConsoleMarkup.Extension;
using ConsoleMarkup.Interface;

namespace ConsoleMarkup.RenderElement;

internal class SpanBlockRender : IRenderElement<Block>
{
    public int AllowedWidth { get; set; }
    public Block ViewComponent { get; set; }
    public Dimension Dimension { get; set; }

    private List<IRenderElement> ChildrenRenders = new List<IRenderElement>();

    public SpanBlockRender(int allowedWidth, Block content)
    {
        AllowedWidth = allowedWidth;
        ViewComponent = content;

        CreateChildren();
        
        Dimension = GetDimension();
    }

    private void CreateChildren()
    {
        // var childAllowWidth = AllowedWidth / ViewComponent.Children.Count();
        // ChildrenRenders = ViewComponent.Children.Select(x => x.CreateRender(childAllowWidth));

        var widthCalculator = new TableColumnWidthCalculator(AllowedWidth, ViewComponent.Children.Count());
        var childrenWidth = widthCalculator.GetWidth();
        foreach (var child in ViewComponent.Children)
        {
            ChildrenRenders.Add(child.CreateRender(childrenWidth.Pop()));
        }
    }

    private Dimension GetDimension()
    {
        var childrenHeight = ChildrenRenders.Select(x => x.Dimension.Height).Max();
        
        var dimension = new Dimension(AllowedWidth, childrenHeight);

        return dimension;
    }

    public IEnumerable<string> Build()
    {
        var layout = new List<string>();

        var array = ChildrenRenders
            .Select(x => 
                new Tuple<int, Stack<string>>(
                    x.AllowedWidth, 
                    new Stack<string>(x.Build().Reverse())))
            .ToList();

        
        for (int i = 0; i < Dimension.Height; i++)
        {
            var stringBuilder = new StringBuilder();
            
            foreach (var child in array)
            {
                if (child.Item2.TryPop(out string result))
                {
                    stringBuilder.Append(result);
                }
                else
                {
                    stringBuilder.Append(string.Empty.GenerateByChar(child.Item1, ' '));
                }
            }
            
            layout.Add(stringBuilder.ToString());
        }
        
        return layout;
    }
}