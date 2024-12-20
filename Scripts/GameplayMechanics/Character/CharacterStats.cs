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
        public  Stat armour;
        public  Stat evasion;
        public  Stat life;
        public  Stat stamina;
        public  Stat meleeDamage;
        public  Stat blockEffect;

        // Constructor
        private PlayerStatManager()
        {
            armour = new Stat("Armour");
            evasion = new Stat("Evasion");
            life = new Stat("Life");
            stamina = new Stat("Stamina");
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

        public string GetPlayerStats()
        {

            return $" {life.GetName()}, {life.GetAppliedTotal()} \n" +
                   $" {stamina.GetName()}, {stamina.GetAppliedTotal()} \n" +
                   $" {armour.GetName()}, {armour.GetAppliedTotal()} \n" +
                   $" {evasion.GetName()}, {evasion.GetAppliedTotal()} \n" +
                   $" {blockEffect.GetName()}, {blockEffect.GetAppliedTotal()} \n" +
                   $" {meleeDamage.GetName()}, {meleeDamage.GetAppliedTotal()} \n";

        }
    }
    
    
    /* ------------------
     Stat Abstract Class
     Used to cut down redundant code.
     ------------------ */
    public class Stat
    {
        private float flat;
        private float multiplier;
        private float added;
        private float appliedTotal;
        private string name;

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
        
        // Setters for stats:
        public void SetFlat(float flat)
        {
            this.flat = flat;
            this.Recalculate();
        }

        public void SetMultiplier(float multiplier)
        {
            this.multiplier = multiplier;
            this.Recalculate();
        }

        public void SetAdded(float added)
        {
            this.added = added;
            this.Recalculate();
        }

        public void SetAppliedTotal(float appliedTotal)
        {
            this.appliedTotal = appliedTotal;
            this.Recalculate();
        }
        
        // Getters for stats:
        public float GetFlat() => this.flat;
        public float GetMultiplier() => this.multiplier;
        public float GetAdded() => this.added;
        public float GetAppliedTotal() => this.appliedTotal;
    }
}