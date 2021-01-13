using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] Image[] endBoats;
    [SerializeField] Color unlockColor = Color.white;
    [SerializeField] int levelToUnlock = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerScore scoreScript = other.gameObject.GetComponent<PlayerScore>();
            scoreScript.Win(levelToUnlock);
            int score = Mathf.Clamp(scoreScript.score, 0, 3);
            
            if (score > 0)
            {
                for (int i = 0; i < score; i++)
                {
                    endBoats[i].color = unlockColor;
                }
            }
        }
    }
}
