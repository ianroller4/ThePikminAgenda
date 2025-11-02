using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Sleep,
        WakeUp,
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

    // --- Variables ---
    private Vector3 startPos;
    private Vector3 targetPos;
    [SerializeField]
    private float chaseDistance = 4f;
    private bool isReturning = false;
    private bool isOnAttackCooldown = false;
    private Color originalColor;
    private float targetUpdateInterval = 0.5f;

    // --- Timers ---
    private float idleTimer = 0f;
    private float targetUpdateTimer = 0f;
    private float returnTimer = 0f;
    private float attackTimer = 0f;
    private float attackCooldownTimer = 0f;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        startPos = transform.position;

        currentState = EnemyState.Idle;

        originalColor = Color.gray;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                Idle();
                break;

            case EnemyState.Sleep:
                Sleep();
                break;

            case EnemyState.WakeUp:
                WakeUp();
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

        if(isOnAttackCooldown)
        {
            attackCooldownTimer += Time.deltaTime;

            if(attackCooldownTimer >= 2f)
            {
                isOnAttackCooldown = false;
            }
        }

        Debug.Log(currentState);
    }

    private void Idle()
    {
        // Search SLGs to chase.
        GameObject SLG = searchSLG();

        if (SLG != null)
        {
            Debug.Log("Closest SLG: " + SLG.name);
            target = SLG;
            currentState = EnemyState.Chase;
        }


        if (idleTimer <= 0f && !isReturning)
        {
            targetPos = transform.position;

            Vector2 randomDir = Random.insideUnitCircle.normalized;

            float randomDist = Random.Range(0.5f, 2f);

            targetPos = transform.position + (Vector3)(randomDir * randomDist);

            idleTimer = Random.Range(3f, 6f);
        }

        if (isReturning)
        {
            float dist = Vector2.Distance(transform.position, startPos);

            if (dist < 1f)
            {
                returnTimer += Time.deltaTime;
                if (returnTimer >= 2f)
                {
                    isReturning = false;
                }
            }
        }

        if (!isReturning)
        {
            float randomSpeed = Random.Range(0.5f, 2f);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * randomSpeed);
            idleTimer -= Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime * 3f);
        }
        
    }

    private void Sleep()
    {

    }

    private void WakeUp()
    {

    }

    private void Chase()
    {
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

        sr.color = Color.green;

        agent.SetDestination(target.transform.position);

        float dist = Vector2.Distance(transform.position, target.transform.position);

        // stop moving if any SLGs gets within 0.8units
        if (dist < 0.8f)
        {
            agent.isStopped = true;
            if (!isOnAttackCooldown)
            {
                currentState = EnemyState.Attack;
            }
        }
        else
        {
            agent.isStopped = false;
        }

        if (dist > chaseDistance)
        {
            target = null;
            currentState = EnemyState.Idle;
            agent.ResetPath();
            isReturning = true;
            returnTimer = 0f;
            sr.color = originalColor;
        }
    }

    private void Attack()
    { 
        attackTimer += Time.deltaTime;
        sr.color = Color.yellow;

        // attack time 0.5secs
        if (attackTimer >= 0.5f)
        {
            attackTimer = 0f;
            target = null;
            agent.ResetPath();
            currentState = EnemyState.Idle;
            sr.color = originalColor;
            isOnAttackCooldown = true;
            attackCooldownTimer = 0f;
        }
    }

    private void Dead()
    {

    }

    private GameObject searchSLG()
    {
        float detectRadius = 2f;
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
}
