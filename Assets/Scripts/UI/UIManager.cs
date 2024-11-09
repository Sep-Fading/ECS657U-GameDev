using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIManager
    {
        public static UIManager Instance;
        private List<GameObject> _uiStack = new List<GameObject>();

        public static void Initialize()
        {
            if (Instance == null)
            {
                Instance = new UIManager();
            }
        }
    
        public void PushUI(GameObject ui)
        {
            _uiStack.Add(ui);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void PopUI()
        {
            _uiStack.RemoveAt(_uiStack.Count - 1);
            CheckEmpty();
        }

        public void PopUIByGameObject(GameObject ui)
        {
            for (int i = 0; i < _uiStack.Count; i++)
            {
                if (_uiStack[i] == ui)
                {
                    _uiStack.RemoveAt(i);
                }
            }
            CheckEmpty();
        }

        private void CheckEmpty()
        {
            if (_uiStack.Count == 0)
            {
                Cursor.visible = false; 
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    
    }
}
