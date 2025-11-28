using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    // --- States ---
    private enum BossState
    {
        ChooseNormalAttack,
        ChooseBigAttack,
        Chase,
        Groggy,
        NormalMeleeAttack,
        RangeAttack,
        RollingAttack,
        RockFragmentsAttack
    }
    private BossState currentState;

    // --- Components ---
    private Rigidbody2D rigid;
    private NavMeshAgent agent;
    private Animator animator;
    private SpriteRenderer sr;

    // --- References ---
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject NormalAttackPrefab;
    [SerializeField]
    private GameObject RangeAttackPrefab;
    [SerializeField]
    private GameObject CrumblingRockPrefab;
    [SerializeField]
    private GameObject RemaningRockPrefab;

    // --- Variables ---
    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 attackPos;
    private float targetUpdateInterval = 0.5f;
    private bool hasAttacked;

    // --- Settings ---
    [SerializeField]
    private float attackRange = 1f;

    // --- Timers ---
    private float targetUpdateTimer = 0f;
    private float attackTimer = 0f;
    private float stateTimer = 0f;
    private float normalAttackTimer = 0f;

    // --- Animation related valuable ---
    private Vector3 lastPos;
    private Vector3 moveDir;
    private Vector2 lastLookDir;

    // initialize references
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        currentState = BossState.Chase;

        lastPos = transform.position;
    }

    // update enemy state
    void Update()
    {
        switch (currentState)
        {
            case BossState.ChooseBigAttack:
                ChooseBigAttack();
                break;

            case BossState.Chase:
                Chase();
                break;

            case BossState.Groggy:
                Groggy();
                break;

            case BossState.NormalMeleeAttack:
                NormalMeleeAttack();
                break;

            case BossState.RangeAttack:
                RangeAttack();
                break;

            case BossState.RollingAttack:
                RollingAttack();
                break;

            case BossState.RockFragmentsAttack:
                RockFragmentsAttack();
                break;
        }

        Debug.Log(currentState);
    }

    /* Chase
     * 
     * Continuously moves toward the target SLG
     * 
     * When the SLG is close enough, starts an attack
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */

    private void ChooseNormalAttack()
    {

    }

    private void Chase()
    {
        animator.SetBool("isChase", true);

        targetUpdateTimer += Time.deltaTime;

        // ------------------Chasing-------------------
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
        //----------------------------------------------


        // Choosing which normal attack proceed
        float dist = Vector2.Distance(transform.position, target.transform.position);

        // if slgs are close, excute meele attack
        if (dist < attackRange)
        {
            normalAttackTimer = 0f;
            animator.SetBool("isChase", false);
            currentState = BossState.NormalMeleeAttack;
        }
        else
        {
            normalAttackTimer += Time.deltaTime;
            agent.isStopped = false;
        }

        // if slgs are far for 5secs, excute range attack
        if(normalAttackTimer <= 5f)
        {
            animator.SetBool("isChase", false);
            currentState = BossState.RangeAttack;
        }

        UpdateMovementDirection();
    }

    /* Attack
     * 
     * Saves the attack position and plays the attack animation
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void NormalMeleeAttack()
    {
        animator.SetBool("isMeleeAttack", true);
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalk", false);

        if (attackTimer == 0f && target != null)
        {
            hasAttacked = false;
            attackPos = target.transform.position; // saves the attack position
            Vector3 dir = (target.transform.position - transform.position).normalized;
            animator.SetFloat("x", dir.x);
            animator.SetFloat("y", dir.y);

            lastLookDir = new Vector2(dir.x, dir.y);
        }

        attackTimer += Time.deltaTime;

        if (attackTimer >= 1.1f) // attack time 1.1secs
        {
            attackTimer = 0f;
            target = null;
            agent.ResetPath();
            animator.SetBool("isAttack", false);
            currentState = EnemyState.Idle;
        }

    }

    /* searchSLG
     *
     * Searches for SLGs within the detection radius and returns the closest one.
     *
     * Parameters: None
     *
     * Return: GameObject (closest SLG) or Null
     * 
     */
    private GameObject searchSLG()
    {
        Vector2 currentPos = transform.position;
        int layerMask = 1 << LayerMask.NameToLayer("SLG");

        Collider2D[] cols = Physics2D.OverlapCircleAll(currentPos, 5f, layerMask);

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

    /* UpdateMovementDirection
     *
     * Calculates movement direction and updates animation parameters
     *
     * Parameters: None
     *
     * Return: None
     * 
     */
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

    /* SpawnAttackHitbox
     *
     * Spawns a hitbox at the attack position once per attack.
     *
     * Parameters: None
     *
     * Return: None
     * 
     */
    public void SpawnNormalMeeleAttackHitbox()
    {
        // To make sure creating one hitbox prefab for one attack
        if (hasAttacked)
        {
            return;
        }
        hasAttacked = true;

        Instantiate(NormalAttackPrefab, attackPos, Quaternion.identity);
    }
}
