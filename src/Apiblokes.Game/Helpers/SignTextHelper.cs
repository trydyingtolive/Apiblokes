namespace Apiblokes.Game.Helpers;

public static class SignTextHelper
{
    public static string[]? GetSignText( int x, int y )
    {
        //Start location sign
        if ( x == Constants.XStart && y == Constants.YStart )
        {
            return ["Welcome to Apiblokes.",
                    "If you need help type 'h' for all available commands",
                    "",
                    "Throughout this world you will find Apiblokes who you can battle and capture",
                    "Managers are common and can be captured without needing anything special.",
                    $"Network, System, Developer, and Help Desk Apiblokes will require a {Constants.Level2CatcherName}",
                    $"Do It All Apiblokes will require a {Constants.Level3CatcherName}",
                    "",
                    "At each corner of the map you will find vending machines for buying different items, coffee makers for healing your Apiblokes, and HR departments for trading your Apiblokes for Apibucks",
                    "",
                    "Source code at github.com/trydyingtolive/apiblokes"];
        }

        //Timely joke
        if ( x == 6 && y == 7 )
        {
            return ["Siiix Seeeeeven", "--Random Middle Schooler"];
        }

        //Zen of Python
        if ( x == 2 && y == 3 )
        {
            return [
"    Beautiful is better than ugly.",
"    Explicit is better than implicit.",
"    Simple is better than complex.",
"    Complex is better than complicated.",
"    Flat is better than nested.",
"    Sparse is better than dense.",
"    Readability counts.",
"    Special cases aren't special enough to break the rules.",
"    Although practicality beats purity.",
"    Errors should never pass silently.",
"    Unless explicitly silenced.",
"    In the face of ambiguity, refuse the temptation to guess.",
"    There should be one-- and preferably only one --obvious way to do it.",
"    Although that way may not be obvious at first unless you're Dutch.",
"    Now is better than never.",
"    Although never is often better than right now.",
"    If the implementation is hard to explain, it's a bad idea.",
"    If the implementation is easy to explain, it may be a good idea.",
"    Namespaces are one honking great idea – let's do more of those!"];
        }

        if ( x == 7 && y == 7 )
        {
            return ["There are two ways of constructing a software design. One way is to make it so simple that there are obviously no deficiencies. And the other way is to make it so complicated that there are no obvious deficiencies.",
                "The first method is far more difficult.",
                "- C.A.R. Hoare"];
        }

        if ( x == 5 && y == 1 )
        {
            return [
                "I’m just an old tortoise,",
                "I have to keep going to stay in front.",
                "--Cliff Young after winning a 544 mile race at 61 years old"
                ];
        }

        if ( x == 2 && y == 9 )
        {
            return ["Chasing failure took me further than chasing success ever did.",
            "--Ryan Leak"];
        }

        if ( x == 9 && y == 8 )
        {
            return ["Most good programmers do programming not because they expect to get paid or get adulation by the public, but because it is fun to program.",
            "--Linus Torvalds"];
        }

        if (x== 8 && y == 4 )
        {
            return ["I don't like to throw shade,",
                "but shade is due.",
                "--Mark Lee"
                ];
        }

        return null;
    }
}
