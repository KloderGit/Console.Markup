using Markup.RenderElement;

namespace Markup.Interface;

internal interface IRenderElement
{
    int Width { get; }
    Dimension Dimension { get; }
    IEnumerable<string> Build();
}

internal interface IRenderElement<T> : IRenderElement where T : IViewComponent
{
    T ViewComponent { get; }
}