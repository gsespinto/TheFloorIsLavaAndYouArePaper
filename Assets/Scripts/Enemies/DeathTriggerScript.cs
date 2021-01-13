using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTriggerScript : MonoBehaviour
{
    [SerializeField]
    private string[] deathMessages;

    private void OnTriggerEnter(Collider other)
    {
        CheckAndKillPlayer(other.gameObject);
    }

    void CheckAndKillPlayer(GameObject other)
    {
        if (other.GetComponent<PlayerScore>())
        {
            other.GetComponent<PlayerScore>().Die(deathMessages[Random.Range(0, deathMessages.Length)]);
        }
    }
}
