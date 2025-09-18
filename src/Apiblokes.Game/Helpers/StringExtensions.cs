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

    public static string Truncate( this string? value, int maxLength )
    {
        if ( string.IsNullOrEmpty( value ) )
        {
            return string.Empty;
        }
        return value.Length <= maxLength ? value : value.Substring( 0, maxLength );
    }

}
