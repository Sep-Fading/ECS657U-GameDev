using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateSaver : MonoBehaviour
{
    public static GameStateSaver Instance { get; private set; }
    [SerializeField] private GameObject[] CarryOverObjects;

    private void Awake()
    {
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
                Vector3[] spawnPoints = { new Vector3(-1f, 10f, 10f), new Vector3(304.38f, 11.05f, 306f), new Vector3(372f, 1f, 361f), new Vector3(175f, 1f, 80f), new Vector3(46f, 1f, 3752f), new Vector3(-1f, 0.7f, -1.7f), new Vector3(50f, 1f, 14f) };
                obj.transform.position = new Vector3(0, 20, 0);
                if (scene.buildIndex < spawnPoints.Length) obj.transform.GetChild(0).gameObject.GetComponent<CharacterController>().Move(spawnPoints[scene.buildIndex] - obj.transform.GetChild(0).position);
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

    public static void ResetInstance()
    {
        foreach (GameObject obj in Instance.CarryOverObjects)
        {
            Destroy(obj);
        }
        Instance = null;
    }
}