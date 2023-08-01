namespace Markup;

public static class Symbol
{
    public static char leftTopCornerSymbol = '┌';
    public static char rightTopCornerSymbol = '┐';
    public static char leftBottomCornerSymbol = '└';
    public static char rightBottomCornerSymbol = '┘';
    
    public static char horizontalSymbol = '─';
    public static char verticalSymbol = '│';
    
    public static char verticalTopSplitter = '┬';
    public static char verticalBottomSplitter = '┴';

    public static char headerUnderlineLeft = '╞';
    public static char headerUnderlineRight = '╡';
    public static char headerUnderlineSplitter = '╪';
    public static char headerUnderlineHorizontal = '═';
    
    public static string GenerateCharSeq(int length, char symbol)
    {
        return new string( Enumerable.Repeat(symbol, length).ToArray() );
    }
}