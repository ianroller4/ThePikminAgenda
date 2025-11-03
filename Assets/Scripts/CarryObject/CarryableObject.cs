using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider2D))]
public class CarryableObject : MonoBehaviour
{
    // --- Reference Variables ---
    private SLGManager slgManager;
    private NavMeshAgent agent;

    // --- Variables ---
    public int slgNeededForCarry = 1;
    public int slgCarriersMax = 10;
    private List<SillyLittleGuys> carriers;
    public Vector3 target = Vector3.zero;

    private bool canMove = false;

    // Start is called before the first frame update
    private void Start()
    {
        slgManager = GameObject.FindObjectOfType<SLGManager>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.enabled = false;

        carriers = new List<SillyLittleGuys>();
    }

    private void Update()
    {
        HasEnoughCarriers();
        if (canMove)
        {
            agent.SetDestination(target);
        }
    }

    public void AddCarrier(SillyLittleGuys slg)
    {
        if (!carriers.Contains(slg) && carriers.Count < slgCarriersMax)
        {
            carriers.Add(slg);
        }
    }

    public void RemoveCarrier(SillyLittleGuys slg)
    {
        if (carriers.Contains(slg))
        {
            carriers.Remove(slg);
        }
    }

    public bool ReachedTarget()
    {
        return transform.position == target;
    }

    public void HasEnoughCarriers()
    {
        if (carriers.Count > slgNeededForCarry)
        {
            canMove = true;
            agent.enabled = true;
        }
        else
        {
            canMove = false;
            agent.enabled = false; 
        }
    }

    private void AssignCarryPositions()
    {
        
    }

    private void BuildCarryPositions()
    {

    }
}
