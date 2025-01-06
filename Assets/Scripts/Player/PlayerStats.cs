using GameplayMechanics.Character;
using UnityEngine;

namespace Player
{

    /// <summary>
    /// This script initialises our player stats
    /// Through the PlayerStatManager singleton.
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            /* --- Stat Manager Initialisation --- */
            PlayerStatManager.Initialize();
            /* --- Give base values to stats --- */
            PlayerStatManager.Instance.Life.SetFlat(100.0f);
            PlayerStatManager.Instance.Stamina.SetFlat(100.0f);
            PlayerStatManager.Instance.Armour.SetFlat(0.0f);
            PlayerStatManager.Instance.Evasion.SetFlat(0.0f);
            PlayerStatManager.Instance.BlockEffect.SetFlat(0.0f);
            PlayerStatManager.Instance.MeleeDamage.SetFlat(0.0f);

            /* --- XP Manager ---*/
            XpManager.Initialize();
        }
    }
}