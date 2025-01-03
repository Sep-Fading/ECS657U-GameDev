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
        if (SceneManager.GetActiveScene().buildIndex == 3) Spawn(70);
        if (SceneManager.GetActiveScene().buildIndex == 4) Spawn(112);
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
                    if (Random.value <= 0.5)
                    {
                        Instantiate(Resources.Load("Cactoro"), new Vector3(Random.Range(150f, 400f), 3f, Random.Range(115f, 220f)), transform.localRotation, transform);
                    }
                    else 
                    {
                        Instantiate(Resources.Load("Spider"), new Vector3(Random.Range(150f, 400f), 3f, Random.Range(115f, 220f)), transform.localRotation, transform);
                    }
                }
                for (int i = 0; i <= w2box2num; i++)
                {
                    if (Random.value <= 0.5)
                    {
                        Instantiate(Resources.Load("Cactoro"), new Vector3(Random.Range(115f, 57f), 3f, Random.Range(231f, 430f)), transform.localRotation, transform);
                    }
                    else 
                    {
                        Instantiate(Resources.Load("Spider"), new Vector3(Random.Range(115f, 57f), 3f, Random.Range(231f, 430f)), transform.localRotation, transform);
                    }
                }
                for (int i = 0; i <= w2box3num; i++)
                {
                    if (Random.value <= 0.5)
                    {
                        Instantiate(Resources.Load("Cactoro"), new Vector3(Random.Range(390f, 444f), 3f, Random.Range(268f, 446f)), transform.localRotation, transform);
                    }
                    else
                    {
                        Instantiate(Resources.Load("Spider"), new Vector3(Random.Range(390f, 444f), 3f, Random.Range(268f, 446f)), transform.localRotation, transform);
                    }
                }
                break;
            case 3:
                int w3boxnum = num / 7;
                int boxes = 7;
                for (int i = 0; i <= boxes; i++)
                {
                    for (int j = 0; j <= w3boxnum; j++)
                    {
                        switch (i)
                        {
                            case 0:
                                Instantiate(Resources.Load("Knight"), new Vector3(Random.Range(271f, 284f), 2f, Random.Range(80f, 305f)), transform.localRotation, transform);
                                break;
                            case 1:
                                Instantiate(Resources.Load("Knight"), new Vector3(Random.Range(200f, 100f), 2f, Random.Range(250f, 287f)), transform.localRotation, transform);
                                break;
                            case 2:
                                Instantiate(Resources.Load("Knight"), new Vector3(Random.Range(200f, 240f), 2f, Random.Range(270f, 305f)), transform.localRotation, transform);
                                break;
                            case 3:
                                Instantiate(Resources.Load("Knight"), new Vector3(Random.Range(210f, 240f), 2f, Random.Range(250f, 220f)), transform.localRotation, transform);
                                break;
                            case 4:
                                Instantiate(Resources.Load("Knight"), new Vector3(Random.Range(210f, 240f), 2f, Random.Range(170f, 200f)), transform.localRotation, transform);
                                break;
                            case 5:
                                Instantiate(Resources.Load("Knight"), new Vector3(Random.Range(210f, 240f), 2f, Random.Range(130f, 150f)), transform.localRotation, transform);
                                break;
                            case 6:
                                Instantiate(Resources.Load("Knight"), new Vector3(Random.Range(200f, 170f), 2f, Random.Range(90f, 130f)), transform.localRotation, transform);
                                break;
                        }
                    }
                }
                break;
            case 4:
                int w4boxnum = num / 8;
                boxes = 8;
                for (int i = 0; i <= boxes; i++)
                {
                    for (int j = 0; j <= w4boxnum; j++)
                    {
                        switch (i)
                        {
                            case 0:
                                if (Random.value <= 0.3)
                                    Instantiate(Resources.Load("SkeleMage"), new Vector3(Random.Range(435f, 456f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                else if (Random.value <= 0.6)
                                    Instantiate(Resources.Load("SkeleRanged"), new Vector3(Random.Range(435f, 456f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                else
                                    Instantiate(Resources.Load("SkeleMelee"), new Vector3(Random.Range(435f, 456f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                break;
                            case 1:
                                if (Random.value <= 0.3)
                                    Instantiate(Resources.Load("SkeleMage"), new Vector3(Random.Range(386f, 413f), 1f, Random.Range(3426f, 3755f)), transform.localRotation, transform);
                                else if (Random.value <= 0.6)
                                    Instantiate(Resources.Load("SkeleRanged"), new Vector3(Random.Range(386f, 413f), 1f, Random.Range(3426f, 3755f)), transform.localRotation, transform);
                                else
                                    Instantiate(Resources.Load("SkeleMelee"), new Vector3(Random.Range(386f, 413f), 1f, Random.Range(3426f, 3755f)), transform.localRotation, transform);
                                break;
                            case 2:
                                if (Random.value <= 0.3)
                                    Instantiate(Resources.Load("SkeleMage"), new Vector3(Random.Range(335f, 365f), 1f, Random.Range(3426f, 3755f)), transform.localRotation, transform);
                                else if (Random.value <= 0.6)
                                    Instantiate(Resources.Load("SkeleRanged"), new Vector3(Random.Range(335f, 365f), 1f, Random.Range(3426f, 3755f)), transform.localRotation, transform);
                                else
                                    Instantiate(Resources.Load("SkeleMelee"), new Vector3(Random.Range(335f, 365f), 1f, Random.Range(3426f, 3755f)), transform.localRotation, transform);
                                break;
                            case 3:
                                if (Random.value <= 0.3)
                                    Instantiate(Resources.Load("SkeleMage"), new Vector3(Random.Range(280f, 314f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                else if (Random.value <= 0.6)
                                    Instantiate(Resources.Load("SkeleRanged"), new Vector3(Random.Range(280f, 314f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                else
                                    Instantiate(Resources.Load("SkeleMelee"), new Vector3(Random.Range(280f, 314f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                break;
                            case 4:
                                if (Random.value <= 0.3)
                                    Instantiate(Resources.Load("SkeleMage"), new Vector3(Random.Range(230f, 260f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                else if (Random.value <= 0.6)
                                    Instantiate(Resources.Load("SkeleRanged"), new Vector3(Random.Range(230f, 260f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                else
                                    Instantiate(Resources.Load("SkeleMelee"), new Vector3(Random.Range(230f, 260f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                break;
                            case 5:
                                if (Random.value <= 0.3)
                                    Instantiate(Resources.Load("SkeleMage"), new Vector3(Random.Range(183f, 210f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                else if (Random.value <= 0.6)
                                    Instantiate(Resources.Load("SkeleRanged"), new Vector3(Random.Range(183f, 210f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                else
                                    Instantiate(Resources.Load("SkeleMelee"), new Vector3(Random.Range(183f, 210f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                break;
                            case 6:
                                if (Random.value <= 0.3)
                                    Instantiate(Resources.Load("SkeleMage"), new Vector3(Random.Range(133f, 163f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                else if (Random.value <= 0.6)
                                    Instantiate(Resources.Load("SkeleRanged"), new Vector3(Random.Range(133f, 163f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                else
                                    Instantiate(Resources.Load("SkeleMelee"), new Vector3(Random.Range(133f, 163f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                break;
                            case 7:
                                if (Random.value <= 0.3)
                                    Instantiate(Resources.Load("SkeleMage"), new Vector3(Random.Range(80f, 144f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                else if (Random.value <= 0.6)
                                    Instantiate(Resources.Load("SkeleRanged"), new Vector3(Random.Range(80f, 144f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                else
                                    Instantiate(Resources.Load("SkeleMelee"), new Vector3(Random.Range(80f, 144f), 1f, Random.Range(3274f, 3755f)), transform.localRotation, transform);
                                break;
                            case 8:
                                if (Random.value <= 0.3)
                                    Instantiate(Resources.Load("SkeleMage"), new Vector3(Random.Range(32f, 60f), 1f, Random.Range(3274f, 3670f)), transform.localRotation, transform);
                                else if (Random.value <= 0.6)
                                    Instantiate(Resources.Load("SkeleRanged"), new Vector3(Random.Range(32f, 60f), 1f, Random.Range(3274f, 3670f)), transform.localRotation, transform);
                                else
                                    Instantiate(Resources.Load("SkeleMelee"), new Vector3(Random.Range(32f, 60f), 1f, Random.Range(3274f, 3670f)), transform.localRotation, transform);
                                break;
                        }
                    }
                }
                break;
        }
    }
}
