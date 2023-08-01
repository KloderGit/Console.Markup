using System.Diagnostics;
using Markup.Interface;

namespace Markup;

public class Render
{
    public void Print(int sz, IViewComponent component)
    {
        var render = component.CreateRender(sz);

        var result = render.Build();
        
        foreach (var str in result)
        {
            Console.WriteLine(str);
        }
    }
    
    public void DebugPrint(int sz, IViewComponent component)
    {
        var render = component.CreateRender(sz);

        var result = render.Build();
        
        foreach (var str in result)
        {
            Debug.WriteLine(str);
        }
    }
}