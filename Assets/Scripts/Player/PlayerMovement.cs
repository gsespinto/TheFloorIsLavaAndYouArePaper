using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [SerializeField] private float gravityForce = -25.0f;

    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float runSpeed = 17f;
    [SerializeField] private float climbSpeed = 5f;
    private float speed;

    [SerializeField] private Animator modelAnimator;
    [SerializeField] private GameObject model;
    private CharacterController charControl;
    private bool glide;

    [SerializeField] private Vector2 movementVec;
    [SerializeField] Vector2 wind;

    private PlayerScore scoreScript;

    [HideInInspector] public bool canClimb = false;
    private bool isClimbing = false;

    private bool isJumping;
    [SerializeField] private float jumpForce = 10f;
    private Coroutine jumpCoroutine = null;

    private GameManager gameManager;

    private KeyCode upCode = KeyCode.W;
    private KeyCode leftCode = KeyCode.A;
    private KeyCode downCode = KeyCode.S;
    private KeyCode rightCode = KeyCode.D;
    private KeyCode sprintCode = KeyCode.LeftShift;
    private KeyCode jumpCode = KeyCode.Space;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        charControl = this.GetComponent<CharacterController>();
        
        speed = walkSpeed;
        movementVec = Vector3.zero;

        scoreScript = this.GetComponent<PlayerScore>();
        gameManager = GameObject.FindObjectOfType<GameManager>();

        SetKeyCodes();
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreScript.isReady)
        {
            if (!scoreScript.isDead && !scoreScript.hasWon)
            {
                Sprint();
                MoveXZ();

                RotateToMovement();
            }

            if (!scoreScript.isDead && !scoreScript.hasWon)
            {
                Climb();
                Glide();
                Jump();
            }
            else
            {
                SetIdleMovement();
            }

            Gravity();

            charControl.Move(movementVec * Time.deltaTime);

            AnimationHandler();
            ResetMovVector();
        }
    }

    void SetKeyCodes()
    {
        if (gameManager)
        {
            upCode = gameManager.upCode;
            rightCode = gameManager.rightCode;
            downCode = gameManager.downCode;
            leftCode = gameManager.leftCode;
            sprintCode = gameManager.sprintCode;
            jumpCode = gameManager.jumpCode;
        }
    }

    void Climb()
    {
        if (canClimb)
        {
            if (Input.GetKey(upCode))
            {
                movementVec.y = climbSpeed;
                isClimbing = true;
                modelAnimator.speed = 1;
            }
            else if (Input.GetKey(downCode))
            {
                movementVec.y = -climbSpeed;
                isClimbing = true;
                modelAnimator.speed = 1;
            }
            else if (isClimbing)
            {
                movementVec.y = 0;
                modelAnimator.speed = 0;
            }
        }
        else
        {
            isClimbing = false;
            modelAnimator.speed = 1;
        }

        modelAnimator.SetBool("climbing", isClimbing);
    }

    void SetIdleMovement()
    {
        isClimbing = false;
        modelAnimator.speed = 1;
        glide = false;
    }

    void ResetMovVector ()
    {
        movementVec.x = 0;

        if (charControl.isGrounded)
            movementVec.y = 0;
    }

    void Jump()
    {
        if (charControl.isGrounded || isClimbing)
        {
            StopJump();
        }

        if (Input.GetKeyDown(jumpCode) && charControl.isGrounded)
        {
            if (jumpCoroutine == null)
            {
                jumpCoroutine = StartCoroutine(JumpRoutine(5f));
            }
        }
    }

    void StopJump()
    {
        isJumping = false;
        jumpCoroutine = null;
    }

    IEnumerator JumpRoutine(float increaseFactor)
    {
        isJumping = true;
        float _jumpForce = 0;
        modelAnimator.SetTrigger("jumping");

        while (isJumping)
        {
            if (_jumpForce < jumpForce)
            {
                movementVec.y += increaseFactor * Time.deltaTime;
                _jumpForce += increaseFactor * Time.deltaTime;
            }
            else
            {
                StopJump();
                yield return null;
            }
        }
        yield return null;
    }

    void Gravity()
    {
        if (!isClimbing && !isJumping)
        {
            if (!glide)
            {
                if (!charControl.isGrounded)
                {
                    movementVec += wind;
                    movementVec.y += gravityForce;
                    return;
                }

                // To ensure the player is grounded so he doesn't glide
                movementVec.y += gravityForce * 5;
                return;
            }
            else
            {
                movementVec.y = gravityForce * 4;
                movementVec += wind;
                return;
            }
        }
    }

    void MoveXZ ()
    {        
        if (Input.GetKey(rightCode))
            movementVec += Vector2.right * speed;
        if (Input.GetKey(leftCode))
            movementVec += Vector2.right * -speed;
    }
    void Sprint()
    {
        if (Input.GetKey(sprintCode) && charControl.isGrounded && !glide)
        {
            speed = runSpeed;
            modelAnimator.SetBool("run", true);
        }
        else
        {
            speed = walkSpeed;
            modelAnimator.SetBool("run", false);
        }
    }
    void RotateToMovement()
    {
        Vector3 direction = movementVec;
        direction.y = 0;
        direction += model.transform.position;

        direction = Vector2.Lerp(direction, model.transform.forward + model.transform.position, Time.deltaTime);

        model.transform.LookAt(direction);
    }

    void AnimationHandler()
    {
        modelAnimator.SetBool("dead", scoreScript.isDead);
        modelAnimator.SetBool("won", scoreScript.hasWon);

        if (!IsFalling())
        {
            // breakpoint
            if (movementVec.x != 0)
            {
                modelAnimator.SetBool("walk", true);
                modelAnimator.SetBool("idle", false);
                return;
            }
            // breakpoint
            else
            {
                modelAnimator.SetBool("walk", false);
                modelAnimator.SetBool("idle", true);
                return;
            }
        }
        // breakpoint
        else
        {
            modelAnimator.SetBool("walk", false);
            modelAnimator.SetBool("idle", false);
            modelAnimator.SetBool("fall", true);
            return;
        }
    }

    void Glide()
    {
        if (!charControl.isGrounded && Input.GetKeyDown(jumpCode))
        {
            glide = true;
            StopJump();
        }
        else if (Input.GetKeyUp(jumpCode) || charControl.isGrounded)
        {
            glide = false;
        }

        modelAnimator.SetBool("glide", glide);
    }

    bool IsFalling()
    {
        Debug.DrawRay(this.transform.position, Vector3.down * (charControl.height + 0.05f));
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out hit, charControl.height))
        {
            if (hit.transform.gameObject != this.gameObject)
                return false;
        }

        return true;
    }
}

public class QuickMaths
{
    public Vector3 LerpVector3 (Vector3 lerpingVector, Vector3 direction, float speed)
    {
        float xAux = ((direction.x - lerpingVector.x) / Mathf.Abs((direction.x - lerpingVector.x)));
        if (xAux > 0)
        {
            if (lerpingVector.x + xAux * speed < direction.x)
                lerpingVector.x += xAux * speed;
            else
                lerpingVector.x = direction.x;
        }
        else if (xAux < 0)
        {
            if (lerpingVector.x + xAux * speed > direction.x)
                lerpingVector.x += xAux * speed;
            else
                lerpingVector.x = direction.x;
        }

        float yAux = ((direction.y - lerpingVector.y) / Mathf.Abs((direction.y - lerpingVector.y)));
        if (yAux > 0)
        {
            if (lerpingVector.y + yAux * speed < direction.y)
                lerpingVector.y += yAux * speed;
            else
                lerpingVector.y = direction.y;
        }
        else if (yAux < 0)
        {
            if (lerpingVector.y + yAux * speed > direction.y)
                lerpingVector.y += yAux * speed;
            else
                lerpingVector.y = direction.y;
        }

        float zAux = ((direction.z - lerpingVector.z) / Mathf.Abs((direction.z - lerpingVector.z)));
        if (zAux > 0)
        {
            if (lerpingVector.z + zAux * speed < direction.z)
                lerpingVector.z += zAux * speed;
            else
                lerpingVector.z = direction.z;
        }
        else if (zAux < 0)
        {
            if (lerpingVector.z + zAux * speed > direction.z)
                lerpingVector.z += zAux * speed;
            else
                lerpingVector.z = direction.z;
        }

        return lerpingVector;
    }
}
