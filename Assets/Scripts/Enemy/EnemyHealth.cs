using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Transform playerPos;
    // Start is called before the first frame update
    void Start()
    {
        //playerPos = GameObject.FindWithTag("Player").transform;
        playerPos = GameStateSaver.Instance.GetSharedObjectByName("PlayerObject").transform;
    }
}
