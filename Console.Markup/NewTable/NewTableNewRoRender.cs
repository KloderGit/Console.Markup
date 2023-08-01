using System.Text;
using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

internal class NewTableNewRoRender : IRenderElement
{
    private readonly NewTableRow row;
    private readonly BorderProperty border;
    private readonly TableDelimiter delimiter;
    private TableColumnWidthCalculator cellWidthCalculator;

    private IEnumerable<IRenderElement> ChildrenRenders;

    public int Width { get; }
    public Dimension Dimension { get; }

    public NewTableNewRoRender(NewTableRow row, BorderProperty border, TableDelimiter delimiter)
    {
        this.row = row;
        this.border = border;
        this.delimiter = delimiter;
        cellWidthCalculator = new TableColumnWidthCalculator(Width, row.Cells.Count);

        ChildrenRenders = CreateChildren();
        Dimension = GetDimension();
    }

    private IEnumerable<IRenderElement> CreateChildren() => CreateCellsRenders(row.Cells);

    private IEnumerable<IRenderElement> CreateCellsRenders(IEnumerable<IViewComponent> components)
    {
        var cellWidth = cellWidthCalculator.GetWidth();

        var result = new List<IRenderElement>();
        foreach (var component in components)
        {
            var render = component.CreateRender(cellWidth.Pop());
            result.Add(render);
        }

        return result;
    }

    private Dimension GetDimension()
    {
        var cellsHeight = ChildrenRenders.Select(x => x.Dimension.Height).Max();

        return new Dimension(Width, cellsHeight);
    }

    public IEnumerable<string> Build()
    {
        var layout = new List<string>();

        var cellsHeight = ChildrenRenders.Select(x => x.Dimension.Height).Max();

        var left = Symbol.GenerateCharSeq((border.left ? 1 : 0), Symbol.verticalSymbol);
        var right = Symbol.GenerateCharSeq((border.right ? 1 : 0), Symbol.verticalSymbol);

        var columnCellAggregate = ChildrenRenders
            .Select(x => new Stack<string>(x.Build().Reverse()))
            .ToList();
        
        for (int i = 0; i < cellsHeight; i++)
        {
            var cellsWidth = cellWidthCalculator.GetWidth();

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(left);

            for (int j = 0; j < columnCellAggregate.Count; j++)
            {
                if (columnCellAggregate[j].TryPop(out string result))
                {
                    stringBuilder.Append(result);
                }
                else
                {
                    stringBuilder.Append(Symbol.GenerateCharSeq(cellsWidth.Pop(), ' '));
                }

                var isLast = columnCellAggregate.Count - 1 == j;
                if (delimiter.vertical && isLast == false)
                {
                    stringBuilder.Append(Symbol.verticalSymbol);
                }
            }

            stringBuilder.Append(right);
            
            layout.Add(stringBuilder.ToString());
        }

        return layout;
    }
}