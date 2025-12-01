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

    // --- rolling related --- 
    private enum RollingPhase
    {
        None,
        Jump,
        Slam,
        Charging,
        Rolling,
    }
    private RollingPhase rollingPhase;
    private Vector3 rollDirection;
    [SerializeField]
    private float rollingSpeed = 20f;
    private bool isHitRock = false;
    private int rollCount = 0;
    private Vector3 jumpStartPos;
    private Vector3 jumpTarget;
    private Vector3 jumpDir;
    [SerializeField]
    private Vector3[] jumpTargetPosArray = new Vector3[4];
    private float jumpLerp = 0f;


    // --- fragments related ---
    private enum FragmentsPhase
    {
        None,
        Jump,
        Slam,
        Charging,
        Rolling,
    }
    private FragmentsPhase fragmentsPhase;

    // --- Components ---
    private Rigidbody2D rigid;
    private NavMeshAgent agent;
    private Animator animator;
    private SpriteRenderer sr;

    // --- References ---
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private PlayerMovement player;
    [SerializeField]
    private GameObject NormalMeleeAttackPrefab;
    [SerializeField]
    private GameObject RangeAttackPrefab;
    [SerializeField]
    private GameObject CrumblingRockPrefab;
    [SerializeField]
    private GameObject RemaningRockPrefab;
    [SerializeField]
    private Transform rockCenter;

    // --- Variables ---
    private Vector3 startPos;
    private Vector3 targetPos;
    private Vector3 attackPos;
    private float targetUpdateInterval = 0.5f;
    private bool hasAttacked;

    // --- Settings ---
    [SerializeField]
    private float attackRange = 1f;
    [SerializeField]
    private float chaseSpeed = 3f;
    [SerializeField]
    private float returnSpeed = 6f;

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
        rollingPhase = RollingPhase.None;

        startPos = transform.position;
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
        
        // ------------------Return---------------------
        if(target == null)
        {
            agent.isStopped = false;
            agent.speed = returnSpeed;

            agent.SetDestination(startPos);

            return;
        }
        //----------------------------------------------

        agent.isStopped = false;
        agent.speed = chaseSpeed;
        agent.SetDestination(target.transform.position);


        float dist = Vector2.Distance(transform.position, target.transform.position);

        // Choosing which normal attack proceed
        // --------if slgs are close, excute meele attack-------
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
        //-------------------------------------------------------


        //-----if slgs are far for 5secs, excute range attack----
        if(normalAttackTimer >= 5f)
        {
            normalAttackTimer = 0f;
            animator.SetBool("isChase", false);
            currentState = BossState.RangeAttack;
        }
        //--------------------------------------------------------
        UpdateMovementDirection();
    }

    private void NormalMeleeAttack()
    {
        animator.SetBool("isMeleeAttack", true);
        animator.SetBool("isChase", false);

        // setting melee attack pos and dir
        if (attackTimer == 0f && target != null)
        {
            agent.isStopped = true;
            rigid.velocity = Vector2.zero;

            hasAttacked = false;
            Vector3 dir = (target.transform.position - transform.position).normalized;
            animator.SetFloat("x", dir.x);
            animator.SetFloat("y", dir.y);

            lastLookDir = new Vector2(dir.x, dir.y);
        }

        attackTimer += Time.deltaTime;

        // melee attack ends, return to Chase state
        if (attackTimer >= 2f)
        {
            SpawnNormalMeeleAttackHitbox();

            GameObject SLG = searchSLG();

            if (SLG != null)
            {
                Debug.Log("Closest SLG: " + SLG.name);
                target = SLG;
            }

            attackTimer = 0f;
            agent.ResetPath();
            animator.SetBool("isMeleeAttack", false);
            currentState = BossState.Chase;
        }
    }

    private void RangeAttack()
    {
        animator.SetBool("isRangeAttack", true);
        animator.SetBool("isChase", false);

        // setting Range attack pos and dir
        if (attackTimer == 0f && target != null)
        {
            agent.isStopped = true;
            rigid.velocity = Vector2.zero;

            hasAttacked = false;
            attackPos = target.transform.position; // saves the attack position
            Vector3 dir = (target.transform.position - transform.position).normalized;
            animator.SetFloat("x", dir.x);
            animator.SetFloat("y", dir.y);

            lastLookDir = new Vector2(dir.x, dir.y);
        }

        attackTimer += Time.deltaTime;

        // Range attack ends, return to Chase state
        if (attackTimer >= 2f)
        {
            SpawnRangeAttackHitbox();

            GameObject SLG = searchSLG();

            if (SLG != null)
            {
                Debug.Log("Closest SLG: " + SLG.name);
                target = SLG;
            }

            attackTimer = 0f;
            agent.ResetPath();
            animator.SetBool("isRangeAttack", false);
            currentState = BossState.Chase;
        }
    }

    private void RollingAttack()
    {
        switch (rollingPhase)
        {
            case RollingPhase.Jump:
                DoRolling_Jump();
                break;

            case RollingPhase.Slam:
                DoRolling_Slam();
                break;

            case RollingPhase.Charging:
                DoRolling_Charging();
                break;

            case RollingPhase.Rolling:
                DoRolling_Roll();
                break;
        }
    }

    private void DoRolling_Jump()
    {
        int randomIndex = Random.Range(0, 2);
        jumpTarget = jumpTargetPosArray[randomIndex];

        jumpStartPos = transform.position;

        jumpDir = (jumpTarget - jumpStartPos).normalized;

        jumpLerp = 0;

        if (jumpLerp < 1)
        {
            transform.position = CalculateTrajectory();
            jumpLerp += 3f * Time.deltaTime;
        }
        else
        {
            transform.position = jumpTarget;
            rollingPhase = RollingPhase.Slam;
        }

    }
    private Vector3 CalculateTrajectory()
    {
        Vector3 linearProgress = Vector3.Lerp(jumpStartPos, jumpTarget, jumpLerp);
        float offset = Mathf.Sin(jumpLerp * Mathf.PI) * 5f;

        return linearProgress + (Vector3.up * offset);
    }

    private void DoRolling_Slam()
    {
        if (stateTimer == 0f)
        {
            animator.SetTrigger("Slam");

            agent.isStopped = true;
            rigid.velocity = Vector2.zero;

            hasAttacked = false;
        }

        stateTimer += Time.deltaTime;

        // Will create falling rocks in animation clips . don't forget! --->>> SpawnRocksForRollingAttack()

        if (stateTimer >= 2f) // this time(2f) needs to change maching with the animation time! don't forget!
        {
            stateTimer = 0f;
            rollingPhase = RollingPhase.Charging;
        }
    }

    private void DoRolling_Charging()
    {
        if (stateTimer == 0f)
        {
            animator.SetBool("isCharging", true);

            GameObject slg = searchSLG();
            if (slg != null)
            {
                rollDirection = (slg.transform.position - transform.position).normalized;
            }
            else
            {
                rollDirection = (player.transform.position - transform.position).normalized;
            }
        }

        stateTimer += Time.deltaTime;

        if (stateTimer >= 3f)
        {
            animator.SetBool("isCharging", false);
            stateTimer = 0f;
            rollingPhase = RollingPhase.Rolling;
        }
    }

    private void DoRolling_Roll()
    {
        if (stateTimer == 0f)
        {
            animator.SetBool("isRolling", true);

            agent.enabled = false;

            rigid.velocity = rollDirection * rollingSpeed;
            rollCount++;
        }

        stateTimer += Time.deltaTime;

        if (stateTimer >= 4f)
        {
            animator.SetBool("isRolling", false);

            rigid.velocity = Vector2.zero;
            rigid.angularVelocity = 0f;

            agent.enabled = true;
            agent.isStopped = false;

            stateTimer = 0f;

            if (isHitRock)
            {
                isHitRock = false;
                rollingPhase = RollingPhase.None;
                currentState = BossState.Groggy;

                return;
            }

            if (rollCount >= 3)
            {
                rollCount = 0;
                rollingPhase = RollingPhase.None;
                currentState = BossState.Chase;
                return;
            }

            rollingPhase = RollingPhase.Charging;
        }
    }

    private void RockFragmentsAttack()
    {
        switch (fragmentsPhase)
        {
            case FragmentsPhase.Jump:
                DoFragments_Jump();
                break;

            case FragmentsPhase.Slam:
                DoFragments_Slam();
                break;

            case FragmentsPhase.Charging:
                DoFragments_Charging();
                break;

            case FragmentsPhase.Rolling:
                DoFragments_Shooting();
                break;
        }
    }
    private void DoFragments_Jump()
    {
        int randomIndex = Random.Range(0, 4);
        jumpTarget = jumpTargetPosArray[randomIndex];

        jumpStartPos = transform.position;

        jumpDir = (jumpTarget - jumpStartPos).normalized;

        jumpLerp = 0;

        if (jumpLerp < 1)
        {
            transform.position = CalculateTrajectory();
            jumpLerp += 3f * Time.deltaTime;
        }
        else
        {
            transform.position = jumpTarget;
            fragmentsPhase = FragmentsPhase.Slam;
        }
    }

    private void DoFragments_Slam()
    {
        if (stateTimer == 0f)
        {
            animator.SetTrigger("Slam");

            agent.isStopped = true;
            rigid.velocity = Vector2.zero;

            hasAttacked = false;
        }

        stateTimer += Time.deltaTime;

        // Will create falling rocks in animation clips . don't forget! --->>> SpawnRocksForFragmentsAttack()

        if (stateTimer >= 2f) // this time(2f) needs to change maching with the animation time! don't forget!
        {
            stateTimer = 0f;
            fragmentsPhase = FragmentsPhase.Charging;
        }
    }

    private void DoFragments_Charging()
    {


    }

    private void DoFragments_Shooting()
    {


    }

    private void Groggy()
    {
        if (stateTimer == 0f)
        {
            agent.isStopped = true;

            rigid.velocity = Vector2.zero;

            animator.SetBool("isGroggy", true);

            hasAttacked = false;
        }

        stateTimer += Time.deltaTime;

        if (stateTimer >= 5f)
        {
            animator.SetBool("isGroggy", false);
            stateTimer = 0f;

            agent.isStopped = false;

            currentState = BossState.Chase;
        }
    }

    private void ChooseBigAttack()
    {
        int choosePattern = Random.Range(0, 2);

        // rolling attack
        if (choosePattern == 0)
        {
            currentState = BossState.RollingAttack;
            rollingPhase = RollingPhase.Slam;
            stateTimer = 0f;
            rollCount = 0;
            isHitRock = false;
            hasAttacked = false;
        }
        // stone fragments attack
        else if (choosePattern == 1)
        {

        }

    }

    private GameObject searchSLG()
    {
        Vector2 currentPos = transform.position;
        int layerMask = 1 << LayerMask.NameToLayer("SLG");

        Collider2D[] cols = Physics2D.OverlapCircleAll(currentPos, 10f, layerMask);

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

        Instantiate(NormalMeleeAttackPrefab, transform.position, Quaternion.identity);
    }

    public void SpawnRangeAttackHitbox()
    {
        // To make sure creating one hitbox prefab for one attack
        if (hasAttacked)
        {
            return;
        }
        hasAttacked = true;

        Vector3 spawnPos = transform.position;
        GameObject proj = Instantiate(RangeAttackPrefab, spawnPos, Quaternion.identity);

        RangeAttackHitbox range = proj.GetComponent<RangeAttackHitbox>();
 
        range.Init(lastLookDir);
    }

    public void SpawnRocksForRollingAttack()
    {
        // To make sure creating one hitbox prefab for one attack
        if (hasAttacked)
        {
            return;
        }
        hasAttacked = true;

        for (int i = 0; i < 10; i++)
        {
            Vector2 CrumblingOffset = Random.insideUnitCircle * 10f; // radius 10
            Vector3 CrumblingSpawnPos = rockCenter.position + new Vector3(CrumblingOffset.x, CrumblingOffset.y, 0f);
            Instantiate(CrumblingRockPrefab, CrumblingSpawnPos, Quaternion.identity);
        }

        Vector2 offset = Random.insideUnitCircle * 10f; // radius 10
        Vector3 spawnPos = rockCenter.position + new Vector3(offset.x, offset.y, 0f);
        Instantiate(RemaningRockPrefab, spawnPos, Quaternion.identity);
    }

    public void SpawnRocksForFragmentsAttack()
    {
        // To make sure creating one hitbox prefab for one attack
        if (hasAttacked)
        {
            return;
        }
        hasAttacked = true;

        for (int i = 0; i < 3; i++)
        {
            Vector2 offset = Random.insideUnitCircle * 10f; // radius 5
            Vector3 spawnPos = rockCenter.position + new Vector3(offset.x, offset.y, 0f);
            Instantiate(RemaningRockPrefab, spawnPos, Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (currentState == BossState.RollingAttack &&
            rollingPhase == RollingPhase.Rolling)
        {
            if (col.collider.gameObject.layer == LayerMask.NameToLayer("BossRock"))
            {
                isHitRock = true;
            }
        }
    }
}
