using System;
using GameplayMechanics.Character;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateSaver : MonoBehaviour
{
    public static GameStateSaver Instance { get; private set; }
    [SerializeField] private GameObject[] CarryOverObjects;
    [SerializeField] private GameObject goScreen;
    private GameOverScreen _gameOverScreen;

    private void Awake()
    {
        _gameOverScreen = goScreen.GetComponent<GameOverScreen>();
        if (Instance == null)
        {
            Instance = this;
            foreach (GameObject carryOverObject in CarryOverObjects)
            {
                DontDestroyOnLoad(carryOverObject);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (GameObject obj in CarryOverObjects)
        {
            if (obj.name == "-- Player")
            {
                Vector3[] spawnPoints = { new Vector3(210f, 11f, 140f), new Vector3(50f, 5f, 150f), new Vector3(240f, 30f, 60f), new Vector3(278f, 2f, 80f), new Vector3(46f, 2f, 3745f) };
                obj.transform.position = new Vector3(0, 20, 0);
                if (scene.buildIndex < spawnPoints.Length)
                {
                    //obj.transform.GetChild(0).gameObject.GetComponent<CharacterController>().Move(spawnPoints[scene.buildIndex] - obj.transform.GetChild(0).position); 
                    obj.transform.GetChild(0).GetComponent<CharacterController>().enabled = false;
                    obj.transform.GetChild(0).transform.position = spawnPoints[scene.buildIndex];
                    obj.transform.GetChild(0).GetComponent<CharacterController>().enabled = true;
                }
                Debug.Log("World " + scene.buildIndex + " " + (scene.buildIndex < spawnPoints.Length));
                //if (scene.buildIndex == 4) obj.transform.GetChild(0).gameObject.GetComponent<CharacterController>().Move(new Vector3(0f, 1f, 0f) - obj.transform.GetChild(0).position);
            }
        }
    }

    public GameObject GetSharedObjectByName(String name)
    {
        foreach (GameObject obj in CarryOverObjects)
        {
            // Top Level (Parent Object)
            if (obj.name == name)
            {
                return obj;
            }

            // Search in children
            GameObject foundChild = FindInChildren(obj.transform, name);
            if (foundChild != null)
            {
                return foundChild;
            }
        }

        return null;
    }

    // Recursively Search through children of a given parent transform.
    private GameObject FindInChildren(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }

            GameObject found = FindInChildren(child, name);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }

    public static void ResetInstance(bool isDead = false)
    {
        if (isDead)
        {
            Instance._gameOverScreen.ShowGameOverScreen();
        }
        PlayerStatManager.Instance.Life.SetCurrent(
            PlayerStatManager.Instance.Life.GetAppliedTotal());
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        /*
        foreach (GameObject obj in Instance.CarryOverObjects)
        {
            Destroy(obj);
        }
        Instance = null;
        */
    }
}