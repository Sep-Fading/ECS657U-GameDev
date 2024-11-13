using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    private float health = 100f;
    private Collider _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
    }


    public float GetHealth()
    {
        return health;
    }
    public void SetHealth(float value)
    {
        health = value;
        if(health <= 0)
        {
            Death();
        }   
    }
    private void Death()
    {
        
    }
}
