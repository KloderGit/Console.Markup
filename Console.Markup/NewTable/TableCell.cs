using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

public class NewTableCell : IViewComponent
{
    public int Order { get; set; }
    
    public NewTableCell(string marker, IViewComponent content)
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

internal class NewTableCellRender : IRenderElement<NewTableCell>
{
    public int Width { get; }
    public Dimension Dimension { get; }
    public NewTableCell ViewComponent { get; }

    private IRenderElement ChildRender;

    public NewTableCellRender(int allowedWidth, NewTableCell component)
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