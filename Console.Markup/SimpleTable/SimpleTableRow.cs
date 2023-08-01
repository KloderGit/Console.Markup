using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

public class SimpleTableRow : IViewComponent
{
    public int Order { get; set; }
    public List<SimpleTableCell> Cells { get; set; }

    public SimpleTableRow(SimpleTableCell[] children = null)
    {
        Cells = children != null && children.Any() ? children.ToList() : new List<SimpleTableCell>();
    }

    internal SimpleTableRow(int order = 0, SimpleTableCell[] children = null) : this(children)
    {
        Order = order;
    }

    public void AddCell(SimpleTableCell cell) => Cells.Add(cell);
    
    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        return new NewTableRowRender(parentWidth, this);
    }
}


internal class NewTableRowRender : IRenderElement<SimpleTableRow>
{
    public int Width { get; }
    public Dimension Dimension { get; }
    public SimpleTableRow ViewComponent { get; }
    private IRenderElement ChildrenRender;

    public NewTableRowRender(int allowedWidth, SimpleTableRow component)
    {
        Width = allowedWidth;
        ViewComponent = component;
        var block = new Block(direction: Direction.Horizontal, 
            children: ViewComponent.Cells.ToArray());
        
        
        ChildrenRender = ((IViewComponent)block).CreateRender(Width);
        Dimension = ChildrenRender.Dimension with { Width = allowedWidth };
    }
    
    public IEnumerable<string> Build()
    {
        return ChildrenRender.Build();
    }

}