using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class SillyLittleGuys : MonoBehaviour
{
    private NavMeshAgent agent;

    // --- Managers ---
    private SLGManager slgManager;
    private CarryObjectManager coManager;
    private EnemyManager enemyManager;

    [SerializeField]
    private GameObject AttackHitboxPrefab;

    [SerializeField]
    private float attackRange = 1f;

    [SerializeField]
    private float attackCooldown = 2f;

    private float originalCooldown;

    private bool isOnAttackCooldown = false;

    public Vector3 moveToTarget;

    private Vector3 thrownTarget;

    private GameObject player;

    public float idleSearchRange = 2;

    private CarryableObject carryObject = null;

    private Enemy targetEnemy;

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

    // --- Animator ---
    private Animator animator;
    private Vector3 lastDir = Vector3.zero;
    private Vector3 currentDir = Vector3.zero;
    private Vector3 prevPosition = Vector3.zero;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
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

        originalCooldown = attackCooldown;
        prevPosition = transform.position;
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
        animator.SetBool("idle", true);
        animator.SetBool("attack", false);
        animator.SetFloat("x", lastDir.x);
        animator.SetFloat("y", lastDir.y);
    }

    public void UpdateIdleState()
    {
        // Look for something to attack
        for (int i = 0; i < enemyManager.enemies.Count; i++)
        {
            if (Vector3.Distance(transform.position, enemyManager.enemies[i].transform.position) < idleSearchRange)
            {
                targetEnemy = enemyManager.enemies[i];
                EnterAttackState();
                break;
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
                break;
            }
        }
    }

    // --- FOLLOW State --- 

    public void EnterFollowState()
    {
        state = States.FOLLOW;
        slgManager.AddFollowingSLG(this);
        animator.SetBool("attack", false);
    }

    public void UpdateFollowState()
    {
        agent.SetDestination(moveToTarget);
        bool result = UpdateDirVector();
        if (result)
        {
            animator.SetBool("idle", false);
            animator.SetFloat("x", currentDir.x);
            animator.SetFloat("y", currentDir.y);
        }
        else
        {
            animator.SetBool("idle", true);
            animator.SetFloat("x", lastDir.x);
            animator.SetFloat("y", lastDir.y);
        }
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
        animator.SetBool("idle", false);
    }

    public void UpdateDismissState()
    {
        if (Vector3.Distance(moveToTarget, transform.position) < 0.2)
        {
            Debug.Log("Ended dismiss");
            ExitDismissState();
            animator.SetBool("idle", true);
        }
        else
        {
            UpdateDirVector();
            animator.SetFloat("x", currentDir.x);
            animator.SetFloat("y", currentDir.y);
        }
    }

    public void ExitDismissState()
    {
        EnterIdleState();
    }

    // --- ATTACK State --- 

    public void EnterAttackState()
    {
        state = States.ATTACK;
        slgManager.RemoveFollowingSLG(this);
        animator.SetBool("idle", false);
    }

    public void UpdateAttackState()
    {
        if (targetEnemy != null)
        {
            if (!isOnAttackCooldown)
            {
                if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= attackRange)
                {
                    animator.SetBool("attack", true);
                    Vector3 attackDir = targetEnemy.transform.position - transform.position;
                    animator.SetFloat("x", attackDir.x);
                    animator.SetFloat("y", attackDir.y);
                    isOnAttackCooldown = true;
                    agent.isStopped = true;
                    attackCooldown = originalCooldown;
                    Instantiate(AttackHitboxPrefab, targetEnemy.transform.position, Quaternion.identity);
                }
                else
                {
                    animator.SetBool("attack", false);
                    UpdateDirVector();
                    animator.SetFloat("x", currentDir.x);
                    animator.SetFloat("y", currentDir.y);
                    agent.isStopped = false;
                    agent.SetDestination(targetEnemy.transform.position);
                }
            }
            else
            {
                attackCooldown -= Time.deltaTime;
                if (attackCooldown < 0)
                {
                    isOnAttackCooldown = false;
                }
            }
        }
        else
        {
            agent.isStopped = false;
            EnterIdleState();
            Debug.Log("Exiting Attack");
        }
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
        if(state == States.ATTACK)
        {
            agent.isStopped = false;
        }

        if (carryObject != null)
        {
            carryObject.RemoveCarrier(this);
            carryObject = null;
        }
        EnterFollowState();
    }
    private bool UpdateDirVector()
    {
        bool result = false;
        if (transform.position.x != prevPosition.x || transform.position.y != prevPosition.y)
        {
            currentDir = (transform.position - prevPosition).normalized;
            lastDir = currentDir;
            prevPosition = transform.position;
            result = true;
        }
        return result;
    }
}
