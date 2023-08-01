// See https://aka.ms/new-console-template for more information

using Markup;

Console.WriteLine("Hello, World!");

var block = new Block(
        border: new Border(top: true, left: true),
        margin: new Margin(Top: 3, Left: 5),
        padding: new Padding(Bottom: 1, Left: 3),
        children: new []
        {
            new Content("123123123"),
            new Content("324234234"),
            new Content("657567567"),
        }
    );
    
var render = new Render();

render.Print(100, block);