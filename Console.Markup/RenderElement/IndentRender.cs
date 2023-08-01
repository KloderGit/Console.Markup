using Markup.Interface;

namespace Markup.RenderElement;

internal class IndentRender : IRenderElement<IndentComponent>
{
    public int Width { get; set; }
    public Dimension Dimension { get; set; }
    public IndentComponent ViewComponent { get; }

    private IRenderElement childrenRender;

    public IndentRender(int allowedWidth, IndentComponent component)
    {
        Width = allowedWidth;
        ViewComponent = component;
        
        BuildChildrenRenders();
        BuildDimensions();
    }

    private void BuildChildrenRenders()
    {
        var availableChildrenWidth = GetAvailableWidthForContent();
        childrenRender = ((IViewComponent)ViewComponent.Child).CreateRender(availableChildrenWidth);
    }

    private int GetAvailableWidthForContent()
    {
        var width = Width - ViewComponent.Left - ViewComponent.Right;
        return width;
    }
    
    private void BuildDimensions()
    {
        var childrenMaxHeight = childrenRender.Dimension.Height;
        var totalRenderElementHeight = childrenMaxHeight + ViewComponent.Top + ViewComponent.Bottom;
        
        var dimension = new Dimension(Width, totalRenderElementHeight);
        
        Dimension = dimension;
    }
    
    public IEnumerable<string> Build()
    {
        var layout = new List<string>();

        var symbol = ' ';
        var horizontalString = Symbol.GenerateCharSeq(Dimension.Width, symbol);

        if (ViewComponent.Top != 0) { for (int i = 0; i < ViewComponent.Top; i++) layout.Add(horizontalString); }

        var left = Symbol.GenerateCharSeq(ViewComponent.Left, symbol);
        var right = Symbol.GenerateCharSeq(ViewComponent.Right, symbol);
        
        var body = childrenRender.Build().Select(x=>left + x + right);
        
        layout.AddRange(body);
        
        if (ViewComponent.Bottom != 0){ for (int i = 0; i < ViewComponent.Bottom; i++) layout.Add(horizontalString); }

        return layout;
    }
}