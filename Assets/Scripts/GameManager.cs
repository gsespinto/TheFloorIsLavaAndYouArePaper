using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System;

public class GameManager : MonoBehaviour
{
    #region Variables
    public int boats = 0;
    public int unlockedLvl = 1;

    public PlayerMaterial[] headMats;
    public PlayerMaterial[] torsoMats;
    public PlayerMaterial[] armsMats;
    public PlayerMaterial[] legsMats;

    [HideInInspector] public int[] selectedMatsIndex = { 0, 0, 0, 0};

    private string gameSavefile;
    private string settingsSavefile;

    public KeyCode upCode = KeyCode.W;
    public KeyCode leftCode = KeyCode.A;
    public KeyCode downCode = KeyCode.S;
    public KeyCode rightCode = KeyCode.D;
    public KeyCode sprintCode = KeyCode.LeftShift;
    public KeyCode jumpCode = KeyCode.Space;

    public float[] volumes = { 0, 0, 0, 0 };
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (FindObjectsOfType(GetType()).Length > 1)
            Destroy(gameObject);

        gameSavefile = Application.persistentDataPath + "/game.txt";
        settingsSavefile = Application.persistentDataPath + "/settings.txt";
        Debug.Log(Application.persistentDataPath);

        GetGameInfo();
        GetSettingsInfo();
    }

    void GetGameInfo()
    {
        // if there's no save file creates one
        if (!File.Exists(gameSavefile))
        {
            SetGameInfo();
            return;
        }

        // gets save file info and assigns it to the correspondent variables
        string fileContent = File.ReadAllText(gameSavefile);
        fileContent = SimpleXOREncryption.EncryptorDecryptor.EncryptDecrypt(fileContent);

        Debug.Log(fileContent);

        unlockedLvl = int.Parse(RegexCaptureToString(fileContent, @"(?isx)<unlockedLvl>(?<capture>.*?)<unlockedLvl>"));
        boats = int.Parse(RegexCaptureToString(fileContent, @"(?isx)<boats>(?<capture>.*?)<boats>"));
        for (int i = 0; i < headMats.Length; i++)
        {
            Debug.Log(i);
            headMats[i].isUnlocked = bool.Parse(RegexCaptureToString(fileContent, @"(?isx)<headMatUnlocked" + i + ">(?<capture>.*?)<headMatUnlocked" + i + ">"));
            torsoMats[i].isUnlocked = bool.Parse(RegexCaptureToString(fileContent, @"(?isx)<torsoMatUnlocked" + i + ">(?<capture>.*?)<torsoMatUnlocked" + i + ">"));
            armsMats[i].isUnlocked = bool.Parse(RegexCaptureToString(fileContent, @"(?isx)<armsMatUnlocked" + i + ">(?<capture>.*?)<armsMatUnlocked" + i + ">"));
            legsMats[i].isUnlocked = bool.Parse(RegexCaptureToString(fileContent, @"(?isx)<legsMatUnlocked" + i + ">(?<capture>.*?)<legsMatUnlocked" + i + ">"));
        }

        for (int i = 0; i < selectedMatsIndex.Length; i++)
        {
            selectedMatsIndex[i] = int.Parse(RegexCaptureToString(fileContent, @"(?isx)<selectedMatsIndex" + i + ">(?<capture>.*?)<selectedMatsIndex" + i + ">"));
        }
    }

    ///<summary> Writes the save file with the initial values for the variables, then calls GetGameInfo() to assign the values to the variables </summary>
    void SetGameInfo()
    {
        string _unlockedLvl = "<unlockedLvl>" + 1 + "<unlockedLvl>";
        string _boats = "<boats>" + 0 + "<boats>";

        string _headMatsUnlocked = ""; 
        string _torsoMatsUnlocked = ""; 
        string _armsMatsUnlocked = ""; 
        string _legsMatsUnlocked = ""; 
        for (int i = 0; i < headMats.Length; i++)
        {
            // Change to false
            _headMatsUnlocked += "<headMatUnlocked" + i + ">" + headMats[i].isUnlocked + "<headMatUnlocked" + i + ">";
            _torsoMatsUnlocked += "<torsoMatUnlocked" + i + ">" + torsoMats[i].isUnlocked + "<torsoMatUnlocked" + i + ">";
            _armsMatsUnlocked += "<armsMatUnlocked" + i + ">" + armsMats[i].isUnlocked + "<armsMatUnlocked" + i + ">";
            _legsMatsUnlocked += "<legsMatUnlocked" + i + ">" + legsMats[i].isUnlocked + "<legsMatUnlocked" + i + ">";
        }

        string _selectedMaterialsIndex = "";
        for (int i = 0; i < selectedMatsIndex.Length; i++)
        {
            _selectedMaterialsIndex += "<selectedMatsIndex" + i + ">0<selectedMatsIndex" + i + ">";
        }

        string[] lines = { _unlockedLvl, _boats, _headMatsUnlocked, _torsoMatsUnlocked, _armsMatsUnlocked, _legsMatsUnlocked, _selectedMaterialsIndex };
        string fileContent = string.Join("", lines);
        fileContent = SimpleXOREncryption.EncryptorDecryptor.EncryptDecrypt(fileContent);
        System.IO.File.WriteAllText(gameSavefile, fileContent);

        GetGameInfo();
    }

    public void UpdateGameInfo()
    {
        string _unlockedLvl = "<unlockedLvl>" + unlockedLvl + "<unlockedLvl>";
        string _boats = "<boats>" + boats + "<boats>";

        string _headMatsUnlocked = "";
        string _torsoMatsUnlocked = "";
        string _armsMatsUnlocked = "";
        string _legsMatsUnlocked = "";
        for (int i = 0; i < headMats.Length; i++)
        {
            // Change to false
            _headMatsUnlocked += "<headMatUnlocked" + i + ">" + headMats[i].isUnlocked + "<headMatUnlocked" + i + ">";
            _torsoMatsUnlocked += "<torsoMatUnlocked" + i + ">" + torsoMats[i].isUnlocked + "<torsoMatUnlocked" + i + ">";
            _armsMatsUnlocked += "<armsMatUnlocked" + i + ">" + armsMats[i].isUnlocked + "<armsMatUnlocked" + i + ">";
            _legsMatsUnlocked += "<legsMatUnlocked" + i + ">" + legsMats[i].isUnlocked + "<legsMatUnlocked" + i + ">";
        }

        string _selectedMaterialsIndex = "";
        for (int i = 0; i < selectedMatsIndex.Length; i++)
        {
            _selectedMaterialsIndex += "<selectedMatsIndex"+ i +">" + selectedMatsIndex[i] + "<selectedMatsIndex" + i + ">";
        }

        string[] lines = { _unlockedLvl, _boats, _headMatsUnlocked, _torsoMatsUnlocked, _armsMatsUnlocked, _legsMatsUnlocked, _selectedMaterialsIndex };
        string fileContent = string.Join("", lines);
        fileContent = SimpleXOREncryption.EncryptorDecryptor.EncryptDecrypt(fileContent);
        System.IO.File.WriteAllText(gameSavefile, fileContent);
    }

    void GetSettingsInfo()
    {
        // if there's no save file creates one
        if (!File.Exists(settingsSavefile))
        {
            SetSettingsInfo();
            return;
        }

        // gets save file info and assigns it to the correspondent variables
        string fileContent = File.ReadAllText(settingsSavefile);
        fileContent = SimpleXOREncryption.EncryptorDecryptor.EncryptDecrypt(fileContent);

        upCode = (KeyCode)Enum.Parse(typeof(KeyCode), RegexCaptureToString(fileContent, @"(?isx)<upCode>(?<capture>.*?)<upCode>"));
        rightCode = (KeyCode)Enum.Parse(typeof(KeyCode), RegexCaptureToString(fileContent, @"(?isx)<rightCode>(?<capture>.*?)<rightCode>"));
        downCode = (KeyCode)Enum.Parse(typeof(KeyCode), RegexCaptureToString(fileContent, @"(?isx)<downCode>(?<capture>.*?)<downCode>"));
        leftCode = (KeyCode)Enum.Parse(typeof(KeyCode), RegexCaptureToString(fileContent, @"(?isx)<leftCode>(?<capture>.*?)<leftCode>"));
        sprintCode = (KeyCode)Enum.Parse(typeof(KeyCode), RegexCaptureToString(fileContent, @"(?isx)<sprintCode>(?<capture>.*?)<sprintCode>"));
        jumpCode = (KeyCode)Enum.Parse(typeof(KeyCode), RegexCaptureToString(fileContent, @"(?isx)<jumpCode>(?<capture>.*?)<jumpCode>"));

        for (int i = 0; i < volumes.Length; i++)
        {
            volumes[i] = float.Parse(RegexCaptureToString(fileContent, @"(?isx)<volume" + i +">(?<capture>.*?)<volume" + i + ">"));
        }
    }

    void SetSettingsInfo()
    {
        string _upCode = "<upCode>" + upCode.ToString() + "<upCode>";
        string _rightCode = "<rightCode>" + rightCode.ToString() + "<rightCode>";
        string _downCode = "<downCode>" + downCode.ToString() + "<downCode>";
        string _leftCode = "<leftCode>" + leftCode.ToString() + "<leftCode>";
        string _sprintCode = "<sprintCode>" + sprintCode.ToString() + "<sprintCode>";
        string _jumpCode = "<jumpCode>" + jumpCode.ToString() + "<jumpCode>";

        string _volumes = "";
        for (int i = 0; i < volumes.Length; i++)
        {
            _volumes += "<volume" + i + ">" + volumes[i] + "<volume" + i + ">"; 
        }

        string[] lines = { _upCode, _rightCode, _downCode, _leftCode, _sprintCode, _jumpCode, _volumes };
        string fileContent = string.Join("", lines);
        fileContent = SimpleXOREncryption.EncryptorDecryptor.EncryptDecrypt(fileContent);
        System.IO.File.WriteAllText(settingsSavefile, fileContent);

        GetSettingsInfo();
    }

    public void UpdateSettingsInfo()
    {
        string _upCode = "<upCode>" + upCode.ToString() + "<upCode>";
        string _rightCode = "<rightCode>" + rightCode.ToString() + "<rightCode>";
        string _downCode = "<downCode>" + downCode.ToString() + "<downCode>";
        string _leftCode = "<leftCode>" + leftCode.ToString() + "<leftCode>";
        string _sprintCode = "<sprintCode>" + sprintCode.ToString() + "<sprintCode>";
        string _jumpCode = "<jumpCode>" + jumpCode.ToString() + "<jumpCode>";

        string _volumes = "";
        for (int i = 0; i < volumes.Length; i++)
        {
            _volumes += "<volume" + i + ">" + volumes[i] + "<volume" + i + ">";
        }

        string[] lines = { _upCode, _rightCode, _downCode, _leftCode, _sprintCode, _jumpCode, _volumes };
        string fileContent = string.Join("", lines);
        fileContent = SimpleXOREncryption.EncryptorDecryptor.EncryptDecrypt(fileContent);
        System.IO.File.WriteAllText(settingsSavefile, fileContent);
    }

    /// <summary> Updates boats amount and calls UpdateGameInfo() </summary>
    public void ChangeBoats(int amount)
    {
        boats += amount;
        UpdateGameInfo();
    }

    /// <summary> Checks to increase unlocked level and calls UpdateGameInfo() </summary>
    public void CheckUnlockLevel(int currentLvl)
    {
        if (unlockedLvl > currentLvl)
            return;

        unlockedLvl++;
        UpdateGameInfo();
    }

    ///<summary> Splits string according to given patern </summary>
    private string RegexCaptureToString(string input, string pattern) //Property of Ivo yes
    {
        string capture = "";
        Regex rg = new Regex(pattern);
        Match m = rg.Match(input);
        capture = m.Groups["capture"].Value;
        return capture;
    }

    public void ResetGame()
    {
        File.Delete(gameSavefile);
        File.Delete(settingsSavefile);
        GetGameInfo();
        GetSettingsInfo();
        SceneManager.LoadScene("Main Menu");
    }
}

//Encryption and Decryption class
namespace SimpleXOREncryption
{
    public static class EncryptorDecryptor
    {
        public static int key = 129;

        public static string EncryptDecrypt(string textToEncrypt)
        {
            StringBuilder inSb = new StringBuilder(textToEncrypt);
            StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
            char c;
            for (int i = 0; i < textToEncrypt.Length; i++)
            {
                c = inSb[i];
                c = (char)(c ^ key);
                outSb.Append(c);
            }
            return outSb.ToString();
        }
    }
}

[Serializable]
public class PlayerMaterial
{
    public Material mat;
    public bool isUnlocked;
    public int price;
}

