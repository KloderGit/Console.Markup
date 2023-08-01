using ConsoleMarkup.Interface;
using ConsoleMarkup.Extension;

namespace ConsoleMarkup.RenderElement;

internal class IndentRender : IRenderElement<Indent>
{
    public int AllowedWidth { get; set; }
    public Dimension Dimension { get; set; }
    public Indent ViewComponent { get; }

    private IRenderElement childrenRender;

    public IndentRender(int allowedWidth, Indent component)
    {
        AllowedWidth = allowedWidth;
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
        var width = AllowedWidth - ViewComponent.Left - ViewComponent.Right;
        return width;
    }
    
    private void BuildDimensions()
    {
        var childrenMaxHeight = childrenRender.Dimension.Height;
        var totalRenderElementHeight = childrenMaxHeight + ViewComponent.Top + ViewComponent.Bottom;
        
        var dimension = new Dimension(AllowedWidth, totalRenderElementHeight);
        
        Dimension = dimension;
    }
    
    public IEnumerable<string> Build()
    {
        var layout = new List<string>();

        var symbol = ' ';
        var horizontalString = string.Empty.GenerateByChar(Dimension.Width, symbol);

        if (ViewComponent.Top != 0) { for (int i = 0; i < ViewComponent.Top; i++) layout.Add(horizontalString); }

        var left = string.Empty.GenerateByChar(ViewComponent.Left, symbol);
        var right = string.Empty.GenerateByChar(ViewComponent.Right, symbol);
        
        var body = childrenRender.Build().Select(x=>left + x + right);
        
        layout.AddRange(body);
        
        if (ViewComponent.Bottom != 0){ for (int i = 0; i < ViewComponent.Bottom; i++) layout.Add(horizontalString); }

        return layout;
    }
}