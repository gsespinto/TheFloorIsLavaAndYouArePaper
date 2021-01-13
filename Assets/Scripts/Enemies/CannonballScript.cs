using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonballScript : MonoBehaviour
{
    private float speed = 10f;
    private Vector3 direction = Vector3.zero;
    private GameObject parentCannon = null;

    [SerializeField]
    private string[] deathMessages;

    public void SetInitialValues(float _speed, Vector3 _direction, GameObject _parent)
    {
        speed = _speed;
        direction = _direction;
        parentCannon = _parent;
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == parentCannon)
        {
            return;
        }

        CheckAndKillPlayer(other.gameObject);
        Destroy(this.gameObject);
    }

    void Move()
    {
        this.transform.position += direction * speed * Time.deltaTime;
    }

    void CheckAndKillPlayer(GameObject other)
    {
        if (other.GetComponent<PlayerScore>())
        {
            other.GetComponent<PlayerScore>().Die(deathMessages[Random.Range(0, deathMessages.Length)]);
        }
        else if (other.GetComponentInParent<PlayerScore>())
        {
            other.GetComponentInParent<PlayerScore>().Die(deathMessages[Random.Range(0, deathMessages.Length)]);
        }
    }
}
