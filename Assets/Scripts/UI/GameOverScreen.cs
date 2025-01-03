using System;
using GameplayMechanics.Character;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private Button RestartButton;
        
        void Awake()
        {
            RestartButton.onClick.AddListener(() => PlayAgain());
            gameObject.SetActive(false);
        }
        
        public void ShowGameOverScreen()
        {
            gameObject.SetActive(true);
            UIManager.Instance.PushUI(gameObject);
        }

        public void PlayAgain()
        {
            Debug.Log(PlayerStatManager.Instance.Life.GetCurrent());
            UIManager.Instance.PopUIByGameObject(gameObject);
            GameStateSaver.ResetInstance();
        }

    }
}