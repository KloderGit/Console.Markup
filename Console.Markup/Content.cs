using Markup.Interface;
using Markup.RenderElement;

namespace Markup;

public class Content: IViewComponent
{
    public bool IsMultiline { get; }
    public string Value { get; }

    public Content(string value, bool isMultiline = false)
    {
        this.IsMultiline = isMultiline;
        this.Value = value;
    }
    
    IRenderElement IViewComponent.CreateRender(int parentWidth)
    {
        return new ContentRender(parentWidth, this);
    }
}