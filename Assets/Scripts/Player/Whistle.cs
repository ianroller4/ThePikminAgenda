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
    public float whistleRadiusIncreaseSpeed = 2f;

    // --- Rotation ---
    public GameObject outerCircle;
    public GameObject innerCircle;
    public float baseRotationSpeed = 20f;
    public float fastRotationSpeed = 60f;
    private float rotationSpeed;

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
        rotationSpeed = baseRotationSpeed;
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
        Rotate();
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
            currentWhistleRadius += whistleRadiusIncreaseSpeed * Time.deltaTime;
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
            rotationSpeed = fastRotationSpeed;
        }
        else
        {
            currentWhistleRadius = whistleRadiusStart;
            transform.localScale = new Vector3(currentWhistleRadius * 2, currentWhistleRadius * 2, 1);
            rotationSpeed = baseRotationSpeed;
        }
    }

    private void Rotate()
    {
        Vector3 rotate = new Vector3(0, 0, rotationSpeed * Time.deltaTime);
        outerCircle.transform.Rotate(rotate);
        innerCircle.transform.Rotate(-rotate);
    }
}
