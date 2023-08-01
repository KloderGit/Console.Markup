using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

public readonly record struct TableDelimiter(bool vertical = false, bool horizontal = false);

public class SimpleTable : IViewComponent
{
    public TableDelimiter Delimiter { get; set; }
    public BorderProperty Border { get; set; }
    
    public List<SimpleTableColumn> Columns { get; }
    public List<SimpleTableRow> Rows { get; }

    public SimpleTable(SimpleTableColumn[] columns = null, SimpleTableRow[] rows = null, TableDelimiter? delimiter = null, BorderProperty? border = null)
    {
        Border = border ?? new BorderProperty();
        Delimiter = delimiter ?? new TableDelimiter();
        
        Columns = columns != default && columns.Any() 
            ? OrderColumns(columns.ToList()).ToList()
            : new List<SimpleTableColumn>();
        
        Rows = rows != default && rows.Any() ? rows.ToList() : new List<SimpleTableRow>();
        OrderCellsInRow(Rows);
    }

    private IEnumerable<SimpleTableColumn> OrderColumns(IEnumerable<SimpleTableColumn> columns)
    {
        var result = columns.Select( (x, idx) => new SimpleTableColumn(x.Marker, x.Content, idx+1));
        return result;
    }

    private void OrderCellsInRow(IEnumerable<SimpleTableRow> rows)
    {
        var rowIndex = 0;
        foreach (var row in rows)
        {
            row.Order = ++rowIndex;
            row.Cells = GetFilteredCells(row.Cells).ToList();
            row.Cells.ForEach(OrderCell);
        }
    }

    IEnumerable<SimpleTableCell> GetFilteredCells(IEnumerable<SimpleTableCell> cells)
    {
        var filteredCellInRowDependOnAvailableColumns =
            cells.Where(IsCellRepresentedInColumns);
        return filteredCellInRowDependOnAvailableColumns;
    }

    private bool IsCellRepresentedInColumns(SimpleTableCell cell)
    {
        var columnMarkers = Columns.Select(x => x.Marker).ToList();
        return columnMarkers.Contains(cell.Marker);
    }

    private void OrderCell(SimpleTableCell cell)
    {
        var column = Columns.FirstOrDefault(x => x.Marker == cell.Marker);
        if (column != default) cell.Order = column.Order;
    }

    public void AddColumn(string header, IViewComponent component)
    {
        var order = Columns.Count() + 1;
        var column = new SimpleTableColumn(header, component, order);
        Columns.Add(column);
    }


    public int AddRow()
    {
        var number = Rows.Count() + 1;
        var row = new SimpleTableRow(number);
        Rows.Add(row);
        return number;
    }

    public void AddCellToRow(int rowNumber, SimpleTableCell cell)
    {
        var row = Rows.FirstOrDefault(x => x.Order == rowNumber);
        if (!IsCellRepresentedInColumns(cell)) return;
        OrderCell(cell);
        row?.Cells.Add(cell);
    }

    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        return new NewTableRender(parentWidth, this);
        //return new NewTableNewRender(parentWidth, this);
    }
}

internal class NewTableRender : IRenderElement<SimpleTable>
{
    public int Width { get; }
    public Dimension Dimension { get; }
    public SimpleTable ViewComponent { get; }

    private IRenderElement HeaderRender;
    private IEnumerable<IRenderElement> RowRenders;

    public NewTableRender(int allowedWidth, SimpleTable component)
    {
        Width = allowedWidth;
        ViewComponent = component;

        var headerUnderline = new Content(Symbol.GenerateCharSeq(Width, '='));

        var columnsBlock = new Block(layout: Layout.Inline, children: ViewComponent.Columns.ToArray());
        
        var headerBlock = new Block(children: new IViewComponent[]{ columnsBlock, headerUnderline });

        HeaderRender = ((IViewComponent)headerBlock).CreateRender(allowedWidth);
        RowRenders = ViewComponent.Rows.Select(x => ((IViewComponent)x).CreateRender(allowedWidth));
    }
    
    public IEnumerable<string> Build()
    {
        var result = HeaderRender.Build().ToList();
        foreach (var rowRender in RowRenders)
        {
            var rowRenderValue = rowRender.Build();
            result.AddRange(rowRenderValue);
        }

        return result;
    }

}