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

    Vector3 guardPosition;
    

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
        guardPosition = transform.position; // setting the orignal starting position to guard
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxDistance);

        ArrayList targets = new ArrayList();

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Enemy") && !targets.Contains(hit.gameObject))
            {
                targets.Add(hit);
            }
        }

        if (targets.Capacity > 0)
        {
            foreach (Collider target in targets)
            {
                Vector3 enemypos = target.transform.position;
                float distance = Vector3.Distance(transform.position, enemypos);
                bool obstacleBetween = Physics.Raycast(transform.position, (enemypos - transform.position).normalized, distance, obstacleLayer);
                bool inChaseRange = distance > minDistance && distance <= maxDistance && !obstacleBetween;
                bool inAttackRange = distance <= minDistance;
                
                triggered = inChaseRange || inAttackRange;
                
                if (triggered && target.gameObject.activeInHierarchy)
                {
                    //Debug.Log("Chasing: " + target.gameObject.name);
                    if (inChaseRange)
                    {
                        TrackEnemy(enemypos);
                    }
                    else if (inAttackRange)
                    {
                        Attack(target);
                    }
                }
                targets.Remove(target);
                //Debug.Log("Removed: " + target.gameObject.name);
            }
        }
        else
        {
            Idle();
            //Debug.Log("Idling rn boss, nothing to see here");
        }

    }

    void Idle()
    {
        if(Vector3.Distance(transform.position, guardPosition) > 3f)
        {
            GoToPosition(guardPosition); // return to guard position
        }
        else
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

    void GoToPosition(Vector3 guardPos)
    {
        transform.LookAt(guardPos);
        transform.position += transform.forward * (speed * Time.deltaTime);
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

