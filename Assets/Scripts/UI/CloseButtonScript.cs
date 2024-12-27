using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CloseButtonScript : MonoBehaviour
    {
        [SerializeField] private GameObject uiObject;
        private void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() =>
                UIManager.Instance.PopUIByGameObject(uiObject));
        }
    }
}