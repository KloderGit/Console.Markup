using ConsoleMarkup.Interface;

namespace ConsoleMarkup.RenderElement;

internal class BlockRender : IRenderElement<Block>
{
    public int AllowedWidth { get; set; }
    public Block ViewComponent { get; set; }
    public Dimension Dimension { get; set; }

    private IEnumerable<IRenderElement> ChildrenRenders = new List<IRenderElement>();

    public BlockRender(int allowedWidth, Block content)
    {
        AllowedWidth = allowedWidth;
        ViewComponent = content;

        CreateChildren();
        
        Dimension = GetDimension();
    }

    private void CreateChildren()
    {
        ChildrenRenders = ViewComponent.Children.Select(x => ((IViewComponent)x).CreateRender(AllowedWidth));
    }

    private Dimension GetDimension()
    {
        var childrenHeight = ChildrenRenders.Select(x => x.Dimension.Height).Sum();
        
        var dimension = new Dimension(AllowedWidth, childrenHeight);

        return dimension;
    }

    public IEnumerable<string> Build()
    {
        var layout = new List<string>();

        foreach (var childrenRender in ChildrenRenders)
        {
            var childrenBody = childrenRender.Build();
            layout.AddRange(childrenBody);
        }
        
        return layout;
    }
}