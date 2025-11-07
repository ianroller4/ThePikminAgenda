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
    private CarryObjectManager coManager;
    private NavMeshAgent agent;

    // --- Variables ---
    public int slgNeededForCarry = 1;
    public int slgCarriersMax = 10;
    private List<SillyLittleGuys> carriers;
    public Vector3 target = Vector3.zero;

    private bool canMove = false;

    private List<Vector3> carryPositions;

    // Start is called before the first frame update
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
        bool result = false;
        if (Vector3.Distance(transform.position, target) < 0.6)
        {
            result = true;
        }
        Debug.Log("Reached target: " + result);
        Debug.Log("Distance to target: " + Vector3.Distance(transform.position, target));
        return result;
    }

    public void HasEnoughCarriers()
    {
        if (carriers.Count >= slgNeededForCarry)
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
        for (int i = 0; i < carriers.Count; i++)
        {
            carriers[i].moveToTarget = carryPositions[i];
        }
    }

    private void BuildCarryPositions()
    {
        carryPositions.Clear();

        float angle;
        Vector3 dir;
        Vector3 position;

        for (int i = 0; i < slgCarriersMax; i++)
        {
            angle = i * (360f / slgCarriersMax);
            dir = ApplyRotationToVector(Vector3.right, angle);
            position = transform.position + dir;
            carryPositions.Add(position);
        }
    }

    private Vector3 ApplyRotationToVector(Vector3 v, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * v;
    }
}
