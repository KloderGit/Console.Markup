using ConsoleMarkup.RenderElement;

namespace ConsoleMarkup.Interface;

internal interface IRenderElement
{
    int AllowedWidth { get; }
    Dimension Dimension { get; }
    IEnumerable<string> Build();
}

internal interface IRenderElement<T> : IRenderElement where T : IViewComponent
{
    T ViewComponent { get; }
}