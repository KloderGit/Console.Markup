using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

/// <summary>
/// Block unit of markup
/// </summary>
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
        
        if (Margin == null && Border == null && Padding == null)
        {
            IRenderElement render = direction switch
            {
                Direction.Horizontal => new SpanBlockRender(parentWidth, this),
                _ => new BlockRender(parentWidth, this)
            };

            return render;
        }
        
        IViewComponent body = new Block(children: this.children.ToArray(), direction: this.direction);

        if (Padding.HasValue)
        {
            var value = Padding.Value;
            var indent = new IndentComponent(
                Top: value.Top, Bottom: value.Bottom, Left: value.Left, Right: value.Right,
                Child: body
            );
            body = indent;
        }
        
        if (Border.HasValue)
        {
            var value = Border.Value;
            var indent = new BorderComponent(
                Top: value.top, Bottom: value.bottom, Left: value.left, Right: value.right,
                Child: body
            );
            body = indent;
        }
        
        if (Margin.HasValue)
        {
            var value = Margin.Value;
            var indent = new IndentComponent(
                Top: value.Top, Bottom: value.Bottom, Left: value.Left, Right: value.Right,
                Child: body
            );
            body = indent;
        }

        return body.CreateRender(parentWidth);
    }
}

public enum Direction
{
    Vertical,
    Horizontal,
    Wrap
}