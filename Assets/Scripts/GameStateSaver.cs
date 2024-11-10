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
                obj.transform.position = new Vector3(0, 20, 0);
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
}