using ConsoleMarkup.Interface;
using ConsoleMarkup.Extension;

namespace ConsoleMarkup.RenderElement;

internal class BorderRender : IRenderElement<Border>
{
    public int AllowedWidth { get; set; }
    public Dimension Dimension { get; set; }
    public Border ViewComponent { get; }

    private IRenderElement childrenRender;

    public BorderRender(int allowedWidth, Border component)
    {
        AllowedWidth = allowedWidth;
        ViewComponent = component;
        
        BuildChildrenRenders();
        BuildDimensions();

        Dimension = BuildDimensions();
    }

    private void BuildChildrenRenders()
    {
        var availableChildrenWidth = GetAvailableWidthForContent();
        childrenRender = ((IViewComponent)ViewComponent.Child).CreateRender(availableChildrenWidth);
    }

    private int GetAvailableWidthForContent()
    {
        var width = AllowedWidth - (ViewComponent.Left ? 1 : 0) - (ViewComponent.Right ? 1 : 0);
        return width;
    }
    
    private Dimension BuildDimensions()
    {
        var childrenMaxHeight = childrenRender.Dimension.Height;
        var totalRenderElementHeight = childrenMaxHeight + (ViewComponent.Top ? 1 : 0) + (ViewComponent.Bottom ? 1 : 0);
        
        var dimension = new Dimension(AllowedWidth, totalRenderElementHeight);
        
        return dimension;
    }
    
    public IEnumerable<string> Build()
    {
        var layout = new List<string>();

        const char horizontalSymbol = '─';
        const char verticalSymbol = '│';
        
        const char leftTopCornerSymbol = '┌';
        const char rightTopCornerSymbol = '┐';

        const char leftBottomCornerSymbol = '└';
        const char rightBottomCornerSymbol = '┘';
        
        var left = string.Empty.GenerateByChar((ViewComponent.Left ? 1 : 0), verticalSymbol);
        var right = string.Empty.GenerateByChar((ViewComponent.Right ? 1 : 0), verticalSymbol);
        
        if (ViewComponent.Top)
        {
            var firstSymbol = ViewComponent.Left ? leftTopCornerSymbol : horizontalSymbol;
            var lastSymbol = ViewComponent.Right ? rightTopCornerSymbol : horizontalSymbol;
            
            var top = string.Empty.GenerateByChar(Dimension.Width-2, horizontalSymbol);
            if(string.IsNullOrEmpty(top) == false) layout.Add($"{firstSymbol}{top}{lastSymbol}");
        }

        var body = childrenRender.Build()
            .Select(x=>left + x + right);
        
        layout.AddRange(body);

        if (ViewComponent.Bottom)
        {
            var firstSymbol = ViewComponent.Left ? leftBottomCornerSymbol : horizontalSymbol;
            var lastSymbol = ViewComponent.Right ? rightBottomCornerSymbol : horizontalSymbol;
            
            var bottom = string.Empty.GenerateByChar(Dimension.Width - 2, horizontalSymbol);
            if(string.IsNullOrEmpty(bottom) == false) layout.Add($"{firstSymbol}{bottom}{lastSymbol}");
        }

        return layout;
    }
}