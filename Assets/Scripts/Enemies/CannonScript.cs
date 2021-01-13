using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AnimationClip shootAnime;

    [SerializeField]
    private Transform emitterTrans;
    [SerializeField]
    private GameObject cannonballPrefab;

    [SerializeField]
    private float projectileSpeed = 10f;
    [SerializeField]
    private float shootRate = 2f;

    private PlayerScore playerScore;

    private AudioSource audioSource;
    [SerializeField]
    private AudioClip fireSFX;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        playerScore = FindObjectOfType<PlayerScore>();
        audioSource = this.GetComponent<AudioSource>();
        StartCoroutine(ShootRoutine());
    }

    IEnumerator ShootRoutine()
    {
        while(true)
        {
            if (!playerScore.isReady)
            {
                yield return null;
            }

            yield return new WaitForSeconds(shootRate);
            animator.SetTrigger("shoot");
            yield return new WaitForSeconds(shootAnime.length - 0.1f);
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject projectile = GameObject.Instantiate(cannonballPrefab, emitterTrans.position, cannonballPrefab.transform.rotation);
        CannonballScript cannonballScript = projectile.GetComponent<CannonballScript>();
        cannonballScript.SetInitialValues(projectileSpeed, emitterTrans.right, this.gameObject);
        audioSource.PlayOneShot(fireSFX);
    }
}
