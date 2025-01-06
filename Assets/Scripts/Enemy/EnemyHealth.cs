using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    /// <summary>
    /// Displays the EnemyHealth on the player view
    /// </summary>
    public Transform playerPos;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameStateSaver.Instance.GetSharedObjectByName("PlayerObject").transform;
    }
}
