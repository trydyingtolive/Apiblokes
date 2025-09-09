using System.Runtime.CompilerServices;

namespace Apiblokes.Helpers
{
    public static class ContextHelper
    {
        public static string GetPlayerId( this HttpContext context )
        {
            var auth = context.Request.Headers.Authorization.FirstOrDefault();

            if ( string.IsNullOrEmpty( auth ) )
            {
                return string.Empty;
            }

            return auth.Replace( "Bearer ", "" );
        }
    }
}
