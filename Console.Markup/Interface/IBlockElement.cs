namespace Markup.Interface;

public interface IBlockElement
{
    IEnumerable<IViewComponent> Children { get; }
}

