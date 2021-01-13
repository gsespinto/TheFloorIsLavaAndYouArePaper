using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelEnemy : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Transform[] patrolPoints;
    private int patrolIndex = 0;
    [SerializeField]
    private float speed = 10f;

    private PlayerScore playerScore;
    #endregion

    private void Start()
    {
        playerScore = FindObjectOfType<PlayerScore>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerScore.isReady)
        {
            return;
        }

        FollowPatrolPath();
    }

    void FollowPatrolPath()
    {
        if (Vector3.Distance(this.transform.position, patrolPoints[patrolIndex].position) < speed)
        {
            this.transform.position = patrolPoints[patrolIndex].transform.position;
            patrolIndex++;
            if (patrolIndex >= patrolPoints.Length)
            {
                patrolIndex = 0;
            }
        }
        else
        {
            Vector3 direction = (patrolPoints[patrolIndex].position - this.transform.position).normalized;
            this.transform.position += direction * speed;
        }
    }
}
