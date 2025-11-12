using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* CarryObjectManager
 * 
 * Keeps track of carryable objects 
 * 
 */
public class CarryObjectManager : MonoBehaviour
{
    // --- List of Carryable Objects ---
    public List<CarryableObject> carryObjects;

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
        carryObjects = new List<CarryableObject>();
    }

    /* AddObject
     * 
     * Adds a carryable object to the list
     * 
     * Parameters: CarryableObject co, the carryable object to add
     * 
     * Return: None
     * 
     */
    public void AddObject(CarryableObject co)
    {
        if (!carryObjects.Contains(co))
        {
            carryObjects.Add(co);
        }
    }

    /* RemoveObject
     * 
     * Removes a carryable object from the list
     * 
     * Parameters: CarryableObject co, the carryable object to remove
     * 
     * Return: None
     * 
     */
    public void RemoveObject(CarryableObject co)
    {
        if (carryObjects.Contains(co))
        {
            carryObjects.Remove(co);
        }
    }
}
