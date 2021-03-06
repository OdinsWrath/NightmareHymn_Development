﻿using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    
    // Rotational Shooting implementation
    public GameObject baseBullet;   // Reference to base bullet prefab
    public Transform firingRadius;  // Reference to the sphere that defines the
                                    // locations a player can fire from
    public GameObject LavaCanon;    // Reference to secondary weapon
    public Transform firePoint;     // Location of to fire bullets from
    public float aimSpeed = 5f;     // Defines how fast to change the firing angle
    public Camera mainCamera;       // Refernce to the in game camera
                                        // Used to reference the mouse position in game position
    public ManaBar playerMana;      // Reference to the mana bar within the game's HUD
                                        // Used to set mana to 0 when player decides to use
                                        // secondary weapon

    void Update()
    {
        // Ensure that the firingRadius and firePoint are active
            // NOTE: will be destroyed when the player is removed from the scene
        if (firingRadius != null && firePoint != null)
        {
            // Constantly update the fire point to mouse location
            SetFirePoint();

            // Check for user fire input
            if (Input.GetButtonDown("Fire1"))
            {
                ShootBaseWeapon();
            }

            // Handle shooting secondary weapon
                // Only activate if the player has collected 2+ mana
            if (Input.GetButtonDown("Fire2") && PlayerController.mana >= 2)
            {
                Debug.Log("Shooting Secondary: current mana = " + PlayerController.mana);
                ShootSecondaryWeapon();
                PlayerController.mana = 0;
                playerMana.UpdateMana();
            }
        }
    }

    // Update the FirePoint based on the location of the mouse
    // NOTE: firePoint is a child of the firing radius so rotation to the 
    //       firingRadius sphere updates the firePoint location
    void SetFirePoint()
    {
        // Get the mouse position in releation to in game gamera
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // Get the vector that represents the desired aim
        Vector2 direction = new Vector3(mousePos.x - firingRadius.position.x, 
                                        mousePos.y - firingRadius.position.y);
        direction.Normalize();

        // Rotate the radius sphere to match the direction defined above
        firingRadius.right = direction;
    }

    // This function spawns a new base bullet to the game at the current
    // firePoint
    void ShootBaseWeapon()
    {
        Instantiate(baseBullet, firePoint.position, firePoint.rotation);
    }


    // This function
    void ShootSecondaryWeapon()
    {
        Instantiate(LavaCanon, transform.position, transform.rotation);
    }
}
