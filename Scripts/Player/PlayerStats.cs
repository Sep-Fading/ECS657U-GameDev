using GameplayMechanics.Character;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            /* --- Stat Manager Initialisation --- */
            PlayerStatManager.Initialize();
            /* --- Give base values to stats --- */
            PlayerStatManager.Instance.life.SetFlat(100.0f);
            PlayerStatManager.Instance.stamina.SetFlat(100.0f);
            PlayerStatManager.Instance.armour.SetFlat(0.0f);
            PlayerStatManager.Instance.evasion.SetFlat(0.0f);
            PlayerStatManager.Instance.blockEffect.SetFlat(0.0f);
            PlayerStatManager.Instance.meleeDamage.SetFlat(20.0f);

            Debug.Log(PlayerStatManager.Instance.GetPlayerStats());
        }
    }
}
