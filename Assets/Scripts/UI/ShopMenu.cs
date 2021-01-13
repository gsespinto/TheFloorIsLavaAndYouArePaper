using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] private int shownHeadIndex = 0;
    [SerializeField] private int shownTorsoIndex = 0;
    [SerializeField] private int shownArmsIndex = 0;
    [SerializeField] private int shownLegsIndex = 0;

    private GameManager gameManager;
    [SerializeField] private Button chooseBT;

    [SerializeField] private SkinnedMeshRenderer avatarMesh;

    [SerializeField] private BuyButton[] buyButtons;
    [SerializeField] private TextMeshProUGUI currentBoats;

    private void Awake()
    {
        GetGameManager();
        ResetShowMaterialIndexs();
        UpdateCurrentBoats();
    }

    private void Update()
    {
        CheckCanBuy();
        CheckCanChoose();
    }

    private void CheckCanChoose()
    {
        bool canChoose = gameManager.headMats[shownHeadIndex].isUnlocked && gameManager.torsoMats[shownTorsoIndex].isUnlocked 
            && gameManager.armsMats[shownArmsIndex].isUnlocked && gameManager.legsMats[shownLegsIndex].isUnlocked;

        chooseBT.interactable = canChoose;
    }

    public void ChooseMats()
    {
        gameManager.selectedMatsIndex[0] = shownHeadIndex;
        gameManager.selectedMatsIndex[1] = shownTorsoIndex;
        gameManager.selectedMatsIndex[2] = shownArmsIndex;
        gameManager.selectedMatsIndex[3] = shownLegsIndex;

        gameManager.UpdateGameInfo();
        UpdateToSelectedMaterials();
    }

    /// <summary> Shows next material of given type </summary>
    /// <param name="typeOfMat"> 0 = Head Material; 1 = Torso Material; 2 = Arms Material; 3 = Legs Material</param>
    public void NextMat(int typeOfMat)
    {
        switch (typeOfMat)
        {
            case 0:
                shownHeadIndex++;
                if (shownHeadIndex >= gameManager.headMats.Length)
                    shownHeadIndex = 0;
                break;            
            case 1:
                shownTorsoIndex++;
                if (shownTorsoIndex >= gameManager.torsoMats.Length)
                    shownTorsoIndex = 0;
                break;
            case 2:
                shownArmsIndex++;
                if (shownArmsIndex >= gameManager.armsMats.Length)
                    shownArmsIndex = 0;
                break;
            case 3:
                shownLegsIndex++;
                if (shownLegsIndex >= gameManager.legsMats.Length)
                    shownLegsIndex = 0;
                break;
        }
        UpdateShownMaterials();
    }    
    
    /// <summary> Shows previous material of given type </summary>
    /// <param name="typeOfMat"> 0 = Head Material; 1 = Torso Material; 2 = Arms Material; 3 = Legs Material</param>
    public void PrevMat(int typeOfMat)
    {
        switch (typeOfMat)
        {
            case 0:
                shownHeadIndex--;
                if (shownHeadIndex < 0)
                    shownHeadIndex = gameManager.headMats.Length - 1;
                break;            
            case 1:
                shownTorsoIndex--;
                if (shownTorsoIndex < 0)
                    shownTorsoIndex = gameManager.torsoMats.Length - 1;
                break;
            case 2:
                shownArmsIndex--;
                if (shownArmsIndex < 0)
                    shownArmsIndex = gameManager.armsMats.Length - 1;
                break;
            case 3:
                shownLegsIndex--;
                if (shownLegsIndex < 0)
                    shownLegsIndex = gameManager.legsMats.Length - 1;
                break;
        }
        UpdateShownMaterials();
    }

    /// <summary> Buys given type of shown material </summary>
    /// <param name="typeOfMat"> 0 = Head Material; 1 = Torso Material; 2 = Arms Material; 3 = Legs Material</param>
    public void BuyMat(int typeOfMat)
    {
        switch (typeOfMat)
        {
            case 0:
                gameManager.ChangeBoats(-gameManager.headMats[shownHeadIndex].price);
                gameManager.headMats[shownHeadIndex].isUnlocked = true;
                break;
            case 1:
                gameManager.ChangeBoats(-gameManager.torsoMats[shownTorsoIndex].price);
                gameManager.torsoMats[shownTorsoIndex].isUnlocked = true;
                break;
            case 2:
                gameManager.ChangeBoats(-gameManager.armsMats[shownArmsIndex].price);
                gameManager.armsMats[shownArmsIndex].isUnlocked = true;
                break;
            case 3:
                gameManager.ChangeBoats(-gameManager.legsMats[shownLegsIndex].price);
                gameManager.legsMats[shownLegsIndex].isUnlocked = true;
                break;
        }
        UpdateCurrentBoats();
    }

    /// <summary> Check if the player can buy shown materials </summary>
    /// <param name="typeOfMat"> 0 = Head Material; 1 = Torso Material; 2 = Arms Material; 3 = Legs Material</param>
    private void CheckCanBuy()
    {
        if (!gameManager.headMats[shownHeadIndex].isUnlocked)
        {
            buyButtons[0].priceTag.text = gameManager.headMats[shownHeadIndex].price.ToString();
            buyButtons[0].button.gameObject.SetActive(true);

            buyButtons[0].button.interactable = gameManager.boats >= gameManager.headMats[shownHeadIndex].price;
        }
        else
            buyButtons[0].button.gameObject.SetActive(false);

        if (!gameManager.torsoMats[shownTorsoIndex].isUnlocked)
        {
            buyButtons[1].priceTag.text = gameManager.torsoMats[shownTorsoIndex].price.ToString();
            buyButtons[1].button.gameObject.SetActive(true);

            buyButtons[1].button.interactable = gameManager.boats >= gameManager.torsoMats[shownTorsoIndex].price;
        }
        else
            buyButtons[1].button.gameObject.SetActive(false);

        if (!gameManager.armsMats[shownArmsIndex].isUnlocked)
        {
            buyButtons[2].priceTag.text = gameManager.armsMats[shownArmsIndex].price.ToString();
            buyButtons[2].button.gameObject.SetActive(true);

            buyButtons[2].button.interactable = gameManager.boats >= gameManager.armsMats[shownArmsIndex].price;
        }
        else
            buyButtons[2].button.gameObject.SetActive(false);

        if (!gameManager.legsMats[shownLegsIndex].isUnlocked)
        {
            buyButtons[3].priceTag.text = gameManager.legsMats[shownLegsIndex].price.ToString();
            buyButtons[3].button.gameObject.SetActive(true);

            buyButtons[3].button.interactable = gameManager.boats >= gameManager.legsMats[shownLegsIndex].price;
        }
        else
            buyButtons[3].button.gameObject.SetActive(false);
    }

    void UpdateCurrentBoats()
    {
        currentBoats.text = gameManager.boats.ToString();
    }

    public void UpdateShownMaterials()
    {
        Material[] mats = avatarMesh.materials;

        mats[0] = gameManager.headMats[shownHeadIndex].mat;
        mats[1] = gameManager.torsoMats[shownTorsoIndex].mat;
        mats[2] = gameManager.legsMats[shownLegsIndex].mat;
        mats[3] = gameManager.armsMats[shownArmsIndex].mat;

        avatarMesh.materials = mats;
    }

    public void UpdateToSelectedMaterials()
    {
        Material[] mats = avatarMesh.materials;

        mats[0] = gameManager.headMats[gameManager.selectedMatsIndex[0]].mat;
        mats[1] = gameManager.torsoMats[gameManager.selectedMatsIndex[1]].mat;
        mats[2] = gameManager.legsMats[gameManager.selectedMatsIndex[3]].mat;
        mats[3] = gameManager.armsMats[gameManager.selectedMatsIndex[2]].mat;

        avatarMesh.materials = mats;
    }

    public void GetGameManager()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void ResetShowMaterialIndexs()
    {
        shownHeadIndex = gameManager.selectedMatsIndex[0];
        shownTorsoIndex = gameManager.selectedMatsIndex[1];
        shownArmsIndex = gameManager.selectedMatsIndex[2];
        shownLegsIndex = gameManager.selectedMatsIndex[3];
    }
}

[Serializable]
public class BuyButton
{
    public Button button;
    public TextMeshProUGUI priceTag;
}
