namespace Markup;

public class TableColumnWidthCalculator
{
    private readonly int allowWidth;
    private readonly int columnCount;
    private Stack<int> columnsWidth;

    public TableColumnWidthCalculator(int allowWidth, int columnCount)
    {
        this.allowWidth = allowWidth;
        this.columnCount = columnCount;

        GenerateWidthSize();
    }

    public Stack<int> GetWidth() => new Stack<int>(columnsWidth);

    private void GenerateWidthSize()
    {
        var headColumnsWidth = allowWidth / columnCount;
        var array = Enumerable.Repeat(headColumnsWidth, columnCount).ToArray();

        var rest = allowWidth - (headColumnsWidth * columnCount);

        for (int i = 0; i < rest; i++)
        {
            array[i]++;
        }

        columnsWidth = new Stack<int>(array);
    }
}