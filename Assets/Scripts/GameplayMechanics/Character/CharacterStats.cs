using UnityEngine;

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
        
        // Constant for diminishing return calculations used for armor:
        private const float K = 125f;
        public bool IsBlocking { set; get; }
        
        // Masteries / Notables Active
        public bool VersMasteryActive;

        // Constructor
        private PlayerStatManager()
        {
            Armour = new Stat("Armour");
            Evasion = new Stat("Evasion");
            Life = new Stat("Life");
            Stamina = new Stat("Stamina");
            MeleeDamage = new Stat("MeleeDamage");
            BlockEffect = new Stat("BlockEffect");
            VersMasteryActive = false;
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
        public void TakeDamage(float damage)
        {
            // Armor Formula
            float effectiveDamage = damage / (1 + (Armour.GetAppliedTotal() / K));
            // Block effectiveness formula
            float effectiveDamageOnBlock = effectiveDamage * (1-BlockEffect.GetAppliedTotal());

            if (IsBlocking)
            {
                Life.SetCurrent(Life.GetCurrent() - effectiveDamageOnBlock);
            }
            else
            {
                Life.SetCurrent(Life.GetCurrent() - effectiveDamage);
            }
            
            
            if (Life.GetCurrent() <= 0)
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
        private float _flat;
        private float _multiplier;
        private float _added;
        private float _appliedTotal;
        private string _name;
        private float _current;

        public Stat(string name, float flat = 100f, float multiplier = 1f,
            float added = 0f)
        {
            this._name = name;
            this._flat = flat;
            this._added = added;
            this._multiplier = multiplier;
            this._appliedTotal = (this._flat + this._added) * this._multiplier;
            _current = this._appliedTotal;
        }
        
        public new string ToString() => $"{this._flat},{this._multiplier},{this._name}";
        public string GetName() => this._name;
        
        // Used after every change to flat or multi to reflect correct stats.
        public void Recalculate()
        {
            bool currentNeedsUpdate = false || _current == _appliedTotal;
            this._appliedTotal = (this._flat + this._added) * this._multiplier;
            if (currentNeedsUpdate)
            {
                SetCurrent(this._appliedTotal);
            }
        }
        
        // Setters for stats:
        public void SetFlat(float flat)
        {
            this._flat = flat;
            this.Recalculate();
        }

        public void SetMultiplier(float multiplier)
        {
            this._multiplier = multiplier;
            this.Recalculate();
        }

        public void SetAdded(float added)
        {
            this._added = added;
            this.Recalculate();
        }

        public void SetAppliedTotal(float appliedTotal)
        {
            if (_current == _appliedTotal)
            { 
                _current = appliedTotal;
            }
            this._appliedTotal = appliedTotal;
            
            this.Recalculate();
        }

        public void SetCurrent(float newVal)
        {
            if (newVal > this._appliedTotal)
            {
                _current = this._appliedTotal;
            }
            else
            {
                _current = Mathf.Max(0, newVal);
            }
        }

        
        
        // Getters for stats:
        public float GetCurrent() => this._current;
        public float GetFlat() => this._flat;
        public float GetMultiplier() => this._multiplier;
        public float GetAdded() => this._added;
        public float GetAppliedTotal() => this._appliedTotal;
    }
}