using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Return,
        Chase,
        Attack,
        Dead
    }

    private EnemyState currentState;

    private NavMeshAgent agent;

    // --- References ---
    private Rigidbody2D rigid;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private GameObject AttackHitboxPrefab;
    private EnemyManager enemyManager;
    private Animator animator;

    // --- Variables ---
    private Vector3 startPos;
    private Vector3 targetPos;
    [SerializeField]
    private float detectRadius = 3f;
    [SerializeField]
    [Tooltip("The maximum distance between the enemy and the SLG before the enemy stops chasing.")]
    private float chaseStopDistance = 4f;
    [SerializeField]
    [Tooltip("If the enemy moves farther than this distance from its start position, it will return.")]
    private float maxLeashDistance = 5f;
    [SerializeField]
    private float attackRange = 1f;
    private float targetUpdateInterval = 0.5f;
    private Vector3 attackPos;
    private bool hasAttacked;

    // --- Timers ---
    private float targetUpdateTimer = 0f;
    private float attackTimer = 0f;

    // --- Animation ---
    private Vector3 lastPos;
    private Vector3 moveDir;
    private Vector2 lastLookDir;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        enemyManager = GameObject.FindObjectOfType<EnemyManager>();

        enemyManager.AddEnemy(this);

        startPos = transform.position;

        currentState = EnemyState.Idle;

        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Return:
                Return();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Dead:
                Dead();
                break;
        }

        Debug.Log(currentState);
    }

    private void Idle()
    {
        animator.SetBool("isAttack", false);
        animator.SetBool("isIdle", true);
        animator.SetBool("isWalk", false);

        // Search SLGs to chase
        GameObject SLG = searchSLG();

        if (SLG != null)
        {
            Debug.Log("Closest SLG: " + SLG.name);
            target = SLG;
            animator.SetBool("isIdle", false);
            currentState = EnemyState.Chase;
        }

        animator.SetFloat("x", lastLookDir.x);
        animator.SetFloat("y", lastLookDir.y);
    }

    private void Return()
    {
        animator.SetBool("isAttack", false);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalk", true);

        agent.SetDestination(startPos);

        float dist = Vector2.Distance(transform.position, startPos);

        if (dist < 0.01f)
        {
            agent.isStopped = true;
            currentState = EnemyState.Idle;
        }

        UpdateMovementDirection();
    }

    private void Chase()
    {
        animator.SetBool("isAttack", false);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalk", true);

        targetUpdateTimer += Time.deltaTime;

        if (targetUpdateTimer >= targetUpdateInterval)
        {
            targetUpdateTimer = 0f;

            GameObject SLG = searchSLG();

            if (SLG != null)
            {
                Debug.Log("Closest SLG: " + SLG.name);
                target = SLG;
            }
        }
        agent.SetDestination(target.transform.position);

        float dist = Vector2.Distance(transform.position, target.transform.position);

        // stop moving if any SLGs gets within Attack range
        if (dist < attackRange)
        {
            agent.isStopped = true;
            animator.SetBool("isWalk", false);
            currentState = EnemyState.Attack;
        }
        else
        {
            agent.isStopped = false;
        }

        float distFromStart = Vector2.Distance(transform.position, startPos);

        if (dist > chaseStopDistance || distFromStart >= maxLeashDistance)
        {
            target = null;
            currentState = EnemyState.Return;
            agent.ResetPath();
        }

        UpdateMovementDirection();
    }

    private void Attack()
    {
        animator.SetBool("isAttack", true);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalk", false);

        if (attackTimer == 0f && target != null)
        {
            hasAttacked = false;
            attackPos = target.transform.position;
            Vector3 dir = (target.transform.position - transform.position).normalized;
            animator.SetFloat("x", dir.x);
            animator.SetFloat("y", dir.y);

            lastLookDir = new Vector2(dir.x, dir.y);
        }

        attackTimer += Time.deltaTime;

        if (attackTimer >= 1.1f)
        {
            attackTimer = 0f;
            target = null;
            agent.ResetPath();
            animator.SetBool("isAttack", false);
            currentState = EnemyState.Idle;
        }

    }

    private void Dead()
    {

    }

    private GameObject searchSLG()
    {
        Vector2 currentPos = transform.position;
        int layerMask = 1 << LayerMask.NameToLayer("SLG");

        Collider2D[] cols = Physics2D.OverlapCircleAll(currentPos, detectRadius, layerMask);

        GameObject closest = null;
        float minDist = 999f;

        for (int i = 0; i < cols.Length; i++)
        {
            Collider2D col = cols[i];

            float dist = Vector2.Distance(currentPos, col.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = col.gameObject;
            }
        }

        return closest;
    }

    private void UpdateMovementDirection()
    {
        moveDir = (transform.position - lastPos).normalized;
        lastPos = transform.position;

        animator.SetFloat("x", moveDir.x);
        animator.SetFloat("y", moveDir.y);

        if (moveDir.sqrMagnitude > 0.0001f)
        {
            lastLookDir = new Vector2(moveDir.x, moveDir.y);
        }
    }

    public void SpawnAttackHitbox()
    {
        // To make sure creating one hitbox prefab for one attack
        if (hasAttacked)
        {
            return;
        }

        hasAttacked = true;
        Instantiate(AttackHitboxPrefab, attackPos, Quaternion.identity);
    }
}
