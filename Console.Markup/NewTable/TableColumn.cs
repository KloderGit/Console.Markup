using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

public class TableColumn : IViewComponent
{
    public int Order { get; set; }
    public string Marker { get; }

    public bool Delimiter { get; }
    public IViewComponent Content { get; }

    public TableColumn(string marker, IViewComponent content, bool delimiter = false)
    {
        this.Marker = marker;
        this.Content = content;
        Delimiter = delimiter;
    }

    internal TableColumn(string Header, IViewComponent Content, int order, bool delimiter = false) : this(Header, Content, delimiter)
    {
        Order = order;
    }

    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        return new TableColumnRender(parentWidth, this);
    }
}


internal class TableColumnRender : IRenderElement<TableColumn>
{
    public int Width { get; }
    public Dimension Dimension { get; }
    public TableColumn ViewComponent { get; }

    private IRenderElement ChildRender;

    public TableColumnRender(int allowedWidth, TableColumn component)
    {
        Width = allowedWidth;
        ViewComponent = component;
        CreateChildren();
        Dimension = GetDimension();
    }

    private void CreateChildren()
    {
        ChildRender = ViewComponent.Content.CreateRender(Width);
    }

    private Dimension GetDimension()
    {
        return ChildRender.Dimension;
    }

    public IEnumerable<string> Build()
    {
        return ChildRender.Build();
    }

}