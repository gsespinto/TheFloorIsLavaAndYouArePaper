using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPoint : MonoBehaviour
{
    [SerializeField] private int pointsAmount = 1;
    [SerializeField] private AudioClip pickupSFX;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerScore>().ChangeScore(pointsAmount);
            other.gameObject.GetComponent<AudioSource>().PlayOneShot(pickupSFX);
            Destroy(this.gameObject);
        }
    }
}
