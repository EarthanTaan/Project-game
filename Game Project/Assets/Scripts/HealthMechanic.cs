using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using Color = System.Drawing.Color;
using UnityEngine.Events;

namespace Mechanics
{
    public class HealthMechanic : MonoBehaviour
    {
        // Variables Declaration Zone
        private SpriteRenderer spriteRenderer;
        public int maxHealth;
        int currentHealth;
        bool gotHit = false;
        bool gotHealed = false;
        public LayerMask ouchies;  // In the editor this is set to include the layers "Enemy" and "Hazard"
        public LayerMask yummies;  // This currently doesn't contain anything because we don't have any healing objects or associated layers.


        void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            currentHealth = maxHealth;  // The player starts at full health.
        }
        
        void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Health: " +  currentHealth + "/" +  maxHealth); //  Display health status, in absence of UI feedback.
            if (yummies.Contains(collision.gameObject.layer))
            {
                gotHealed = true;
            }
            
            if (ouchies.Contains(collision.gameObject.layer))
            {
                gotHit = true;
            }
        }
        void FixedUpdate()
        {
            // Healing damage appears first in the script to ensure that if the player is healed at the same instant
            // that they take what would have been lethal damage - the heal occurs first, potentially preventing death.
            if (gotHealed)
            {
                HealDamage();
            }
            
            // Take damage
            if (gotHit)
            {
                TakeDamage();
            }

            
            
        }

        // Restore health
        private void HealDamage(int  amount = 1)
        {
            currentHealth += amount;
            // Prevent overhealing
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            gotHealed = false;
        }


        void TakeDamage(int amount = 1)
        {
            currentHealth -= amount;  // Reduce current health by the amount given in the call, with a default of 1 if no amount is specified.
            
            // Prevent negative health. Zero should be the minimum. This is just to prevent unwanted unexpected behavior later on.
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            
            // People die if they are killed
            if (currentHealth <= 0)
            {
                Die();
            }
            
            gotHit = false;  //  Reset the switch.
        }
        
        void Die()
        {
            // Death behavior. At the moment, this just turns the player sprite red.
            spriteRenderer.color = UnityEngine.Color.red;
        }
    }
    
    public static class UnityExtensions
    {
        /// <summary>
        /// Extension method to check if a layer is in a layermask
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }
    }
}
