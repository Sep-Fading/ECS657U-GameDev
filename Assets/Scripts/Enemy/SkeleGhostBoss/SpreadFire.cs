using Enemy;
using GameplayMechanics.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadFire : MonoBehaviour
{
    float thrownTime;
    // Start is called before the first frame update
    void Start()
    {
        thrownTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (thrownTime >= 1f) Destroy(gameObject);
        else thrownTime += Time.deltaTime;

        if (PlayerStatManager.Instance != null)
        {
            if (!PlayerStatManager.Instance.IsBlocking && GameObject.FindWithTag("Shield") != null) GameObject.FindWithTag("Shield").GetComponentInChildren<Collider>().enabled = false;
            else if (GameObject.FindWithTag("Shield") != null) GameObject.FindWithTag("Shield").GetComponent<Collider>().enabled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Shield")
        {
            GameObject ball = Instantiate(Resources.Load("GhostBall") as GameObject, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z), transform.parent);
            ball.transform.LookAt(transform.parent);
            ball.GetComponent<Renderer>().enabled = true;
            ball.GetComponent<Rigidbody>().isKinematic = false;
            ball.GetComponent<Rigidbody>().AddForce(((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).normalized) * 20f, ForceMode.Impulse);
        }
        else if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Weapon"))
        { 
            if (PlayerStatManager.Instance != null)
            {
                PlayerStatManager.Instance.TakeDamage(gameObject.GetComponentInParent<AbstractEnemy>().stats.Damage.GetCurrent());
            }
        }
        else if (!(other.gameObject.tag == "Boss"))
        {
            Debug.Log("Spreading AOE");
            GameObject fireAOE = Instantiate(Resources.Load("FireAOE") as GameObject);
            fireAOE.transform.position = transform.position;
            if (transform.position.y <= -0.67f) fireAOE.transform.position = new Vector3(fireAOE.transform.position.x, -0.67f, fireAOE.transform.position.z);
        }

        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (!(collision.gameObject.CompareTag("Player") || collision.gameObject.transform.gameObject.CompareTag("ShieldSlot") || collision.gameObject.CompareTag("Weapon")))
        //{
        //    Debug.Log("Spreading AOE");
        //    GameObject fireAOE = Instantiate(Resources.Load("FireAOE") as GameObject);
        //    fireAOE.transform.position = transform.position;
        //    if (transform.position.y <= 0.1f) fireAOE.transform.position = new Vector3(fireAOE.transform.position.x, 0.15f, fireAOE.transform.position.z);
        //}
    }
}
