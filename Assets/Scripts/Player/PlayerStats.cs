using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer playerMesh;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        UpdateToSelectedMaterials();
    }

    public void UpdateToSelectedMaterials()
    {
        Material[] mats = playerMesh.materials;

        mats[0] = gameManager.headMats[gameManager.selectedMatsIndex[0]].mat;
        mats[1] = gameManager.torsoMats[gameManager.selectedMatsIndex[1]].mat;
        mats[2] = gameManager.legsMats[gameManager.selectedMatsIndex[3]].mat;
        mats[3] = gameManager.armsMats[gameManager.selectedMatsIndex[2]].mat;

        playerMesh.materials = mats;
    }
}
