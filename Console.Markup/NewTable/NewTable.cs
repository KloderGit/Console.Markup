using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

public readonly record struct TableDelimiter(bool vertical = false, bool horizontal = false);

public class NewTable : IViewComponent
{
    public TableDelimiter Delimiter { get; set; }
    public BorderProperty Border { get; set; }
    
    public List<TableColumn> Columns { get; }
    public List<NewTableRow> Rows { get; }

    public NewTable(TableColumn[] columns = null, NewTableRow[] rows = null, TableDelimiter? delimiter = null, BorderProperty? border = null)
    {
        Border = border ?? new BorderProperty();
        Delimiter = delimiter ?? new TableDelimiter();
        
        Columns = columns != default && columns.Any() 
            ? OrderColumns(columns.ToList()).ToList()
            : new List<TableColumn>();
        
        Rows = rows != default && rows.Any() ? rows.ToList() : new List<NewTableRow>();
        OrderCellsInRow(Rows);
    }

    private IEnumerable<TableColumn> OrderColumns(IEnumerable<TableColumn> columns)
    {
        var result = columns.Select( (x, idx) => new TableColumn(x.Marker, x.Content, idx+1));
        return result;
    }

    private void OrderCellsInRow(IEnumerable<NewTableRow> rows)
    {
        var rowIndex = 0;
        foreach (var row in rows)
        {
            row.Order = ++rowIndex;
            row.Cells = GetFilteredCells(row.Cells).ToList();
            row.Cells.ForEach(OrderCell);
        }
    }

    IEnumerable<NewTableCell> GetFilteredCells(IEnumerable<NewTableCell> cells)
    {
        var filteredCellInRowDependOnAvailableColumns =
            cells.Where(IsCellRepresentedInColumns);
        return filteredCellInRowDependOnAvailableColumns;
    }

    private bool IsCellRepresentedInColumns(NewTableCell cell)
    {
        var columnMarkers = Columns.Select(x => x.Marker).ToList();
        return columnMarkers.Contains(cell.Marker);
    }

    private void OrderCell(NewTableCell cell)
    {
        var column = Columns.FirstOrDefault(x => x.Marker == cell.Marker);
        if (column != default) cell.Order = column.Order;
    }

    public void AddColumn(string header, IViewComponent component)
    {
        var order = Columns.Count() + 1;
        var column = new TableColumn(header, component, order);
        Columns.Add(column);
    }


    public int AddRow()
    {
        var number = Rows.Count() + 1;
        var row = new NewTableRow(number);
        Rows.Add(row);
        return number;
    }

    public void AddCellToRow(int rowNumber, NewTableCell cell)
    {
        var row = Rows.FirstOrDefault(x => x.Order == rowNumber);
        if (!IsCellRepresentedInColumns(cell)) return;
        OrderCell(cell);
        row?.Cells.Add(cell);
    }

    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        //return new NewTableRender(parentWidth, this);
        return new NewTableNewRender(parentWidth, this);
    }
}

internal class NewTableRender : IRenderElement<NewTable>
{
    public int Width { get; }
    public Dimension Dimension { get; }
    public NewTable ViewComponent { get; }

    private IRenderElement HeaderRender;
    private IEnumerable<IRenderElement> RowRenders;

    public NewTableRender(int allowedWidth, NewTable component)
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