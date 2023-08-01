namespace Markup.Interface;

public interface IViewComponent
{
    internal IRenderElement CreateRender(int parentWidth);
}