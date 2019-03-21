using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUI : MonoBehaviour
{
    public Toggle muteToggle;
    public GameObject settingPanel;

    void Start()
    {
        muteToggle.isOn = !AudioManager.Instance.IsMute;
    }

    public void SwitchMute(bool isOn)
    {
        AudioManager.Instance.SwitchMuteState(isOn);
    }

    public void OnBackButtonDown()
    {
        PlayerPrefs.SetInt("gold",GameController.Instance.gold);
        PlayerPrefs.SetInt("lv", GameController.Instance.lv);
        PlayerPrefs.SetFloat("scd", GameController.Instance.smallTimer);
        PlayerPrefs.SetFloat("bcd", GameController.Instance.bigTimer);
        PlayerPrefs.SetInt("exp", GameController.Instance.exp);
        PlayerPrefs.SetInt("mute", AudioManager.Instance.IsMute?1:0);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnSettingButtonDown()
    {
        settingPanel.SetActive(true);
    }

    public void OnCloseButtonDown()
    {
        settingPanel.SetActive(false);
    }
}
