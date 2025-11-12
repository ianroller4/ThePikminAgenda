using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* FixedFollow
 * 
 * Fixes an object behind another at a set distance without needing child objects and rotations
 * 
 */
public class FixedFollow : MonoBehaviour
{
    // Distance to follow at
    public float followDistance = 2f;

    // The object to follow
    private Transform parentTransform;
    private Rigidbody2D parentRB;

    // Position behind object
    private Vector3 distanceBehindPlayer;

    /* Start
     * 
     * Called once before the first frame of update
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    void Start()
    {
        parentTransform = transform.parent;
        parentRB = parentTransform.gameObject.GetComponent<Rigidbody2D>();
        transform.position = parentTransform.position - Vector3.up * followDistance;
    }

    /* Update
     * 
     * Called once per frame
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    void Update()
    {
        UpdateDistanceBehindPlayer();
    }

    /* UpdateDistanceBehindPlayer
     * 
     * Gets position behind the player and scales it by follow distance
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void UpdateDistanceBehindPlayer()
    {
        if (parentRB != null)
        {
            if (parentRB.velocity != Vector2.zero)
            {
                // Get direction object is moving
                distanceBehindPlayer = new Vector3(parentRB.velocity.normalized.x, parentRB.velocity.normalized.y, 0);
                // Update position to be behind
                transform.position = parentTransform.position - distanceBehindPlayer * followDistance;
            }
        }
    }

}
