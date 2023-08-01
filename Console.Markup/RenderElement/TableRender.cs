using Markup.Interface;

namespace Markup.RenderElement;

internal class TableRender : IRenderElement<Table>
{
    public int Width { get; }
    public Dimension Dimension { get; }
    public Table ViewComponent { get; }

    private IRenderElement RenderObject;

    public TableRender(int allowedWidth, Table content)
    {
        Width = allowedWidth;
        ViewComponent = content;

        CreateChildren();

        Dimension = GetDimension();
    }

    private void CreateChildren()
    {
        var rowsArray = new List<IViewComponent>();

        var headerComponents = ViewComponent.Headers.Select(x => x.Component).ToArray();
        var headerBlock = new Block(layout: Layout.Inline, children: headerComponents);
        
        rowsArray.Add(headerBlock);

        var markers = ViewComponent.Headers.Select(x => x.Marker).ToList();
        var filteredRowCellsDependOnColumns = ViewComponent.Rows.Select(
                x=> x.Cells.Where(c=>markers.Contains(c.Marker))
            );
        var rowsComponents = filteredRowCellsDependOnColumns
            .Select(x => x.Select(c => c.Component));

        foreach (var cells in rowsComponents)
        {
            var rowComponent = new Block(layout: Layout.Inline, children: cells.ToArray());
            rowsArray.Add(rowComponent);
        }

        var block = new Block(children: rowsArray.ToArray());
        var render = ((IViewComponent)block).CreateRender(Width);

        RenderObject = render;
    }    
    
    private Dimension GetDimension()
    {
        var childrenHeight = RenderObject.Dimension.Height;
        
        var dimension = new Dimension(Width, childrenHeight);

        return dimension;
    }
    
    public IEnumerable<string> Build()
    {
        var layout = RenderObject.Build();
        
        return layout;
    }

}