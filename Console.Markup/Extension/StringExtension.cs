namespace ConsoleMarkup.Extension;

public static class StringExtension
{
    public static string GenerateByChar(this string str, int length, char symbol)
    {
        str = new string( Enumerable.Repeat(symbol, length).ToArray() );
        return str;
    }
}