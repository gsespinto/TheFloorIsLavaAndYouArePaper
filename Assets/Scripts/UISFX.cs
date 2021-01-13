using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISFX : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip clickClip;
    [SerializeField] AudioClip enterClip;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void PlayClickSFX()
    {
        audioSource.PlayOneShot(clickClip);
    }

    public void PlayerEnterSFX()
    {
        audioSource.PlayOneShot(enterClip);
    }
}
