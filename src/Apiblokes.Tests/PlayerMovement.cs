using Apiblokes.Model;

namespace Apiblokes.Tests
{
    public class PlayerMovement
    {

        [Test]
        public void Player_MoveNorth()
        {
            var player = new Player()
            {
                Y = 1
            };

            player.Move( "north" );

            Assert.That(player.Y, Is.EqualTo( 2 ) );
        }

        [Test]
        public void Player_MoveSouth()
        {
            var player = new Player()
            {
                Y = 1
            };

            player.Move( "south" );

            Assert.That( player.Y, Is.EqualTo( 0 ) );
        }

        [Test]
        public void Player_MoveEast()
        {
            var player = new Player()
            {
                X = 1
            };

            player.Move( "east" );

            Assert.That( player.X, Is.EqualTo( 2 ) );
        }

        [Test]
        public void Player_MoveWest()
        {
            var player = new Player()
            {
                X = 1
            };

            player.Move( "west" );

            Assert.That( player.X, Is.EqualTo( 0 ) );
        }
    }
}
