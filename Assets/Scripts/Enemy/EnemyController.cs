using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public Collider wallCollider;
    public float speed;
    public float minDistance = 3f;
    public float maxDistance = 10f;
    float idleTime = 0f;
    public LayerMask obstacleLayer;
    float randomRot;
    private bool isRotating = false; // To track whether the enemy is currently rotating

    // Start is called before the first frame update
    void Start()
    {
        speed = 4f;
        randomRot = Random.Range(-360f, 360f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.position;
        float distance = Vector3.Distance(transform.position, playerPos);
        bool obstacleBetween = Physics.Raycast(transform.position, (playerPos - transform.position).normalized, distance, obstacleLayer);

        if (distance > minDistance && distance <= maxDistance && !obstacleBetween)
        {
            speed = 4.5f;
            isRotating = false;
            //var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
            //targetRotation.y = 0;
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            transform.LookAt(playerPos);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else if (distance <= minDistance)
        {
            //attack
        }
        else
        {
            idle();
        }
    }

    void idle()
    {
        idleTime += Time.deltaTime;
        speed = 1f;
        if (!isRotating)
        {
            StartCoroutine(SmoothTurn(Random.Range(-360f, 360f), 5f));
        }
        else if (idleTime > 2f)
        {
            //var targetRotation = Quaternion.LookRotation(transform.position - target);
            //Debug.Log(targetRotation);
            //Quaternion axisRotation = Quaternion.AngleAxis(90, Vector3.up);
            //Debug.Log(new Quaternion(0f, 1f, 0f, 1f) * Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime));
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            //transform.rotation = axisRotation * transform.rotation;
            idleTime = 0f;
        }
        transform.position += transform.forward * speed * Time.deltaTime;
        //target = new Vector3(Random.Range(90f, 360f), 0, Random.Range(90f, 360f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" && !isRotating)
        {
            Debug.Log("Collided with wall");
            StartCoroutine(SmoothTurn(Random.Range(90f, 180f), 1f));
        }
    }

    IEnumerator SmoothTurn(float angle, float duration)
    {
        isRotating = true;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, angle, 0);

        float time = 0;
        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;

        isRotating = false;
    }
}
