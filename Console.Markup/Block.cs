using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

public class Block : IBlockElement, IViewComponent
{
    private readonly Direction direction;
    private readonly List<IViewComponent> children;
    public IEnumerable<IViewComponent> Children => children;
    public Border? Border { get; }
    public Margin? Margin { get; }
    public Padding? Padding { get; }

    public Block(Direction direction = Direction.Vertical, Border? border = null, Margin? margin = null, 
        Padding? padding = null, params IViewComponent[] children)
    {
        Margin = margin;
        Padding = padding;
        Border = border;
        this.direction = direction;
        this.children = children.ToList();
    }
    
    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        IRenderElement render = direction switch
        {
            Direction.Horizontal => new SpanBlockRender(parentWidth, this),
            _ => new BlockRender(parentWidth, this)
        };
        
        return render;
    }
}

public enum Direction
{
    Vertical,
    Horizontal,
    Wrap
}