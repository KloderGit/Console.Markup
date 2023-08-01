using System.Diagnostics;
using ConsoleMarkup.Interface;
using ConsoleMarkup.RenderElement;

namespace ConsoleMarkup;

public class Block : IBlockElement, IViewComponent
{
    private readonly Layout layout;
    private readonly List<IViewComponent> children;
    public IEnumerable<IViewComponent> Children => children;
    
    public Block(
        Layout layout = Layout.Block,
        params IViewComponent[] children)
    {
        this.layout = layout;
        this.children = children.ToList();
    }
    
    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        IRenderElement render = layout switch
        {
            Layout.Inline => new SpanBlockRender(parentWidth, this),
            _ => new BlockRender(parentWidth, this)
        };
        
        return render;
    }
}

public enum Layout
{
    Block,
    Inline,
    Wrap
}