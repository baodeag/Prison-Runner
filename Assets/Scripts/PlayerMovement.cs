using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(10f,10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    bool isAlive = true; // Variable to track if the player is alive

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale; // Store the initial gravity scale
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return; // If the player is not alive, skip the update logic
        }
        Run();
        FlipSprite();
        ClimbingLadder();
        Die();
    }
    void OnFire(InputValue value)
    {
        if (!isAlive)
        {
            return; // If the player is not alive, skip the update logic
        }
        Instantiate(bullet, gun.position, transform.rotation); // Instantiate the bullet at the gun's position
    }

    void OnMove(InputValue value)
    {
        if (!isAlive)
        {
            return; 
        }
        moveInput = value.Get<Vector2>();
        Debug.Log(moveInput);
    }

    void OnJump(InputValue value)
    {
        if (!isAlive)
        {
            return; 
        }
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) // check if the player is touching the ground
        {
            return; // If not touching the ground, do not allow jumping
        }

        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed); // Adjust the jump force as needed
        }
    }



    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon; // Mathf.Epsilon is a very small value to avoid floating point errors
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed); // Set the running animation state
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void ClimbingLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = gravityScaleAtStart; // Restore the original gravity scale
            myAnimator.SetBool("isClimbing", false);
            return;
        }
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f; // Disable gravity while climbing


        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed); // Reset climbing animation state

    }

    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {

            isAlive = false; // Set the player to not alive
            myAnimator.SetTrigger("Dying"); // Trigger the dying animation
            myRigidbody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath(); // Notify the GameSession about player death

        }
    }

}
