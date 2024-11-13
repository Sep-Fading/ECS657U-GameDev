using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    float timeSince = 0f;

    [SerializeField] private float xRangeMin = 125f;
    [SerializeField] private float xRangeMax = 280f;
    [SerializeField] private float yRangeMin = 125f;
    [SerializeField] private float yRangeMax = 350f;
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
            Instantiate(enemyPrefab, new Vector3(Random.Range(xRangeMin, xRangeMax), 20f, Random.Range(yRangeMin, yRangeMax)), Quaternion.identity, transform);
        }
    }
}
