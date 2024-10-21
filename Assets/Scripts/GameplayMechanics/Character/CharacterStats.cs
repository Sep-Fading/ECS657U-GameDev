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
        public  Stat Armour;
        public  Stat Evasion;
        public  Stat Life;
        public  Stat Stamina;
        public  Stat MeleeDamage;
        public  Stat BlockEffect;
        private float _currentHealth;
        
        // Constant for diminishing return calculations used for armor:
        private const float k = 125f;
        public bool IsBlocking { set; get; }
        
        // Masteries / Notables Active
        public bool versMasteryActive;

        // Constructor
        private PlayerStatManager()
        {
            Armour = new Stat("Armour");
            Evasion = new Stat("Evasion");
            Life = new Stat("Life");
            Stamina = new Stat("Stamina");
            MeleeDamage = new Stat("MeleeDamage");
            BlockEffect = new Stat("BlockEffect");
            versMasteryActive = false;
            _currentHealth = Instance.GetHealthAsFloat();
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
            return $" {Life.GetName()}, {Life.GetAppliedTotal()} \n" +
                   $" {Stamina.GetName()}, {Stamina.GetAppliedTotal()} \n" +
                   $" {Armour.GetName()}, {Armour.GetAppliedTotal()} \n" +
                   $" {Evasion.GetName()}, {Evasion.GetAppliedTotal()} \n" +
                   $" {BlockEffect.GetName()}, {BlockEffect.GetAppliedTotal()} \n" +
                   $" {MeleeDamage.GetName()}, {MeleeDamage.GetAppliedTotal()} \n";
        }
        
        public string GetHealth() => this.Life.GetAppliedTotal().ToString();
        public float GetHealthAsFloat() => this.Life.GetAppliedTotal();

        public void TakeDamage(float damage)
        {
            // Armor Formula
            float effectiveDamage = damage / (1 + (Armour.GetAppliedTotal() / k));
            // Block effectiveness formula
            float effectiveDamageOnBlock = effectiveDamage * (1-BlockEffect.GetAppliedTotal());

            if (IsBlocking)
            {
                _currentHealth -= effectiveDamageOnBlock;
            }
            else
            {
                _currentHealth -= effectiveDamage;
            }
            
            
            if (_currentHealth <= 0)
            {
                PlayerDeathHandler();
            }
        }

        private void PlayerDeathHandler()
        {
            // TODO
            throw new System.NotImplementedException();
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