using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using Color = System.Drawing.Color;

namespace Mechanics
{
    public class HealthMechanic : MonoBehaviour
    {
        // Variables Declaration Zone
        public int maxHealth;
        public int currentHealth;
        bool gotHit = false;
        public LayerMask ouchies;  // In the editor this is set to include the layers "Enemy" and "Hazard"
      
        
        void Start()
        {
            currentHealth = maxHealth;
        }
        
        void FixedUpdate()
        {
            void onCollisionEnter2D(Collision2D collision)
            {
                if (collision.collider.gameObject.layer == ouchies)     // Here's where I'm leaving off for the night, because I can't keep my eyes open or maintain a train of thought.
                {
                    gotHit = true;
                }
            }
            // Take damage
            if (gotHit)
            {
                currentHealth--;  // Reduce current health by 1.
                gotHit = false;  //  Reset the switch.
            }
            
            // Prevent overhealing
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            
            // People die if they are killed
            if (currentHealth <= 0)
            {
                die();
            }
        }
        
        void die()
        {
            // Death behavior. At the moment, this just turns the player sprite red.
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = UnityEngine.Color.red;

        }
    }
}
