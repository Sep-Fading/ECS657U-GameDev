using System.Collections;
using Enemy;
using GameplayMechanics.Character;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;


public class AllyController : MonoBehaviour
{
    public Collider wallCollider;
    public float speed;
    public float maxDistance = 30f;
    public float minDistance = 1.5f;
    float idleTime = 0f;
    public LayerMask obstacleLayer;
    float randomRot;
    private bool isRotating = false;
    private bool triggered;
    public Material defaultMaterial;
    Renderer allyRenderer;
    

    // Start is called before the first frame update
    void Start()
    {
        speed = 4f;
        triggered = false;
        //enemyRenderer = GetComponent<Renderer>();
        randomRot = Random.Range(-360f, 360f);
    }

    private void Awake()
    {
 
        allyRenderer = GetComponent<Renderer>();
        allyRenderer.material = defaultMaterial;
        wallCollider = GameObject.FindWithTag("Environment").GetComponent<Collider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxDistance);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector3 enemypos = hit.transform.position;
                float distance = Vector3.Distance(transform.position, enemypos);
                bool obstacleBetween = Physics.Raycast(transform.position, (enemypos - transform.position).normalized, distance, obstacleLayer);
                bool inChaseRange = distance > minDistance && distance <= maxDistance && !obstacleBetween;
                bool inAttackRange = distance <= minDistance;
                
                triggered = inChaseRange || inAttackRange;
                
                if (triggered)
                {
                    if (inChaseRange)
                    {
                        TrackEnemy(enemypos);
                    }
                    else if (inAttackRange)
                    {
                        Attack(hit);
                    }
                }
            }
            else
            {
                Idle();
            }
        }
        
        
    }

    void Idle()
    {
        triggered = false;
        idleTime += Time.deltaTime;
        speed = 1f;
        if (!isRotating)
        {
            StartCoroutine(SmoothTurn(Random.Range(-360f, 360f), 5f));
        }
        else if (idleTime > 2f)
        {
            idleTime = 0f;
        }
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    void TrackEnemy(Vector3 enemypos) 
    {
        speed = 6f;
        isRotating = false;
        triggered = true;
        transform.LookAt(enemypos);
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    void Attack(Collider hit) 
    { 
            hit.GetComponent<EnemyController>().DestroyEnemy();
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !isRotating)
        {
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

