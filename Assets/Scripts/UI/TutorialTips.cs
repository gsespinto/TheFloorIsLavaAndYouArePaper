using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class TutorialTips : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private TextMeshPro[] tips;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        AssignKeycodesToTips();
    }

    void AssignKeycodesToTips()
    {
        foreach (TextMeshPro tip in tips)
        {
            string tipAux = tip.text;
            tipAux = ReplaceWord(tipAux, @"upCode", gameManager.upCode.ToString());
            tipAux = ReplaceWord(tipAux, @"leftCode", gameManager.leftCode.ToString());
            tipAux = ReplaceWord(tipAux, @"downCode", gameManager.downCode.ToString());
            tipAux = ReplaceWord(tipAux, @"rightCode", gameManager.rightCode.ToString());
            tipAux = ReplaceWord(tipAux, @"sprintCode", gameManager.sprintCode.ToString());
            tipAux = ReplaceWord(tipAux, @"jumpCode", gameManager.jumpCode.ToString());

            tip.text = tipAux;
        }
    }

    string ReplaceWord(string original, string pattern, string replace)
    {
        if (Regex.IsMatch(original, pattern))
            return Regex.Replace(original, pattern, replace);
        else
            return original;
    }
}
