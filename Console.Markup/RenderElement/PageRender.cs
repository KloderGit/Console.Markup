using Markup.Interface;

namespace Markup.RenderElement;

internal record PageRender : IRenderElement<Page>
{
    public int Width { get; }
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