using GameplayMechanics.Effects;
using UnityEngine;

namespace InventoryScripts
{
    /// <summary>
    /// Storage of the main Prefabs we use for the player
    /// Allows for generation of random prefabs when generated
    /// </summary>
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
            return swordPrefabs[UnityEngine.Random.Range(0, swordPrefabs.Length)];
        }
    
        public GameObject GetRandomShield()
        {
            return shieldPrefabs[UnityEngine.Random.Range(0, shieldPrefabs.Length)];
        }
    
        public GameObject GetRandomArmour()
        {
            return armourPrefabs[UnityEngine.Random.Range(0, armourPrefabs.Length)];
        }
    
        public GameObject GetRandomHealthPotion()
        {
            return healthPotionPrefabs[UnityEngine.Random.Range(0, healthPotionPrefabs.Length)];
        }
    
        public GameObject GetRandomGreatSword()
        {
            return greatSwordPrefabs[UnityEngine.Random.Range(0, greatSwordPrefabs.Length)];
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