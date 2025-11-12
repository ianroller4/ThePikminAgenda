using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Dismiss
 * 
 * Handles input from the player for running dismiss command
 * 
 */
public class Dismiss : MonoBehaviour
{
    private SLGManager slgManager;

    /* Start
     * 
     * Called once before the first frame of update
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void Start()
    {
        slgManager = GameObject.FindObjectOfType<SLGManager>();
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            slgManager.OnDismiss();
        }
    }
}
