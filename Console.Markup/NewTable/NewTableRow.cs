using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

public class NewTableRow : IViewComponent
{
    public int Order { get; set; }
    public List<NewTableCell> Cells { get; set; }

    public NewTableRow(NewTableCell[] children = null)
    {
        Cells = children != null && children.Any() ? children.ToList() : new List<NewTableCell>();
    }

    internal NewTableRow(int order = 0, NewTableCell[] children = null) : this(children)
    {
        Order = order;
    }

    public void AddCell(NewTableCell cell) => Cells.Add(cell);
    
    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        return new NewTableRowRender(parentWidth, this);
    }
}


internal class NewTableRowRender : IRenderElement<NewTableRow>
{
    public int Width { get; }
    public Dimension Dimension { get; }
    public NewTableRow ViewComponent { get; }
    private IRenderElement ChildrenRender;

    public NewTableRowRender(int allowedWidth, NewTableRow component)
    {
        Width = allowedWidth;
        ViewComponent = component;
        var block = new Block(layout: Layout.Inline, 
            children: ViewComponent.Cells.ToArray());
        
        
        ChildrenRender = ((IViewComponent)block).CreateRender(Width);
        Dimension = ChildrenRender.Dimension with { Width = allowedWidth };
    }
    
    public IEnumerable<string> Build()
    {
        return ChildrenRender.Build();
    }

}