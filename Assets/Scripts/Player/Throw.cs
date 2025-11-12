using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Throw
 * 
 * Handles throwing and SLG
 * 
 */
public class Throw : MonoBehaviour
{
    private SLGManager slgManager;

    private SillyLittleGuys heldSLG = null;

    private bool throwing = false;

    private bool canceledThrow = false;

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
        if (Input.GetMouseButton(0) && !canceledThrow)
        {
            GrabSLG();
            if (Input.GetKeyDown(KeyCode.Q) && heldSLG != null)
            {
                heldSLG.ExitHeldState();
                heldSLG = null;
                canceledThrow = true;
            }
        }
        else if (!Input.GetMouseButton(0))
        {
            canceledThrow = false;
            if (throwing)
            {
                ThrowSLG();
            }
        }
    }

    /* GrabSLG
     * 
     * Gets an SLG to throw
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void GrabSLG()
    {
        // If not holding anything
        if (heldSLG == null)
        {
            // Get SLG to hold
            heldSLG = slgManager.GetNextSLGForThrow();
            if (heldSLG != null)
            {
                throwing = true;
                heldSLG.EnterHeldState();
            }
        }
    }

    /* ThrowSLG
     * 
     * Throws an SLG to where the mouse is located when throw button is released
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void ThrowSLG()
    {
        if (heldSLG != null)
        {
            // Get mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            // Set SLG to thrown state
            heldSLG.EnterThrownState(mousePosition);
            heldSLG = null;
            throwing = false;
        }

    }
}
