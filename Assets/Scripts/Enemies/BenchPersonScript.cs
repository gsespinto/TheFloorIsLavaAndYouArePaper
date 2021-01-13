using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchPersonScript : MonoBehaviour
{
    [SerializeField] Arm[] arms;    
    [SerializeField] Transform[] positions;
    [SerializeField] float speed = 1;
    [SerializeField] float moveTheshold = 0.05f;
    private PlayerScore playerScore;
    private bool hasStarted = false;

    [SerializeField] AudioClip hitClip;
    // Start is called before the first frame update
    void Start()
    {
        playerScore = GameObject.FindObjectOfType<PlayerScore>();
    }

    private void Update()
    {
        if (playerScore.isReady)
        {
            Attack();
        }
    }

    void Attack()
    {
        foreach (Arm arm in arms)
        {
            Vector3 pos = positions[arm.posIndex].position;
            pos.x = arm.obj.transform.position.x;
            pos.z = arm.obj.transform.position.z;

            float distance = Vector3.Distance(arm.obj.transform.position, pos);

            if (distance > moveTheshold)
            {
                Vector3 direction = (pos - arm.obj.transform.position).normalized;
                if (distance > speed)
                {
                    arm.obj.transform.position = arm.obj.transform.position + direction * speed;
                }
                else
                {
                    arm.obj.transform.position = arm.obj.transform.position + direction * distance;
                }
            }
            else
            {
                arm.posIndex++;

                if (arm.posIndex >= positions.Length)
                {
                    arm.audioSource.PlayOneShot(hitClip);
                    arm.posIndex = 0;
                }
            }
        }
    }
}

[Serializable]
public class Arm
{
    public GameObject obj;
    public int posIndex = 0;
    public AudioSource audioSource;
}
