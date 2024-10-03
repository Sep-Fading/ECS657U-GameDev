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
        public MultiplicativeStat armour;
        public MultiplicativeStat evasion;
        public MultiplicativeStat life;
        public MultiplicativeStat meleeDamage;
        public AdditiveStat blockEffect;

        // Constructor or initializer
        private PlayerStatManager()
        {
            armour = new MultiplicativeStat("Armour");
            evasion = new MultiplicativeStat("Evasion");
            life = new MultiplicativeStat("Life");
            meleeDamage = new MultiplicativeStat("MeleeDamage");
            blockEffect = new AdditiveStat("BlockEffect");
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
     Stat Abstract Classes
     Used to cut down redundant code.
     ------------------ */
    public class MultiplicativeStat
    {
        public float flat { get; set; }
        public float multiplier { get; set; }
        public float appliedTotal { get; set; }
        private string name { get; }

        public MultiplicativeStat(string name, float flat = 0f, float multiplier = 1f)
        {
            this.name = name;
            this.flat = flat;
            this.multiplier = multiplier;
            this.appliedTotal = flat * multiplier;
        }
        
        public new string ToString() => $"{flat},{multiplier},{name}";
        public string GetName() => name;
        
        // Used after every change to flat or multi to reflect correct stats.
        public void Recalculate()
        {
            this.appliedTotal = flat * multiplier;
        }
    }

    public class AdditiveStat
    {
        public float flat { get; set; }
        public float added { get; set; }
        private float appliedTotal { get; set; }
        private string name { get; }

        public AdditiveStat(string name, float flat = 0f, float added = 0f)
        {
            this.name = name;
            this.flat = flat;
            this.added = added;
            this.appliedTotal = flat + added;
        }
        
        public new string ToString() => $"{flat},{added},{name}";
        public string GetName() => name;
        
        // Used after every change to flat or added to reflect correct stats.
        public void Recalculate()
        {
            this.appliedTotal = flat + added;
        }
    }
}