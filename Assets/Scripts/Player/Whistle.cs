using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Whistle
 * 
 * Handles input for whistle and calling SLGs
 * 
 */
public class Whistle : MonoBehaviour
{
    private SLGManager slgManager;

    private bool isWhistling = false;

    public float whistleRadiusStart = 0.5f;
    public float whistleRadiusMax = 2.5f;
    private float currentWhistleRadius;

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
        currentWhistleRadius = whistleRadiusStart;
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
        UpdatePosition();
        ListenForInput();
        WhistleForSLG();
    }

    /* UpdatePosition
     * 
     * Updates the position of the cursor to the mouse position
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void UpdatePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;
        
    }

    /* ListenForInput
     * 
     * Listends for right click
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void ListenForInput()
    {
        isWhistling = Input.GetMouseButton(1);
    }

    /* WhistleForSLG
     * 
     * Handles checking for SLGs in a growing radius of the whistle area
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void WhistleForSLG()
    {
        if (isWhistling)
        {
            // Increase radius
            currentWhistleRadius += Time.deltaTime;
            if (currentWhistleRadius > whistleRadiusMax)
            {
                currentWhistleRadius = whistleRadiusMax;
            }
            transform.localScale = new Vector3(currentWhistleRadius * 2, currentWhistleRadius * 2, 1);
            foreach (SillyLittleGuys slg in slgManager.SLGList)
            {
                if (Vector2.Distance(slg.transform.position, transform.position) < currentWhistleRadius)
                {
                    slg.OnWhistleCall();
                }
            }
        }
        else
        {
            currentWhistleRadius = whistleRadiusStart;
            transform.localScale = new Vector3(currentWhistleRadius * 2, currentWhistleRadius * 2, 1);
        }
    }
}
