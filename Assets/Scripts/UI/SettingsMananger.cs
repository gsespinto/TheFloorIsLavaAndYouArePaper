using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using TMPro;

public class SettingsMananger : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    //  [0] - Master, [1] - Music, [2] - SFX, [3] - UI
    [SerializeField] Slider[] audioSliders;
    [SerializeField] Image[] muteImages;

    [SerializeField] Sprite unmutedIcon;
    [SerializeField] Sprite mutedIcon;

    [SerializeField] KeyButton[] keyBTs;
    [SerializeField] Button[] controlBTs;
    private GameManager gameManager;
    private bool setKey = false;
    private ColorBlock normalColor;
    [SerializeField] Color highColor;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        AssignButtons();
        GetSavedVolumes();
    }

    public void MuteUnmuteAudio(int groupIndex)
    {
        float volume;
        audioMixer.GetFloat(GetGroupName(groupIndex), out volume);

        if (volume < audioSliders[groupIndex].minValue)
        {
            SetVolume(groupIndex);
        }
        else
        {
            MuteAudio(groupIndex);
        }
    }

    string GetGroupName(int groupIndex)
    {
        string group = "";
        switch (groupIndex)
        {
            case 0:
                group = "GlobalVolume";
                break;
            case 1:
                group = "MusicVolume";
                break;
            case 2:
                group = "SFXVolume";
                break;
            case 3:
                group = "UIVolume";
                break;
        }
        return group;
    }

    public void SetVolume(int groupIndex)
    {
        string group = GetGroupName(groupIndex);
        audioMixer.SetFloat(group, audioSliders[groupIndex].value);

        gameManager.volumes[groupIndex] = audioSliders[groupIndex].value;
        gameManager.UpdateSettingsInfo();

        if (audioSliders[groupIndex].value > audioSliders[groupIndex].minValue)
            muteImages[groupIndex].sprite = unmutedIcon;
        else
            MuteAudio(groupIndex);
    }

    void MuteAudio(int groupIndex)
    {
        string group = GetGroupName(groupIndex);
        audioMixer.SetFloat(group, -80);
        muteImages[groupIndex].sprite = mutedIcon;

        gameManager.volumes[groupIndex] = -80;
        gameManager.UpdateSettingsInfo();
    }

    void AssignButtons()
    {
        foreach (KeyButton bt in keyBTs)
        {
            switch (bt.button.name)
            {
                case ("up"):
                    bt.keyString.text = gameManager.upCode.ToString();
                    break;
                case ("left"):
                    bt.keyString.text = gameManager.leftCode.ToString();
                    break;
                case ("down"):
                    bt.keyString.text = gameManager.downCode.ToString();
                    break;
                case ("right"):
                    bt.keyString.text = gameManager.rightCode.ToString();
                    break;
                case ("sprint"):
                    bt.keyString.text = gameManager.sprintCode.ToString();
                    break;
                case ("jump"):
                    bt.keyString.text = gameManager.jumpCode.ToString();
                    break;
            }
        }
    }

    void GetSavedVolumes()
    {
        for (int i = 0; i < gameManager.volumes.Length; i++)
        {
            audioSliders[i].value = gameManager.volumes[i];
            SetVolume(i);
        }
    }

    /// <summary> Sets given keycode according to player's input </summary>
    public void SetKeyCode(string variableName)
    {
        setKey = true;
        StartCoroutine(KeyRoutine(variableName));
        foreach (KeyButton bt in keyBTs)
        {
            if (bt.button.name == variableName)
            {
                ColorBlock colorSpace = bt.button.colors;
                normalColor = colorSpace;
                colorSpace.disabledColor = highColor;
                bt.button.colors = colorSpace;
            }

            bt.button.interactable = false;
        }

        foreach (Button bt in controlBTs)
            bt.interactable = false;
    }

    /// <summary> Waits for player input to change given keycode </summary>
    IEnumerator KeyRoutine(string variableName)
    {
        while (setKey && gameManager != null)
        {
            if (Input.anyKeyDown)
            {
                KeyCode keyChange = KeyCode.None;
                foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(kcode))
                        keyChange = kcode;
                }
                switch (variableName)
                {
                    case ("up"):
                        gameManager.upCode = keyChange;
                        break;
                    case ("left"):
                        gameManager.leftCode = keyChange;
                        break;
                    case ("down"):
                        gameManager.downCode = keyChange;
                        break;
                    case ("right"):
                        gameManager.rightCode = keyChange;
                        break;
                    case ("sprint"):
                        gameManager.sprintCode = keyChange;
                        break;
                    case ("jump"):
                        gameManager.jumpCode = keyChange;
                        break;
                }
                foreach (KeyButton bt in keyBTs)
                {
                    bt.button.interactable = true;
                    bt.button.colors = normalColor;
                }
                foreach (Button bt in controlBTs)
                    bt.interactable = true;
                AssignButtons();
                gameManager.UpdateSettingsInfo();
                setKey = false;
            }
            yield return null;
        }
    }

    public void ResetSaveFiles()
    {
        gameManager.ResetGame();
    }
}

[Serializable]
public class KeyButton
{
    public Button button;
    public TextMeshProUGUI keyString;
}
