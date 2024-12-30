using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) Spawn(150);
        if (SceneManager.GetActiveScene().buildIndex == 2) Spawn(150);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Spawn(int num)
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                int w1box1num = num * 2 / 3;
                int w1box2num = num * 1 / 3;
                for (int i = 0; i <= w1box1num; i++)
                {
                    if (Random.value <= 0.5)
                    {
                        Instantiate(Resources.Load("Orc"), new Vector3(Random.Range(40f, 200f), 10f, Random.Range(70f, 370f)), transform.localRotation, transform);
                    }
                    else
                    {
                        Instantiate(Resources.Load("MushroomKing"), new Vector3(Random.Range(40f, 200f), 10f, Random.Range(70f, 370f)), transform.localRotation, transform);
                    }
                }
                for (int i = 0; i <= w1box2num; i++)
                {
                    Instantiate(Resources.Load("Orc"), new Vector3(Random.Range(200f, 320f), 3f, Random.Range(145f, 245f)), transform.localRotation, transform);
                }
                break;
            case 2:
                int w2box1num = num / 3;
                int w2box2num = num / 3;
                int w2box3num = num / 3;
                for (int i = 0; i <= w2box1num; i++)
                {
                    if (Random.value <= 0.3)
                    {
                        Instantiate(Resources.Load("Cactoro"), new Vector3(Random.Range(150f, 400f), 3f, Random.Range(115f, 220f)), transform.localRotation, transform);
                    }
                    else if (Random.value <= 0.6)
                    {
                        Instantiate(Resources.Load("Spider"), new Vector3(Random.Range(150f, 400f), 3f, Random.Range(115f, 220f)), transform.localRotation, transform);
                    }
                    else
                    {
                        Instantiate(Resources.Load("Frog"), new Vector3(Random.Range(150f, 400f), 3f, Random.Range(115f, 220f)), transform.localRotation, transform);
                    }
                }
                for (int i = 0; i <= w2box1num; i++)
                {
                    if (Random.value <= 0.3)
                    {
                        Instantiate(Resources.Load("Cactoro"), new Vector3(Random.Range(150f, 400f), 3f, Random.Range(115f, 220f)), transform.localRotation, transform);
                    }
                    else if (Random.value <= 0.6)
                    {
                        Instantiate(Resources.Load("Spider"), new Vector3(Random.Range(150f, 400f), 3f, Random.Range(115f, 220f)), transform.localRotation, transform);
                    }
                    else
                    {
                        Instantiate(Resources.Load("Frog"), new Vector3(Random.Range(150f, 400f), 3f, Random.Range(115f, 220f)), transform.localRotation, transform);
                    }
                }
                for (int i = 0; i <= w2box2num; i++)
                {
                    if (Random.value <= 0.3)
                    {
                        Instantiate(Resources.Load("Cactoro"), new Vector3(Random.Range(115f, 57f), 3f, Random.Range(231f, 430f)), transform.localRotation, transform);
                    }
                    else if (Random.value <= 0.6)
                    {
                        Instantiate(Resources.Load("Spider"), new Vector3(Random.Range(115f, 57f), 3f, Random.Range(231f, 430f)), transform.localRotation, transform);
                    }
                    else
                    {
                        Instantiate(Resources.Load("Frog"), new Vector3(Random.Range(115f, 57f), 3f, Random.Range(231f, 430f)), transform.localRotation, transform);
                    }
                }
                for (int i = 0; i <= w2box3num; i++)
                {
                    if (Random.value <= 0.3)
                    {
                        Instantiate(Resources.Load("Cactoro"), new Vector3(Random.Range(390f, 444f), 3f, Random.Range(268f, 446f)), transform.localRotation, transform);
                    }
                    else if (Random.value <= 0.6)
                    {
                        Instantiate(Resources.Load("Spider"), new Vector3(Random.Range(390f, 444f), 3f, Random.Range(268f, 446f)), transform.localRotation, transform);
                    }
                    else
                    {
                        Instantiate(Resources.Load("Frog"), new Vector3(Random.Range(390f, 444f), 3f, Random.Range(268f, 446f)), transform.localRotation, transform);
                    }
                }
                break;
        }
    }
}
