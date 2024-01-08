using Markup;
using Markup.Interface;

Console.WriteLine("Hello, World!");

var block = new Block(
        direction: Direction.Horizontal,
        border: new Border(top: true, left: true, right: true),
        margin: new Margin(Top: 3, Left: 5),
        padding: new Padding(Bottom: 1, Left: 3),
        children: new IViewComponent[]
        {
            new Block(
                border: new Border(right: true, bottom: true),
                margin: new Margin(Left: 2),
                children: new []{ new Content("1231 123123123 123123123 123123123 123123123 23123", true)}),
            new Block(
                border: new Border(right: true, bottom: true, left: true),
                //margin: new Margin(Left: 2),
                children: new []{ new Content("123123123")}),
            new Content("657567567"),
        }
    );
    
var render = new Render();

render.Print(100, block);