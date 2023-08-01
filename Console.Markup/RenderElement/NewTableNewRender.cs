using System.Text;
using Markup.Interface;

namespace Markup.RenderElement;

internal class NewTableNewRender : IRenderElement<NewTable>
{
    private TableColumnWidthCalculator cellWidthCalculator;
    
    public int Width { get; }
    public Dimension Dimension { get; }
    public NewTable ViewComponent { get; }

    private List<IRenderElement> ColumnRenders;

    private IEnumerable<IEnumerable<IRenderElement>> RowRenders;

    
    
    public NewTableNewRender(int allowedWidth, NewTable component)
    {
        Width = allowedWidth;
        ViewComponent = component;
        cellWidthCalculator = new TableColumnWidthCalculator(GetFreeSpaceForCells(), ViewComponent.Columns.Count);
        ColumnRenders = CreteColumnCellRenders().ToList();
    }

    private int GetTableBorderSpace()
    {
        var delimiter = ViewComponent.Delimiter;
        var border = ViewComponent.Border;
        
        var result = delimiter.vertical
            ? (ViewComponent.Columns.Count - 1) + (border.left ? 1 : 0) + (border.right ? 1 : 0)
            : (border.left ? 1 : 0) + (border.right ? 1 : 0);

        return result;
    }

    private int GetFreeSpaceForCells() => Width - GetTableBorderSpace();

    private IEnumerable<IRenderElement> CreteColumnCellRenders()
    {
        var result = CreateCellsRenders(
            ViewComponent.Columns.Select(x => x.Content))
            .ToList();
        return result;
    }

    private IEnumerable<IRenderElement> CreteRowCellRenders()
    {
        var array = ViewComponent.Rows.Select(x => x.Cells);
        
        var result = new List<IRenderElement>();

        foreach (var item in array)
        {
            var renders = CreateCellsRenders(item);
            result.AddRange(renders);
        }
        
        return result;
    }
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



    public IEnumerable<string> Build()
    {
        var header = CreateHeader();
        var body = CreateBody();
        
        return header.Concat(body);
    }

    private IEnumerable<string> CreateHeader()
    {
        var layout = new List<string>();

        var border = ViewComponent.Border;
        
        var left = Symbol.GenerateCharSeq((border.left ? 1 : 0), Symbol.verticalSymbol);
        var right = Symbol.GenerateCharSeq((border.right ? 1 : 0), Symbol.verticalSymbol);

        if (border.top)
        {
            var firstSymbol = border.left ? Symbol.leftTopCornerSymbol : Symbol.horizontalSymbol;
            var lastSymbol = border.right ? Symbol.rightTopCornerSymbol : Symbol.horizontalSymbol;

            var rs = firstSymbol.ToString();

            var columnWidth = cellWidthCalculator.GetWidth();
            
            for (int i = 0; i < ColumnRenders.Count; i++)
            {
                var sb = Symbol.GenerateCharSeq(columnWidth.Pop(), Symbol.horizontalSymbol);
                rs += sb;
                var l1 = rs.Length;
                var isLast = ColumnRenders.Count - 1 == i;
                if (isLast == false) rs = rs + Symbol.verticalTopSplitter;
                var l2 = rs.Length;
            }

            rs += lastSymbol.ToString();
            
            layout.Add(rs);
        }
        
        var columnCellAggregate = ColumnRenders
            .Select(x => new Stack<string>(x.Build().Reverse()))
            .ToList();

        var columnHeight = ColumnRenders.Select(x => x.Dimension.Height).Max();
        
        
        for (int i = 0; i < columnHeight; i++)
        {
            var columnWidth = cellWidthCalculator.GetWidth();

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
                    stringBuilder.Append(Symbol.GenerateCharSeq(columnWidth.Pop(), ' '));
                }

                var isLast = columnCellAggregate.Count - 1 == j;
                if (ViewComponent.Delimiter.vertical && isLast == false)
                {
                    stringBuilder.Append(Symbol.verticalSymbol);
                }
                
            }

            stringBuilder.Append(right);
            
            layout.Add(stringBuilder.ToString());
        }

        
        if (border.bottom)
        {
            var firstSymbol = border.left ? Symbol.headerUnderlineLeft : Symbol.horizontalSymbol;
            var lastSymbol = border.right ? Symbol.headerUnderlineRight : Symbol.horizontalSymbol;

            var rs = firstSymbol.ToString();

            var columnWidth = cellWidthCalculator.GetWidth();
            
            for (int i = 0; i < ColumnRenders.Count; i++)
            {
                var sb = Symbol.GenerateCharSeq(columnWidth.Pop(), '═');
                rs += sb;
                var l1 = rs.Length;
                var isLast = ColumnRenders.Count - 1 == i;
                if (isLast == false) rs = rs + "╪";
                var l2 = rs.Length;
            }

            rs += lastSymbol.ToString();
            
            layout.Add(rs);
        }
        
        return layout;
    }
    
    private IEnumerable<string> CreateBody()
    {
        var layout = new List<string>();
        
        // foreach (var rowRender in RowRenders)
        // {
        //     layout.AddRange(rowRender.Build());
        // }
        
        return layout;
    }


}