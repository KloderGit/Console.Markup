namespace ConsoleMarkup;

public class View
{
    public static void PrintValue(string value, int limit, ConsoleColor color)
    {
        var storedColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        
        if (value.Length > limit-1)
        {
            var head = value[..(limit - 3)];
            var tail = value.Substring(limit - 3, 1);
            Console.Write(head);
            
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(tail);
            Console.Write("\u2026 ");
            Console.ForegroundColor = storedColor;
            
            return;
        }

        Console.Write(value.PadRight(limit));
        
        Console.ForegroundColor = storedColor;
    }

}