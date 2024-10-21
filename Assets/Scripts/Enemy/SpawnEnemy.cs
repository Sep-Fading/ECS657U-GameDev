using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Spawn(3);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
        {
            Spawn(3);
        }
    }

    void Spawn(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Instantiate(enemyPrefab, new Vector3(Random.Range(-23, 23), 1.8f, Random.Range(-23, 23)), Quaternion.identity, transform);
        }
    }
}
