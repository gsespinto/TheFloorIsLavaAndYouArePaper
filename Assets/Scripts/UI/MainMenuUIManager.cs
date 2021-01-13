using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] GameObject mainUI;
    [SerializeField] GameObject levelSelect;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] PlayerMenuAnimation playerAnimator;
    [SerializeField] GameObject shopMenu;

    void Start()
    {
        shopMenu.GetComponent<ShopMenu>().GetGameManager();
        shopMenu.GetComponent<ShopMenu>().UpdateToSelectedMaterials();
    }

    public void GoToMainUI()
    {
        playerAnimator.gameObject.SetActive(true);
        playerAnimator.GoToShop = false;
        playerAnimator.GoToLevels = false;

        mainUI.SetActive(true);
        levelSelect.SetActive(false);
        settingsMenu.SetActive(false);
        shopMenu.SetActive(false);
    }

    public void GoToLevelSelect()
    {
        playerAnimator.gameObject.SetActive(true);
        playerAnimator.GoToShop = false;
        playerAnimator.GoToLevels = true;

        mainUI.SetActive(false);
        levelSelect.SetActive(true);
        settingsMenu.SetActive(false);
        shopMenu.SetActive(false);
    }

    public void GoToSettingsMenu()
    {
        playerAnimator.gameObject.SetActive(false);
        playerAnimator.GoToShop = false;
        playerAnimator.GoToLevels = false;

        mainUI.SetActive(false);
        levelSelect.SetActive(false);
        settingsMenu.SetActive(true);
        shopMenu.SetActive(false);
    }

    public void GoToShopMenu()
    {
        playerAnimator.gameObject.SetActive(true);
        playerAnimator.GoToLevels = false;
        playerAnimator.GoToShop = true;

        mainUI.SetActive(false);
        levelSelect.SetActive(false);
        settingsMenu.SetActive(false);
        shopMenu.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
