using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement playerMovement;
    [SerializeField] private FixedJoystick fixedJoystick;
    [SerializeField] private Button jumpBT;
    [SerializeField] private Button sprintBT;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = this.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        JoystickInput();
    }

    void JoystickInput()
    {
        Vector2 input = new Vector2(fixedJoystick.Horizontal, fixedJoystick.Vertical);

        if (input.x < 0)
        {
            playerMovement.GoLeft();
        }
        else if (input.x > 0)
        {
            playerMovement.GoRight();
        }

        if (input.y >= 0.5)
        {
            playerMovement.Climb(1);
        }
        else if (input.y <= -0.5)
        {
            playerMovement.Climb(-1);
        }
        else
        {
            playerMovement.Climb(0);
        }
    }
}
