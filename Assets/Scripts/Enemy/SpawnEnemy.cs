using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    float timeSince = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Spawn(25);
    }

    // Update is called once per frame
    void Update()
    {
        timeSince += Time.deltaTime;
        if (timeSince >= 30)
        {
            timeSince = 0f;
            Spawn(5);
        }
    }

    void Spawn(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Instantiate(enemyPrefab, new Vector3(Random.Range(-149, 149), 20f, Random.Range(-149, 149)), Quaternion.identity, transform);
        }
    }
}
