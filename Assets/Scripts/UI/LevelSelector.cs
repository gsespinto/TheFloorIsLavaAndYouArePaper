using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] LevelButton[] levelBTs;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        EnableLevelBTs();
    }

    void EnableLevelBTs()
    {
        if (!gameManager)
        {
            Debug.LogError(this.gameObject.name + " hasn't found GameManager.");
            return;
        }

        for (int i = 0; i < levelBTs.Length; i++)
        {
            if (i < gameManager.unlockedLvl)
            {
                levelBTs[i].button.interactable = true;
                levelBTs[i].label.text = levelBTs[i].lvlName;
            }
            else
            {
                levelBTs[i].button.interactable = false;
                levelBTs[i].label.text = "LOCKED";
            }
        }
    }
}
