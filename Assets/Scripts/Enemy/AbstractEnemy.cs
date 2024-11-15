using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractEnemy
{
    GameObject player;
    StatManager enemyStats;
    List<Action> attackPattern;
    EnemyState enemyState;
    float idleTime;

    AbstractEnemy()
    {
        player = GameStateSaver.Instance.GetSharedObjectByName("PlayerObject");
        enemyStats = new StatManager();
        enemyState = EnemyState.Idle;
        idleTime = 0f;
    }

    void idle() { }
    void attack() { }
    void block() { }
}
