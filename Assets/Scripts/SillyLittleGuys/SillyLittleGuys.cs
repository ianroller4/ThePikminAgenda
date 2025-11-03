using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SillyLittleGuys : MonoBehaviour
{
    private NavMeshAgent agent;

    // --- Managers ---
    private SLGManager slgManager;
    private CarryObjectManager coManager;
    private EnemyManager enemyManager;

    public Vector3 moveToTarget;

    private Vector3 thrownTarget;

    private GameObject player;

    public float idleSearchRange = 2;

    private CarryableObject carryObject = null;

    public enum States
    {
        IDLE,
        FOLLOW,
        HELD,
        THROWN,
        DISMISS,
        ATTACK,
        CARRY
    }
    public States state;

    // Start is called before the first frame update
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.enabled = true;
        EnterIdleState();

        slgManager = GameObject.FindObjectOfType<SLGManager>();
        coManager = GameObject.FindObjectOfType<CarryObjectManager>();
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();

        slgManager.AddSLG(this);
        moveToTarget = transform.position;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        switch (state)
        {
            case States.IDLE:
                UpdateIdleState();
                break;
            case States.FOLLOW:
                UpdateFollowState();
                break;
            case States.HELD:
                UpdateHeldState();
                break;
            case States.THROWN:
                UpdateThrownState();
                break;
            case States.DISMISS:
                UpdateDismissState();
                break;
            case States.ATTACK:
                UpdateAttackState();
                break;
            case States.CARRY:
                UpdateCarryState();
                break;
        }
    }

    public States GetState()
    {
        return state;
    }

    // --- IDLE State --- 

    public void EnterIdleState()
    {
        state = States.IDLE;
    }

    public void UpdateIdleState()
    {
        // Look for something to attack
        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            if (Vector3.Distance(transform.position, enemyManager.enemies[i].transform.position) < idleSearchRange)
            {

            }
        }
        // Look for something to carry
        for (int i = 0; i < coManager.carryObjects.Count; i++)
        {
            if (Vector3.Distance(transform.position, coManager.carryObjects[i].transform.position) < idleSearchRange)
            {
                coManager.carryObjects[i].AddCarrier(this);
                carryObject = coManager.carryObjects[i];
                EnterCarryState();
                Debug.Log("I will Carry this!");
                break;
            }
        }
    }

    public void ExitIdleState()
    {

    }

    // --- FOLLOW State --- 

    public void EnterFollowState()
    {
        state = States.FOLLOW;
        slgManager.AddFollowingSLG(this);
    }

    public void UpdateFollowState()
    {
        agent.SetDestination(moveToTarget);
    }


    public void ExitFollowState()
    {

    }

    // --- HELD State --- 

    public void EnterHeldState()
    {
        state = States.HELD;
        slgManager.RemoveFollowingSLG(this);
    }

    public void UpdateHeldState()
    {
        GetComponent<Collider2D>().enabled = false;
        agent.enabled = false;
        transform.position = player.transform.position + Vector3.right;
    }

    public void ExitHeldState()
    {
        state = States.FOLLOW;
        GetComponent<Collider2D>().enabled = true;
        agent.enabled = true;
        slgManager.AddFollowingSLG(this);
    }

    // --- THROWN State --- 

    public void EnterThrownState(Vector3 target)
    {
        state = States.THROWN;
        thrownTarget = target;
    }

    public void UpdateThrownState()
    {
        // Add throw movement
        transform.position = thrownTarget;
        if (transform.position == thrownTarget)
        {
            ExitThrownState();
        }
    }

    public void ExitThrownState()
    {
        state = States.IDLE;
        agent.enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }

    // --- DISMISS State ---

    public void EnterDismissState(Vector3 dismissSpot)
    {
        state = States.DISMISS;
        moveToTarget = dismissSpot;
        agent.SetDestination(moveToTarget);
    }

    public void UpdateDismissState()
    {
        if (transform.position == moveToTarget)
        {
            ExitDismissState();
        }
    }

    public void ExitDismissState()
    {
        state = States.IDLE;
    }

    // --- ATTACK State --- 

    public void EnterAttackState()
    {
        state = States.ATTACK;
    }

    public void UpdateAttackState()
    {

    }

    public void ExitAttackState()
    {
        state = States.IDLE;
    }

    // --- CARRY State --- 

    public void EnterCarryState()
    {
        state = States.CARRY;
    }

    public void UpdateCarryState()
    {
        agent.SetDestination(moveToTarget);
    }

    public void ExitCarryState()
    {
        state = States.IDLE;
    }

    // --- Command Calls ---

    public void OnWhistleCall()
    {
        if (carryObject != null)
        {
            carryObject.RemoveCarrier(this);
            carryObject = null;
        }
        EnterFollowState();
    }

    public void Dismiss(Vector3 target)
    {
        
    }
}
