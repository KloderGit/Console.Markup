using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

public record TableCell(string Marker, IViewComponent Component);

public record TableRow
{
    public int Number { get; init; }
    public List<TableCell> Cells { get; init; }
    
    public TableRow(int number = 0, TableCell[] cells = null)
    {
        this.Cells = cells != default && cells.Any() ? cells.ToList() : new List<TableCell>();
        this.Number = number;
    }
    
    public void Deconstruct(out List<TableCell> cells, out int number)
    {
        cells = this.Cells;
        number = this.Number;
    }
}

public class Table : IViewComponent
{
    public List<TableCell> Headers { get; }
    public List<TableRow> Rows { get; }

    public Table(TableCell[] headers = null, TableRow[] rows = null)
    {
        Headers = headers != default && headers.Any() ? headers.ToList() : new List<TableCell>();
        Rows = rows != default && rows.Any() ? rows.ToList() : new List<TableRow>();
    }

    public void AddHeader(string marker, IViewComponent component) => Headers.Add(new TableCell(marker, component));

    public int AddRow()
    {
        var number = Rows.Count() + 1;
        var row = new TableRow(Rows.Count() + 1);
        Rows.Add(row);
        return number;
    }

    public void AddCellToRow(int rowNumber, TableCell cell)
    {
        var row = Rows.FirstOrDefault(x => x.Number == rowNumber);
        if (row != default) row.Cells.Add(cell);
    }

    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        return new TableRender(parentWidth, this);
    }
}