using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

public class SimpleTableCell : IViewComponent
{
    public int Order { get; set; }
    
    public SimpleTableCell(string marker, IViewComponent content)
    {
        Marker = marker;
        Content = content;
    }

    public string Marker { get; }
    public IViewComponent Content { get; }
    
    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        return new NewTableCellRender(parentWidth, this);
    }
}

internal class NewTableCellRender : IRenderElement<SimpleTableCell>
{
    public int Width { get; }
    public Dimension Dimension { get; }
    public SimpleTableCell ViewComponent { get; }

    private IRenderElement ChildRender;

    public NewTableCellRender(int allowedWidth, SimpleTableCell component)
    {
        Width = allowedWidth;
        ViewComponent = component;
        ChildRender = ViewComponent.Content.CreateRender(allowedWidth);
        Dimension = ChildRender.Dimension;
    }

    public IEnumerable<string> Build()
    {
        return ChildRender.Build();
    }
}