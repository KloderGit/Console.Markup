using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

public record Indent(IViewComponent Child, int Top = 0, int Bottom = 0, int Left = 0, int Right = 0) 
    : Bound(Top, Bottom, Left, Right), IViewComponent
{
    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        return new IndentRender(parentWidth, this);
    }
}
