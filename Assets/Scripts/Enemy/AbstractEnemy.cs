using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEnemy : MonoBehaviour
{
    GameObject player;
    StatManager enemyStats;
    List<Action> attackPattern;
    EnemyState enemyState;
    float idleTime;

    protected virtual void Awake()
    {
        player = GameStateSaver.Instance.GetSharedObjectByName("PlayerObject");
        enemyStats = new StatManager();
        enemyState = EnemyState.Idle;
        idleTime = 0f;
    }

    protected virtual void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                idle();
                break;
            case EnemyState.Triggered:
                trackPlayer();
                break;
            case EnemyState.Attack: 
                attack(); 
                break;
            default:
                break;
        }
    }

    void idle() { }
    void trackPlayer() { }
    void attack() { }
    void block() { }
}
