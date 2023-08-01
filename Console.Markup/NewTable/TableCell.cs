using ConsoleMarkup.Interface;
using ConsoleMarkup.RenderElement;

namespace ConsoleMarkup;

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
    public int AllowedWidth { get; }
    public Dimension Dimension { get; }
    public NewTableCell ViewComponent { get; }

    private IRenderElement ChildRender;

    public NewTableCellRender(int allowedWidth, NewTableCell component)
    {
        AllowedWidth = allowedWidth;
        ViewComponent = component;
        ChildRender = ViewComponent.Content.CreateRender(allowedWidth);
        Dimension = ChildRender.Dimension;
    }

    public IEnumerable<string> Build()
    {
        return ChildRender.Build();
    }
}