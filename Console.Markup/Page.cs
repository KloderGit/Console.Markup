using System.Linq.Expressions;
using ConsoleMarkup.Interface;
using ConsoleMarkup.RenderElement;

namespace ConsoleMarkup;

public class Page : IViewComponent
{
    public int Width { get; }
    public ConsoleColor ForegroundColor { get; }
    public ConsoleColor BackgroundColor { get; }
    
    private List<Block> children;
    public IEnumerable<IViewComponent> Children => children;

    public Page(int width, ConsoleColor foregroundColor, ConsoleColor backgroundColor, params Block[] children)
    {
        this.Width = width;
        this.ForegroundColor = foregroundColor;
        this.BackgroundColor = backgroundColor;
        this.children = children.Any() ? children.ToList() : new List<Block>();
    }
    
    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        return new PageRender(parentWidth, this);
    }
}