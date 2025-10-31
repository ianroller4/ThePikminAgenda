using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SillyLittleGuys : MonoBehaviour
{
    private NavMeshAgent agent;

    private SLGManager slgManager;

    public Vector3 moveToTarget;

    private Vector3 thrownTarget;

    private GameObject player;

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
        state = States.IDLE;
        slgManager = GameObject.FindObjectOfType<SLGManager>();
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
        Debug.Log("In Idle State");
    }

    public void UpdateIdleState()
    {
        // Listen for whistle
        // Look for something to attack
        // Look for something to carry
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

    }

    public void ExitCarryState()
    {
        state = States.IDLE;
    }

    // --- Command Calls ---

    public void OnWhistleCall()
    {
        EnterFollowState();
    }

    public void Dismiss(Vector3 target)
    {
        
    }
}
