using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* EnemyManager
 * 
 * Keeps track of enemies
 * 
 */
public class EnemyManager : MonoBehaviour
{
    // --- List of Enemies ---
    public List<Enemy> enemies;

    /* Awake
     * 
     * Called once when script is loaded in
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void Awake()
    {
        enemies = new List<Enemy>();
    }

    /* AddEnemy
     * 
     * Adds the passed enemy to the list
     * 
     * Parameters: Enemy enemy, enemy to add
     * 
     * Return: None
     * 
     */
    public void AddEnemy(Enemy enemy) 
    { 
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    /* RemoveEnemy
     * 
     * Removes the passed enemy from the list
     * 
     * Parameters: Enemy enemy, enemy to remove
     * 
     * Return: None
     * 
     */
    public void RemoveEnemy(Enemy enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
}
