namespace Blueprints.Factory.Static
{
    public class NetworkPlayer
    {
        public string Name { get; private set; }
        public int Health { get; private set; }

        private NetworkPlayer(string name, int health)
        {
            Name = name;
            Health = health;
        }

        public static NetworkPlayer CreateAndRegister(string name, int health)
        {
            var player = new NetworkPlayer(name, health);
            RegisterPlayer(player);
            return player;
        }
        
        private static void RegisterPlayer(NetworkPlayer player){ }
    }
}