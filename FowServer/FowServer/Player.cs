using System.Numerics;

namespace FowServer
{
    class Player
    {
        public int id;
        public string username;

        public Player(int _id, string _username)
        {
            id = _id;
            username = _username;
        }
    }
}