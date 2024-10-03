namespace GameplayMechanics.Character
{
    /* ------------
     The Stat Manager has instances of all player stats and
     manages them in one singleton class.
     -------------*/
    public class PlayerStatManager
    {
        // Singleton instance
        public static PlayerStatManager Instance { get; private set; }

        // Stat fields
        public Stat armour;
        public Stat evasion;
        public Stat life;
        public Stat meleeDamage;
        public Stat blockEffect;

        // Constructor or initializer
        private PlayerStatManager()
        {
            armour = new Stat("Armour");
            evasion = new Stat("Evasion");
            life = new Stat("Life");
            meleeDamage = new Stat("MeleeDamage");
            blockEffect = new Stat("BlockEffect");
        }

        // Method to initialize the Singleton
        public static PlayerStatManager Initialize()
        {
            if (Instance == null)
            {
                Instance = new PlayerStatManager();
            }
            return Instance;
        }
    }
    
    
    /* ------------------
     Stat Abstract Class
     Used to cut down redundant code.
     ------------------ */
    public class Stat
    {
        public float flat { get; set; }
        public float multiplier { get; set; }
        public float added { get; set; }
        public float appliedTotal { get; set; }
        private string name { get; }

        public Stat(string name, float flat = 0f, float multiplier = 1f,
            float added = 0f)
        {
            this.name = name;
            this.flat = flat;
            this.added = added;
            this.multiplier = multiplier;
            this.appliedTotal = (this.flat + this.added) * this.multiplier;
        }
        
        public new string ToString() => $"{this.flat},{this.multiplier},{this.name}";
        public string GetName() => this.name;
        
        // Used after every change to flat or multi to reflect correct stats.
        public void Recalculate()
        {
            this.appliedTotal = (this.flat + this.added) * this.multiplier;
        }
    }
}