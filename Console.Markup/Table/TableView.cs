
namespace Markup;

public readonly record struct Header(string Value);
public readonly record struct Column(Header Title, int Limit, ConsoleColor color = ConsoleColor.Black, bool LineBreak = false );
public readonly record struct Cell(Header Header, IEnumerable<string> Values);

public class TableView : View
{
    private List<Column> columns = new List<Column>();
    
    private List<Row> rows = new List<Row>();

    public void AddHeader(Column column)
    {
        this.columns.Add(column);
    }

    public void AddRow(Row row)
    {
        rows.Add(row);
    }

    public void Print()
    {
        foreach (var column in columns)
        {
            View.PrintValue(column.Title.Value, column.Limit, column.color);
        }
        
        Console.WriteLine();
        
        foreach (var column in columns)
        {
            var sr = Enumerable.Repeat('-', column.Limit).ToArray();
            var str = new string(sr);
            Console.Write(str);
        }
        
        Console.WriteLine();
        
        
        foreach (var row in rows)
        {
            row.Print(columns);
        }
    }
}


public class Row
{
    private List<Cell> cells = new List<Cell>();

    public void AddCell(Cell cell) => cells.Add(cell);

    private IEnumerable<Cell> GetCellsForPresentedColumns(IEnumerable<Column> columns)
    {
        var filteredCellsOfThisRow = cells.Where(x => columns.Select(c => c.Title).Contains(x.Header));
        return filteredCellsOfThisRow;
    }

    private static int GetHeight(IEnumerable<PreparedCell> cells)
    {
        var height = cells.Select(x => x.Values.Count).Max();
        return height;
    }

    private PreparedCell Prepare(Cell cell, Column column)
    {
        PreparedCell result;
        
        if (column.LineBreak)
        {
            var listOfSeparatedStrings = new List<string>();
            
            foreach (var value in cell.Values)
            {
                if (value.Length < column.Limit)
                {
                    listOfSeparatedStrings.Add(value);
                    continue;
                }

                var index = 0;
                int substringCount = value.Length <= column.Limit - 2
                    ? 1
                    : (int)Math.Ceiling((double)value.Length / (column.Limit - 2));

                var substringArray = new List<string>();
                
                for (int i = 0; i < substringCount; i++)
                {
                    var sbString = value.Skip(index).Take(column.Limit-2).ToArray();
                    index += column.Limit-2;
                    var isLast = i + 1 == substringCount;
                    var str = isLast ? new string(sbString) : new string(sbString) + "\u2938";
                    substringArray.Add(str);
                }
                
                listOfSeparatedStrings.AddRange(substringArray);
            }

            listOfSeparatedStrings.Reverse();
            var stack = new Stack<string>(listOfSeparatedStrings);
            result = new PreparedCell(cell.Header, stack);
        }
        else
        {
            var strings = cell.Values;
            strings.Reverse();
            var stack = new Stack<string>(strings);
            result = new PreparedCell(cell.Header, stack);
        }

        return result;
    }

    private readonly record struct PreparedCell(Header header, Stack<string> Values);
    
    public void Print(IEnumerable<Column> columns)
    {
        var cellsForHeaders = GetCellsForPresentedColumns(columns);

        var preparedCells = new List<PreparedCell>();

        foreach (var column in columns)
        {
            var cells = cellsForHeaders
                .Where(x => x.Header == column.Title)
                .Select(x=>Prepare(x, column));
            preparedCells.AddRange(cells);
        }

        var height = GetHeight(preparedCells);

        for (int i = 0; i < height; i++)
        {
            foreach (var column in columns)
            {
                var cell = preparedCells.FirstOrDefault(x => x.header.Equals(column.Title));
                if (cell == default) View.PrintValue("", column.Limit, column.color);
                
                View.PrintValue(cell.Values.TryPop(out string result) ? result : "", column.Limit, column.color);
            }
            Console.WriteLine();
        }
    }
}