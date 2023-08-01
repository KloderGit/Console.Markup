using System.Diagnostics;
using System.Drawing;
using ConsoleMarkup.Interface;
using ConsoleMarkup.RenderElement;

namespace ConsoleMarkup;

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