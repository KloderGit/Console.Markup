using Markup.Interface;

namespace Markup.RenderElement;

internal record BorderComponent(IViewComponent Child, bool Top = false, bool Bottom = false, bool Left = false, bool Right = false) 
    : IViewComponent
{
    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        return new BorderRender(parentWidth, this);
    }
}