using Markup.Interface;

namespace Markup.RenderElement;

internal class BlockRender : IRenderElement<Block>
{
    public int Width { get; set; }
    public Block ViewComponent { get; set; }
    public Dimension Dimension { get; set; }

    private IEnumerable<IRenderElement> ChildrenRenders = new List<IRenderElement>();
    
    public BlockRender(int allowedWidth, Block content)
    {
        Width = allowedWidth;
        ViewComponent = content;

        ChildrenRenders = CreateChildrenRender().ToList();
        
        Dimension = GetDimension();
    }

    private IEnumerable<IRenderElement> CreateChildrenRender()
    {
        return ViewComponent.Children.Select(x => x.CreateRender(Width));
    }

    private Dimension GetDimension()
    {
        var childrenHeight = ChildrenRenders.Select(x => x.Dimension.Height).Sum();
        
        var dimension = new Dimension(Width, childrenHeight);

        return dimension;
    }

    public IEnumerable<string> Build()
    {
        return ChildrenRenders.SelectMany(x => x.Build());
    }
}