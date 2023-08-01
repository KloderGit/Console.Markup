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

        ChildrenRenders = CreateRender().ToList();
        
        Dimension = GetDimension();
    }

    private IEnumerable<IRenderElement> CreateRender()
    {
        var margin = ViewComponent.Margin;
        var border = ViewComponent.Border;
        var padding = ViewComponent.Padding;

        if (margin == null && border == null && padding == null)
        {
            return ViewComponent.Children.Select(x => x.CreateRender(Width));
        }

        IViewComponent body = new Block(children: ViewComponent.Children.ToArray());
        
        if (padding.HasValue)
        {
            var value = padding.Value;
            var indent = new IndentComponent(
                Top: value.Top, Bottom: value.Bottom, Left: value.Left, Right: value.Right,
                Child: body
            );
            body = indent;
        }
        
        if (border.HasValue)
        {
            var value = border.Value;
            var indent = new BorderComponent(
                Top: value.top, Bottom: value.bottom, Left: value.left, Right: value.right,
                Child: body
            );
            body = indent;
        }
        
        if (margin.HasValue)
        {
            var value = margin.Value;
            var indent = new IndentComponent(
                Top: value.Top, Bottom: value.Bottom, Left: value.Left, Right: value.Right,
                Child: body
            );
            body = indent;
        }

        var render = new List<IRenderElement>() { body.CreateRender(Width) };
        
        return render;
    }

    private Dimension GetDimension()
    {
        var childrenHeight = ChildrenRenders.Select(x => x.Dimension.Height).Sum();
        
        var dimension = new Dimension(Width, childrenHeight);

        return dimension;
    }

    public IEnumerable<string> Build()
    {
            Console.WriteLine(Dimension.Width + " | " + Dimension.Height);
        
        return ChildrenRenders.SelectMany(x => x.Build());
    }
}