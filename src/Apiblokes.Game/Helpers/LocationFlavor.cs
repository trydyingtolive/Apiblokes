namespace Apiblokes.Game.Helpers;

public static class LocationFlavor
{
    public static string[] GetLocationFlavor( int x, int y )
    {
        //Starting Location
        if (x== Constants.XStart && y == Constants.YStart )
        {
            return ["You are in the center of the Apiblokes world.",
                "There is a sign with information on it.",
                "(type 'read sign' to read the sign)"];
        }

        //Special text
        if ( x == Constants.XMaximum && y == Constants.YMaximum )
        {
            return ["You emerge from the tangle of office life to find a lone vending machine",
                $"\"1 {Constants.Level2CatcherName} for 10 Apibucks\"",
                "\"Allows capturing of uncommon blokes\"",
                "(type 'use vending' to purchase)"
            ];
        }

        if ( x == Constants.XMaximum && y == Constants.YMinimum )
        {
            return ["You emerge from the tangle of office life to find a lone vending machine",
                $"\"1 {Constants.Level3CatcherName} for 10 Apibucks\"",
                "\"Allows capturing of uncommon blokes\"",
                "(type 'use vending' to purchase)"
            ];
        }

        if ( x == Constants.XMinimum && y == Constants.YMaximum )
        {
            return ["You emerge from the tangle of office life to find the HR department.",
                $"Here you can fire your Apiblokes in return for Apibucks.",
                "(type 'use hr on <bloke name>' to fire bloke)"
            ];
        }

        if ( x == Constants.XMinimum && y == Constants.YMinimum )
        {
            return ["You emerge from the tangle of office life to find the coffee maker.",
                $"Listed as a \"benefit\" of the job, you see a hoard of IT workers gathered around it's gurgling chassis. Their coffee mugs held over bowed heads, hoping for a dredge of  ",
                "(type 'use hr on <bloke name>' to fire bloke)"
            ];
        }

        //General text
        if ( x < ( Constants.XMaximum / 2 ) )
        {
            if ( y < ( Constants.YMaximum / 2 ) )
            {
                return ["You find yourself in the world of helpdesk. Unfinished tickets fall like rain on half assembled computers."];
            }
            else
            {
                return ["You find yourself in the world of system administration. Books on managing various systems clutter with floppy disks of deprecated systems."];
            }
        }
        else
        {
            if ( y < ( Constants.YMaximum / 2 ) )
            {
                return ["You find yourself in the world of networking. Firewalls block your way and unlabeled Cat 5 cables trip your feet."];
            }
            else
            {
                return ["You find yourself in the field of software development. Undocumented code litters the ground and a ghostly Kanban board floats in the sky."];
            }
        }
    }
}
