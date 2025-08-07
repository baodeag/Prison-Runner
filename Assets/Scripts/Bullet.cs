using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    PlayerMovement playerMovement;
    float xSpeed; // Speed of the bullet in the x direction

    [SerializeField] float bulletSpeed = 10f; // Speed of the bullet

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        playerMovement = FindObjectOfType<PlayerMovement>(); // Find the PlayerMovement script in the scene
        xSpeed = playerMovement.transform.localScale.x * bulletSpeed; // Set the bullet's speed based on the player's facing direction
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed, 0f); // Set the bullet's velocity to move right at a speed of 1 units per second
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Destroy(collision.gameObject); // Destroy the enemy if the bullet collides with it
        }
        Destroy(gameObject); // Destroy the bullet after hitting the enemy

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Destroy(gameObject);
    }
}
