using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class OptionsMenuManagerMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingMenu;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _player;
    [SerializeField] private Button _exitButton;
    private PlayerLook _playerLook;
    void Start()
    {
        _playerLook = _player.GetComponent<PlayerLook>();
        
        
        _slider.onValueChanged.AddListener((v) =>
        {
            _text.text = v.ToString("0.00");
            _playerLook.UpdateSensitivity(v);   
        });
        
    }
}
