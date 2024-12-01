using Enemy;
using GameplayMechanics.Effects;
using InventoryScripts;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public Stat Armour;
        public Stat Evasion;
        public Stat Life;
        public Stat Stamina;
        public Stat MeleeDamage;
        public Stat BlockEffect;
        public Stat Bleed;
        public Stat DamageReduction;
        
        // Constant for diminishing return calculations used for armor:
        private const float K = 125f;
        public bool IsBlocking { set; get; }
        
        // Constructor
        private PlayerStatManager()
        {
            Armour = new Stat("Armour", 0f);
            Evasion = new Stat("Evasion", 0f);
            Life = new Stat("Life", 100f);
            Stamina = new Stat("Stamina", 100f);
            MeleeDamage = new Stat("MeleeDamage", 10f);
            BlockEffect = new Stat("BlockEffect", 0.05f);
            Bleed = new Stat("Bleed", MeleeDamage.GetAppliedTotal());
            DamageReduction = new Stat("DamageReduction", 0);
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

        public static void ResetInstance()
        {
            Instance = null;
        }
        public string GetPlayerStats()
        {
            return $" {Life.GetName()}, {Life.GetAppliedTotal()} \n" +
                   $" {Stamina.GetName()}, {Stamina.GetAppliedTotal()} \n" +
                   $" {Armour.GetName()}, {Armour.GetAppliedTotal()} \n" +
                   $" {Evasion.GetName()}, {Evasion.GetAppliedTotal()} \n" +
                   $" {BlockEffect.GetName()}, {BlockEffect.GetAppliedTotal()} \n" +
                   $" {MeleeDamage.GetName()}, {MeleeDamage.GetAppliedTotal()} \n" +
                   $" Bleed Chance, {Bleed.GetChance() * 100}%";
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

        public void DoDamage(EnemyController enemy)
        {
            StatManager enemyStatManager = enemy.GetStatManager();
            HealthBar enemyHealthBar = enemy.GetComponentInChildren<HealthBar>(); // Assuming HealthBar is a child of enemy
            enemyStatManager.Life.SetCurrent(
                enemyStatManager.Life.GetCurrent() - Instance.MeleeDamage.GetAppliedTotal());
            if (RollForBleed())
            {
                BleedEffect bleed =
                    new BleedEffect(3f, enemyStatManager, enemyHealthBar, enemy); 
                bleed.Apply();
            }
        }

        private bool RollForBleed()
        {
            float sucessChance = Instance.Bleed.GetChance();
            float roll = Random.Range(0f, 100f);
            return roll <= sucessChance*100;
        }

        private void PlayerDeathHandler()
        {
            // Restart Scene on death
            Debug.Log("Player Death");
            XpManager.ResetInstance();
            Inventory.ResetInstance();
            ResetInstance();
            GameStateSaver.ResetInstance();
            SceneManager.LoadScene("Tutorial");
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
        private float _chance;

        public Stat(string name, float flat, float multiplier = 1f,
            float added = 0f, float chance = 0f)
        {
            this._name = name;
            this._flat = flat;
            this._added = added;
            this._multiplier = multiplier;
            this._appliedTotal = (this._flat + this._added) * this._multiplier;
            this._chance = chance;
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

        public void SetChance(float chance)
        {
            this._chance = chance;
            this.Recalculate();
        }

        
        
        // Getters for stats:
        public float GetCurrent() => this._current;
        public float GetFlat() => this._flat;
        public float GetMultiplier() => this._multiplier;
        public float GetAdded() => this._added;
        public float GetAppliedTotal() => this._appliedTotal;
        public float GetChance() => this._chance;
    }
}