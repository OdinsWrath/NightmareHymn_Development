﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public static int numPlayers = 0;   // Holds the number of player objects that are currently in 
                                        // memory. 
                                            // NOTE: Used to enforce singleton design pattern

    public float moveSpeed;             // Movement speed of the player
    public int health;                  // Maintains player health
    public static int mana;             // Holds player mana count (used to fire secondary weapon)
    public float jumpForce;             // Force of player jump
    int jumpCount;                      // Number of jumps performed in one airborne state
    public int maxJumps;                // Max allowed jumps in each airborne state
    public Rigidbody theRB;             // Reference to player RigidBody component
    public float airMovement;           // How much control player has in the air
    float startSpd;                     // Initial speed of the player
    bool inAir;                         // True if the player is not touching a floor
    public float gravity;               // Custom gravity scale for player
    bool running;                       // True if the player is running
    private Vector2 moveDirection;      // Vector representign movement direction of player
    public float horizAxis;             // Holds a value representing the left/right controller input
    public float fallingGravScale;      // Scalar of how much increased gravity is applied when player is falling
    public bool isJumping;              // True when the player has pressed the jump button
    public HealthBar playerHealth;      // Reference to the health bar within the game's HUD
    public ManaBar playerMana;          // Reference to the mana bar within the game's HUD
    public GameObject PlayerDeath_part; // Particle animation to be played when the player dies



    // Start is called before the first frame update
    void Start()
    {
        //numPlayers++;
        theRB = GetComponent<Rigidbody>();
        
        // Initialize player variable components
        maxJumps = 2;
        startSpd = moveSpeed;
        running = false;
        health = 10;
        mana = 0;

        // Initialize full healthbar for newly spawned player
        playerHealth.SetFullHealth(health);
    }

    // Get player input from keyboard/controller
    void Update()
    {
        // Handle player death
        if (isDead() == true && transform != null)
        {
            //numPlayers--;
            Debug.Log("In isDead area of playerController");
            // Spawn death particles and destroy the player object
            Instantiate(PlayerDeath_part, transform.position, Quaternion.Euler(new Vector3(-100, 0, 0)));
            FindObjectOfType<GameManager>().EndGame();
            Destroy(gameObject);
        }
        else
        {
            // Get the horizontal direction
            horizAxis = Input.GetAxisRaw("Horizontal");

            // Check if player has hit jump button, and is withing jump count range
            if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
            {
                isJumping = true;
            }
        }
    }


    // Handle all physics interaction on the player
    void FixedUpdate()
    {
        // Move the player according to the horizontal controller input
        theRB.velocity = new Vector2(horizAxis * moveSpeed, theRB.velocity.y);

        // Handle player jumping
        if (isJumping == true)
        {
            theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            jumpCount += 1;
            isJumping = false;
        }

        // Handle airborne movement and increased fall gravity
        if (inAir == true)
        {
            // Reduce allowed movement speed while player is in the air
            moveSpeed = airMovement * startSpd;

            // Increase the gravity if the player is falling
            if (theRB.velocity.y < 0)
            {
                theRB.velocity -= Vector3.up * gravity * (fallingGravScale - 1) * Time.deltaTime;
            }
        }
        else
        {
            moveSpeed = startSpd;
        }

        // Apply custom gravity scale to player
        theRB.AddForce(Vector2.down * gravity * theRB.mass * Time.deltaTime);
    }


    // Handle player collision
    void OnCollisionEnter(Collision other)
    {
        //Debug.Log("Player hit: " + other.gameObject.tag);

        // Handle player hitting instant death trap
        if (other.gameObject.CompareTag("DeathTrap"))
        {
            // Fully deplete player health
            health = 0;

            // Update the healthbar
            playerHealth.UpdateHealth(health);
            //FindObjectOfType<GameManager>().EndGame();
        }
        //handle win area(need to go to win screen/nxt level but for now acts like death trap)
        if (other.gameObject.CompareTag("Finish"))
        {
            FindObjectOfType<GameManager>().EndGame();
        }

        // Handle player landing from airborne state
        if (other.gameObject.tag == "Floor")
        {
            jumpCount = 0;
            inAir = false;
        }

        if(other.gameObject.tag == "Enemy")
        {
            // Decrment health
            health--;

            // Update health bar
            playerHealth.UpdateHealth(health);
        }
    }

    void OnCollisionExit(Collision other)
    {
        // Handle leaving the ground
        if (other.gameObject.CompareTag("Floor"))
        {
            inAir = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Take damage if hit by enemy projectile
        if (other.gameObject.CompareTag("Enemybullet"))
        {
            health--;

            // Update health bar
            playerHealth.UpdateHealth(health);
        }

        // Update player health when they collect red tokens from killing enemies
        if (other.gameObject.CompareTag("Health") && health + 1 <= 10)
        {
            Destroy(other.gameObject);
            health++;
            playerHealth.UpdateHealth(health);
        }

        // Update player mana when they collect blue tokens from killing enemies
        else if (other.gameObject.CompareTag("Mana"))
        {
            Destroy(other.gameObject);
            mana++;
            playerMana.UpdateMana();
        }
    }


    // Signal if player has run out of health
    bool isDead()
    {
        if(health <= 0)
        {
            return(true);
        }
        else
        {
            return (false);
        }
    }

}
