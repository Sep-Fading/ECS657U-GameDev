using GameplayMechanics.Effects;
using UnityEngine;

namespace InventoryScripts
{
    public class PrefabBank
    {
        // Start is called before the first frame update
        public static PrefabBank Instance { get; private set; }
        public GameObject[] swordPrefabs;
        public GameObject[] shieldPrefabs;
        public GameObject[] armourPrefabs;
        public GameObject[] healthPotionPrefabs;
        public GameObject[] greatSwordPrefabs;
        public GameObject[] axePrefabs;
    
        
        private PrefabBank(GameObject[] swordPrefabs,
            GameObject[] shieldPrefabs,
            GameObject[] armourPrefabs,
            GameObject[] healthPotionPrefabs,
            GameObject[] greatSwordPrefabs,
            GameObject[] axePrefabs)
        {
            this.swordPrefabs = swordPrefabs;
            this.shieldPrefabs = shieldPrefabs;
            this.armourPrefabs = armourPrefabs;
            this.healthPotionPrefabs = healthPotionPrefabs;
            this.greatSwordPrefabs = greatSwordPrefabs;
            this.axePrefabs = axePrefabs;
        }
        
        public static PrefabBank Initialize(GameObject[] swordPrefabs,
            GameObject[] shieldPrefabs,
            GameObject[] armourPrefabs,
            GameObject[] healthPotionPrefabs,
            GameObject[] greatSwordPrefabs,
            GameObject[] axePrefabs)
        {
            if (Instance == null)
            {
                Instance = new PrefabBank(swordPrefabs, shieldPrefabs, armourPrefabs, healthPotionPrefabs,
                    greatSwordPrefabs, axePrefabs);
            }
            return Instance;
        }
        
        public GameObject GetRandomSword()
        {
            return swordPrefabs[0];
        }
    
        public GameObject GetRandomShield()
        {
            return shieldPrefabs[0];
        }
    
        public GameObject GetRandomArmour()
        {
            return armourPrefabs[0];
        }
    
        public GameObject GetRandomHealthPotion()
        {
            return healthPotionPrefabs[UnityEngine.Random.Range(0, healthPotionPrefabs.Length)];
        }
    
        public GameObject GetRandomGreatSword()
        {
            return greatSwordPrefabs[0];
        }
    
        public GameObject GetRandomAxe()
        {
            return axePrefabs[UnityEngine.Random.Range(0, axePrefabs.Length)];
        }

        public GameObject GetRandomPrefab(EquipmentType equipmentType)
        {
            switch (equipmentType)
            {
                case EquipmentType.AXE:
                    return GetRandomAxe();
                case EquipmentType.GREATSWORD:
                    return GetRandomGreatSword();
                case EquipmentType.MAINHAND:
                    return GetRandomSword();
                case EquipmentType.OFFHAND:
                    return GetRandomShield();
                default:
                    return null;
            }
        }
    }
}