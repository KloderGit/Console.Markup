using System.Collections;
using ConsoleMarkup.Interface;

namespace ConsoleMarkup;

public interface IBlockElement
{
    IEnumerable<IViewComponent> Children { get; }
}

public interface IViewComponent
{
    internal IRenderElement CreateRender(int parentWidth);
}