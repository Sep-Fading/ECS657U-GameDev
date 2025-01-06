using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Manages the functionality of the title screen
    /// </summary>
    public class TitleScreenManager : MonoBehaviour
    {
        [SerializeField] private Button _playbutton;

        void Start()
        {
            _playbutton.onClick.AddListener(() => ToggleCutscene());
        }

        public void ToggleCutscene()
        {
            Debug.Log("ToggleCutscene");
            SceneManager.LoadScene("CUTSCENE");
        }
    }
}