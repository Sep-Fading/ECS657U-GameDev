using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float speed;
    public float minDistance = 3f;
    public float maxDistance = 10f;
    float idleTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.position;

        float distance = Vector3.Distance(transform.position, playerPos);

        if (distance > minDistance && distance <= maxDistance)
        {
            speed = 5f;
            transform.LookAt(player);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else if (distance <= minDistance)
        {
            //attack

        }
        else
        {
            idleTime += Time.deltaTime;
            speed = 1f;
            if (idleTime > 0.5f)
            {
                transform.Rotate(0, Random.Range(-50f, 50f), 0);
                idleTime = 0f;
            }
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
