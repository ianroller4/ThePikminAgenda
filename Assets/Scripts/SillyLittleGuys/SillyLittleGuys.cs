using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SillyLittleGuys : MonoBehaviour
{
    public GameObject target;

    private NavMeshAgent agent;

    private SLGManager slgManager;

    public enum States
    {
        IDLE,
        FOLLOW,
        HELD,
        THROWN,
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
        state = States.IDLE;
        slgManager = GameObject.FindObjectOfType<SLGManager>();
        slgManager.AddSLG(this);
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
    }

    public void UpdateFollowState()
    {
        agent.SetDestination(target.transform.position);
    }


    public void ExitFollowState()
    {

    }

    // --- HELD State --- 

    public void EnterHeldState()
    {

    }

    public void UpdateHeldState()
    {
        // Disable collision
        // Disable movement
        // Move to point close to player
        // Go to thrown
    }

    public void ExitHeldState()
    {

    }

    // --- THROWN State --- 

    public void EnterThrownState()
    {

    }

    public void UpdateThrownState()
    {
        // Move to cursor point
        // Go back to idle
    }

    public void ExitThrownState()
    {

    }

    // --- ATTACK State --- 

    public void EnterAttackState()
    {

    }

    public void UpdateAttackState()
    {

    }

    public void ExitAttackState()
    {

    }

    // --- CARRY State --- 

    public void EnterCarryState()
    {

    }

    public void UpdateCarryState()
    {

    }

    public void ExitCarryState()
    {

    }

    // --- Command Calls ---

    public void OnWhistleCall()
    {
        EnterFollowState();
    }
}
