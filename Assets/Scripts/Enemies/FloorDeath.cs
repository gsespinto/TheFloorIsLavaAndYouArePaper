using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDeath : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject flameDeathVFXPrefab;
    [SerializeField] private string[] deathFireMessages;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            virtualCamera.Follow = null;
            other.gameObject.GetComponent<PlayerScore>().Die(deathFireMessages[Random.Range(0, deathFireMessages.Length)]);
            GameObject.Instantiate(flameDeathVFXPrefab, other.transform.position, this.transform.rotation);
        }
    }
}
