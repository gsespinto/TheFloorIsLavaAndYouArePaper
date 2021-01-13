using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCanClimb : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Climbable")
            playerMovement.canClimb = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Climbable")
            playerMovement.canClimb = false;
    }
}
