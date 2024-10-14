using System.Collections;
using System.Collections.Generic;
using GameplayMechanics.Character;
using Player;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI promptText;
    [SerializeField] private PlayerSkillTreeManager _playerSkillTreeManager;

    // Start is called before the first frame update
    void Start()
    {
        _playerSkillTreeManager = GetComponent<PlayerSkillTreeManager>();
    }

    // Update is called once per frame
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    public void SkillTreeToggle(InputAction.CallbackContext context)
    {
        _playerSkillTreeManager._skillTreeUI.SetActive(
            !_playerSkillTreeManager._skillTreeUI.activeSelf);

        if (_playerSkillTreeManager._skillTreeUI.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
