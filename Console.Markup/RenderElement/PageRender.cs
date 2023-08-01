using ConsoleMarkup.Interface;

namespace ConsoleMarkup.RenderElement;

internal record PageRender : IRenderElement<Page>
{
    public int AllowedWidth { get; }
    public Dimension Dimension { get; }
    public Page ViewComponent { get; }

    public PageRender(int limitWidth, Page component)
    {
        
    }
    
    public IEnumerable<string> Build()
    {
        throw new NotImplementedException();
    }

    
    public IRenderElement Create(int allowedWidth, Page component)
    {
        throw new NotImplementedException();
    }
}