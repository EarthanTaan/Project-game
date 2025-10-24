using System.Collections.Generic;
using UnityEngine;

namespace Mechanics
{
    public class JumpMechanic : MonoBehaviour
    {
        public float jumpForce = 7f;
        Rigidbody2D rb;
        int jumpCounter;            // Counts how many times the player has jumped.
        public int maxJumps = 2;           // The number of times the player can jump before touching the ground again.
        Vector2 position;           // To store the position of the player.
        Vector2 direction = Vector2.down; // Part of the ground-check raycast maneuver below.
        public float distance;  // This is adjustable to accomodate future changes to the player sprite.
        private LayerMask groundLayer;
        private RaycastHit2D groundSensor; // A ray fired from 'position' straight down at a 'distance' of just below the player's feet.
        private Vector2 spriteSize;  // The size of this GameObject's sprite.
        public float boxHt;  // The height of the BoxCast for ground detection.
        private float hfBoxHt;  // Half that height, for use in the Debug.DrawLine visualization.
        private bool jumpRequested = false;
        
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();  // This object's RigidBody2D component, for short.
            groundLayer = LayerMask.GetMask("Ground");
            spriteSize = GetComponent<SpriteRenderer>().bounds.size;
            
        }

        void Update()
        {
            position = transform.position;      // Keep 'position' updated with the player's present position.
            
            // Listen for jump input in Update(), but execute jump action in FixedUpdate()
            if (Input.GetKeyDown(KeyCode.Space) && jumpCounter < maxJumps)  // If the player presses 'Space' AND the current jump counter does not exceed the maximum...
            {
                jumpRequested = true;
            }
            
            
            
            // Debug visualizer for BoxCast
            hfBoxHt = boxHt * 0.5f;
            Debug.DrawLine(new Vector2(position.x - (spriteSize.x * 0.25f), position.y - distance - hfBoxHt),
                new Vector2(position.x + (spriteSize.x * 0.25f), position.y - distance - hfBoxHt), Color.blue); // This line should illustrate the BoxCast's width.
            Debug.DrawLine(new Vector2(position.x, position.y - distance + hfBoxHt),
                new Vector2(position.x, position.y - distance - hfBoxHt), Color.blue);  // This line should illustrate the BoxCast's depth.
            
            
        }

        // Physics should generally go in FixedUpdate(), or so I've read.
        void FixedUpdate()
        {
            // / Ground Check Logic:
            // - The RaycastHit2D variable named 'groundSensor' will now be supplied the hit data from a 2D box.
            //      - groundSensor was declared at the top with the other variables, but not defined. It will now be defined, below:
            // - The box will originate at the player's 'position'. (transform.position)
            // - Its size will be the width of the sprite, but very shallow (spriteSize.x, 0.01
            // - The 'direction' will be straight down. (Vector2.down)
            // - The 'distance' the ray will travel will terminate it at a point just below the player's feet. (adjustible in the Unity Editor)
            // - The ray will exist only on the "Ground" layer. Thereby, it can only "hit" objects that are also on that layer.
            groundSensor = Physics2D.BoxCast(position, new Vector2(spriteSize.x * 0.5f, boxHt), 0, Vector2.down, distance, groundLayer);
            
            if (groundSensor)  // the 'groundSensor' (a RaycastHit2D) will return "true" if it contains any information at all, and "null" if it doesn't.
            {
                jumpCounter = 0;  // Restore the player's ability to jump, because their feet have touched the ground.
                // print("There's the ground! (jumpCounter = "+jumpCounter+")");
            }
            
            if (jumpRequested)  // Received from a jump input during Update()
            {
                DoJumping();
            }
        }

        void DoJumping()
        {
            jumpCounter++;  // Increment the counter by 1 each time the player jumps, then
            print("jumpCounter = " +  jumpCounter);
            rb.linearVelocityY = 0; // reset vertical velocity so each jump is equally strong, and finally
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);  // add impulse force to the character's positive Y axis, according to the current jump force.
            jumpRequested = false; // Don't forgot to reset the switch so it's all ready to fire again.
        }
    }
}