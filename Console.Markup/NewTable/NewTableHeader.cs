using Markup.Interface;

namespace Markup;

public class NewTableHeader : IViewComponent
{
    public List<TableColumn> Columns { get; }

    public NewTableHeader(TableColumn[] columns)
    {
        Columns = columns != default && columns.Any() 
            ? OrderColumns(columns.ToList()).ToList()
            : new List<TableColumn>();
    }
    
    private IEnumerable<TableColumn> OrderColumns(IEnumerable<TableColumn> columns)
    {
        var result = columns.Select( (x, idx) => new TableColumn(x.Marker, x.Content, idx+1));
        return result;
    }
    
    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        throw new NotImplementedException();
    }
}