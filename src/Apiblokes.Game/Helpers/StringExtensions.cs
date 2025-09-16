namespace Apiblokes.Game.Helpers;

public static class StringExtensions
{
    public static string Capitalize( this string? input )
    {
        if ( string.IsNullOrEmpty( input ) )
        {
            return string.Empty;
        }
        return $"{char.ToUpper( input[0] )}{( input[1..] ).ToLower()}";
    }
}
