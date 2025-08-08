using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int pointsForCoinPickup = 100;

    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !wasCollected)
        {
            wasCollected = true; // Prevent multiple pickups
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup); // Add points to the player's score
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position); // Play the coin pickup sound effect
            Destroy(gameObject); // Destroy the coin object
        }
    }


}

