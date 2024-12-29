using UnityEngine;

namespace InventoryScripts
{
    public class PrefabBankInitializer : MonoBehaviour
    {
        public GameObject[] swordPrefabs;
        public GameObject[] shieldPrefabs;
        public GameObject[] armourPrefabs;
        public GameObject[] healthPotionPrefabs;
        public GameObject[] greatSwordPrefabs;
        public GameObject[] axePrefabs;
        // Start is called before the first frame update
        void Awake()
        {
            PrefabBank.Initialize(swordPrefabs,
                shieldPrefabs,
                armourPrefabs,
                healthPotionPrefabs,
                greatSwordPrefabs,
                axePrefabs);
        }
    }
}
