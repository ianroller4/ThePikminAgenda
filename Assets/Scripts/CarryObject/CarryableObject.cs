using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/* CarryableObject
 * 
 * Enables an object to be carried by SLGs
 * 
 */
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider2D))]
public class CarryableObject : MonoBehaviour
{
    // --- Reference Variables ---
    private SLGManager slgManager;
    private CarryObjectManager coManager;
    private NavMeshAgent agent;

    // --- Variables ---
    public int slgNeededForCarry = 1;
    public int slgCarriersMax = 10;
    private List<SillyLittleGuys> carriers;
    public Vector3 target = Vector3.zero;

    private bool canMove = false;

    private List<Vector3> carryPositions;

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
        coManager = GameObject.FindObjectOfType<CarryObjectManager>();
        coManager.carryObjects.Add(this);
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.enabled = false;

        carriers = new List<SillyLittleGuys>();
        carryPositions = new List<Vector3>();
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
        BuildCarryPositions();
        AssignCarryPositions();
        HasEnoughCarriers();
        if (canMove)
        {
            agent.SetDestination(target);
            if (ReachedTarget())
            {
                Debug.Log("Reached target");
                for (int i = 0; i < carriers.Count; i++)
                {
                    carriers[i].EnterIdleState();
                }
                carriers.Clear();
            }
        }
    }

    /* AddCarrier
     * 
     * Adds an SLG to the carriers list
     * 
     * Parameters: SillyLittleGuys slg, the SLG to add to the carriers
     * 
     * Return: None
     * 
     */
    public void AddCarrier(SillyLittleGuys slg)
    {
        if (!carriers.Contains(slg) && carriers.Count < slgCarriersMax)
        {
            carriers.Add(slg);
        }
    }

    /* RemoveCarrier
     * 
     * Removes an SLG from the carriers list
     * 
     * Parameters: SillyLittleGuys slg, the SLG to add to the carriers
     * 
     * Return: None
     * 
     */
    public void RemoveCarrier(SillyLittleGuys slg)
    {
        if (carriers.Contains(slg))
        {
            carriers.Remove(slg);
        }
    }

    /* ReachedTarget
     * 
     * Checks if the object has reached its target position
     * 
     * Parameters: None
     * 
     * Return: bool result, true if reached target, false if not
     * 
     */
    public bool ReachedTarget()
    {
        bool result = false;
        // Reached if object is within 0.6 units, Unity's navmesh sucks and it only ever gets to within 0.58... units
        if (Vector3.Distance(transform.position, target) < 0.6)
        {
            result = true;
        }
        Debug.Log("Reached target: " + result);
        Debug.Log("Distance to target: " + Vector3.Distance(transform.position, target));
        return result;
    }

    /* HasEnoughCarriers
     * 
     * Check if the object has the required number of SLGs to move the object
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void HasEnoughCarriers()
    {
        if (carriers.Count >= slgNeededForCarry)
        {
            canMove = true;
            agent.enabled = true;
            // Update speed based on number of carriers
            agent.speed = Mathf.Lerp(1, 2, (carriers.Count - slgNeededForCarry) / (slgCarriersMax - slgNeededForCarry));
        }
        else
        {
            canMove = false;
            agent.enabled = false; 
        }
    }

    /* AssignCarryPositions
     * 
     * Assign positions to carriers
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void AssignCarryPositions()
    {
        for (int i = 0; i < carriers.Count; i++)
        {
            carriers[i].moveToTarget = carryPositions[i];
        }
    }

    /* BuildCarryPositions
     * 
     * Builds x positions in a ring around the object 
     * 
     * Parameters: None
     *             
     * Return: None
     * 
     */
    private void BuildCarryPositions()
    {
        carryPositions.Clear(); // Clear positions

        // Variables for building positions
        float angle;
        Vector3 dir;
        Vector3 position;

        // If carriers are present
        if (carriers.Count > 0)
        {
            // Up to max carriers build positions
            for (int i = 0; i < slgCarriersMax; i++)
            {
                // Angle of position
                angle = i * (360f / carriers.Count);
                // Rotate angle for direction
                dir = ApplyRotationToVector(Vector3.right, angle);
                // Make vector
                position = transform.position + dir * transform.localScale.x;
                // Add position
                carryPositions.Add(position);
            }
        }
    }

    /* ApplyRotationToVector
     * 
     * Rotates a vector v by angle
     * 
     * Parameters: Vector3 v, vector to rotate
     *             float angle, angle to rotate v to 
     * 
     * Return: Vector3, the rotated vector
     * 
     */
    private Vector3 ApplyRotationToVector(Vector3 v, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * v;
    }
}
