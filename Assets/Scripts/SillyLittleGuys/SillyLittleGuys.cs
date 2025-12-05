using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* SillyLittleGuys
 * 
 * Implements the state machine for controlling the Silly LittleGuys or SLGs
 * 
 */
public class SillyLittleGuys : MonoBehaviour
{
    // --- Nav Agent Variables ---
    private NavMeshAgent agent;
    public Vector3 moveToTarget;

    // --- Managers ---
    private SLGManager slgManager;
    private CarryObjectManager coManager;
    private EnemyManager enemyManager;

    // --- Attack Variables ---
    [SerializeField]
    private GameObject AttackHitboxPrefab;

    [SerializeField]
    private float attackRange = 1f;

    private float attackTimer = 0f;
    private bool hasAttacked = false;

    private Vector3 thrownTarget;

    // --- Game Object References ---

    private GameObject player;

    private Animator playerAnimator;

    private CarryableObject carryObject = null;

    private Enemy targetEnemy;

    private SpriteRenderer sr;

    private Boss boss;

    // --- Misc Variables --- 
    public float idleSearchRange = 2;
    private bool isFalling = false;
    private Weight targetWeight;

    public enum States
    {
        IDLE,
        FOLLOW,
        HELD,
        THROWN,
        DISMISS,
        ATTACK,
        ATTACK_BOSS,
        ATTACK_WEIGHT,
        CARRY
    }
    public States state;

    // --- Animator ---
    private Animator animator;
    public Vector3 lastDir = Vector3.zero;
    private Vector3 currentDir = Vector3.zero;
    private Vector3 prevPosition = Vector3.zero;

    // --- Throw ---
    private Vector3 direction = Vector3.zero;
    private Vector3 throwStart;
    public float speed = 3f;
    public float height = 3f;
    private float throwLerp = 0f;

    // --- Audio ---
    private AudioSource audioSource;
    [SerializeField] private AudioClip throwSound;
    [SerializeField] private AudioClip[] randomSounds;
    private float soundTimer = 0f;
    [SerializeField] private float soundTimerMax = 4f;
    [SerializeField] private float soundChance = 0.05f;

    // --- Shadow ---
    [SerializeField] private GameObject shadow;

    // --- Particles ---
    [SerializeField] private GameObject throwParticles;
    [SerializeField] private GameObject idleParticles;

    /* Start
     * 
     * Called once before the first frame of update
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.enabled = true;
        EnterIdleState();

        slgManager = GameObject.FindObjectOfType<SLGManager>();
        coManager = GameObject.FindObjectOfType<CarryObjectManager>();
        enemyManager = GameObject.FindObjectOfType<EnemyManager>();
        boss = GameObject.FindObjectOfType<Boss>();

        slgManager.AddSLG(this);
        moveToTarget = transform.position;
        player = GameObject.Find("Player");
        playerAnimator = player.GetComponent<Animator>();

        prevPosition = transform.position;

        audioSource = GetComponent<AudioSource>();

        throwParticles.SetActive(false);
        idleParticles.SetActive(false);
    }

    /* Update
     * 
     * Called once per frame
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void Update()
    {
        switch (state)
        {
            case States.IDLE:
                UpdateIdleState();
                RandomSoundTimer();
                break;
            case States.FOLLOW:
                UpdateFollowState();
                RandomSoundTimer();
                break;
            case States.HELD:
                UpdateHeldState();
                RandomSoundTimer();
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
            case States.ATTACK_BOSS:
                UpdateBossAttackState();
                break;
            case States.ATTACK_WEIGHT:
                UpdateAttackWeightState();
                break;
            case States.CARRY:
                UpdateCarryState();
                RandomSoundTimer();
                break;
        }
        if (state == States.IDLE && !isFalling)
        {
            idleParticles.SetActive(true);
        } 
        else
        {
            idleParticles.SetActive(false);
        }

        Debug.Log("SLG State : " + state);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;   
        audioSource.Play();
    }

    private void PlayRandomSound()
    {
        int index = (int)Random.Range(0, randomSounds.Length - 1);
        audioSource.clip = randomSounds[index];
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    private void RandomSoundTimer()
    {
        soundTimer += Time.deltaTime;
        if (soundTimer > soundTimerMax)
        {
            soundTimer -= soundTimerMax;
            if (Random.Range(0f, 1f) < soundChance)
            {
                PlayRandomSound();
            }
        }
    }

    public States GetState()
    {
        return state;
    }

    // --- IDLE State --- 

    /* EnterIdleState
     * 
     * Switches to idle state and updates animation variables
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void EnterIdleState()
    {
        state = States.IDLE;
        animator.SetBool("idle", true);
        animator.SetBool("attack", false);
        animator.SetBool("held", false);
        animator.SetFloat("x", lastDir.x);
        animator.SetFloat("y", lastDir.y);
    }

    /* UpdateIdleState
     * 
     * Checks for something to attack or carry
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
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

        if (boss != null)
        {
            if (Vector3.Distance(transform.position, boss.transform.position) < 6f)
            {
                EnterBossAttackState();
            }
        }

    }

    // --- FOLLOW State --- 

    /* EnterFollowState
     * 
     * Switches to follow state, adds to following, and disables attack animation
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void EnterFollowState()
    {
        state = States.FOLLOW;
        slgManager.AddFollowingSLG(this);
        animator.SetBool("attack", false);
    }

    /* UpdateFollowState
     * 
     * Calls agent to move to new target position and updates animation
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void UpdateFollowState()
    {
        agent.SetDestination(moveToTarget);
        bool result = UpdateDirVector();
        if (result) // If SLG has moved
        {
            animator.SetBool("idle", false);
            animator.SetFloat("x", currentDir.x);
            animator.SetFloat("y", currentDir.y);
        }
        else // If SLG has not moved
        {
            animator.SetBool("idle", true);
            animator.SetFloat("x", lastDir.x);
            animator.SetFloat("y", lastDir.y);
        }
    }

    // --- HELD State --- 

    /* EnterHeldState
     * 
     * Switches to held state, remove from following, and disables collider and agent
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void EnterHeldState()
    {
        state = States.HELD;
        slgManager.RemoveFollowingSLG(this);
        GetComponent<Collider2D>().enabled = false;
        agent.enabled = false;

        animator.SetBool("held", true);
    }

    /* UpdateHeldState
     * 
     * Update the position to be next to the player
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void UpdateHeldState()
    {
        float dirX = playerAnimator.GetFloat("x");
        float dirY = playerAnimator.GetFloat("y");

        Vector3 offset = Vector3.zero;
        Vector2 animDir = Vector2.down;

        if (Mathf.Abs(dirX) >= Mathf.Abs(dirY))
        {
            if (dirX >= 0f)
            {
                offset = new Vector3(-0.25f, 0.2f, 0f);
                animDir = Vector2.right;
                sr.sortingLayerName = "AboveDefault";
            }
            else
            {
                offset = new Vector3(0.25f, 0.2f, 0f);
                animDir = Vector2.left;
                sr.sortingLayerName = "AboveDefault";
            }
        }
        else
        {
            if (dirY >= 0f)
            {
                offset = new Vector3(-0.26f, 0.2f, 0f);
                animDir = Vector2.up;
                sr.sortingLayerName = "Default";
            }
            else
            {
                offset = new Vector3(0.27f, 0.2f, 0f);
                animDir = Vector2.down;
                sr.sortingLayerName = "AboveDefault";
            }
        }

        transform.position = player.transform.position + offset;
        animator.SetFloat("x", animDir.x);
        animator.SetFloat("y", animDir.y);

        lastDir = animDir;
    }

    /* ExitHeldState
     * 
     * Cleans up held state and switches to follow 
     * Called when throw is cancelled
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void ExitHeldState()
    {
        state = States.FOLLOW;
        GetComponent<Collider2D>().enabled = true;
        agent.enabled = true;
        slgManager.AddFollowingSLG(this);

        sr.sortingLayerName = "Default";

        animator.SetBool("held", false);
    }

    // --- THROWN State --- 

    /* EnterThrownState
     * 
     * Switches to thrown state and sets it target to the passed target
     * 
     * Parameters: Vector3 target, where the SLG is thrown to
     * 
     * Return: None
     * 
     */
    public void EnterThrownState(Vector3 target)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(target, out hit, 10f, NavMesh.AllAreas))
        {
            target = hit.position;
        }

        state = States.THROWN;
        throwStart = transform.position;
        thrownTarget = target;
        direction = (thrownTarget - transform.position).normalized;
        throwLerp = 0;
        audioSource.Stop();
        PlaySound(throwSound);
        throwParticles.SetActive(true);
    }

    /* UpdateThrownState
     * 
     * Update thrown state. 
     * Currently sets position to thrownTarget, will later have actual throw arc
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void UpdateThrownState()
    {
        if (throwLerp < 1)
        {
            transform.position = CalculateTrajectory();
            shadow.transform.position = Vector3.Lerp(throwStart, thrownTarget, throwLerp); 
            throwLerp += speed * Time.deltaTime;
        }
        else
        {
            transform.position = thrownTarget;
            shadow.transform.position = transform.position + new Vector3(0, -0.3f, 0);
            ExitThrownState();
        }
    }

    private Vector3 CalculateTrajectory()
    {
        Vector3 linearProgress = Vector3.Lerp(throwStart, thrownTarget, throwLerp);
        float offset = Mathf.Sin(throwLerp * Mathf.PI) * height;

        return linearProgress + (Vector3.up * offset);
    }

    /* ExitThrownState
     * 
     * Cleans up thrown state and switches to idle state
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void ExitThrownState()
    {
        GetComponent<Collider2D>().enabled = true;
        agent.enabled = true;

        throwParticles.SetActive(false);
        animator.SetBool("held", false);
        sr.sortingLayerName = "Default";

        Collider2D myCol = GetComponent<Collider2D>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 2f, LayerMask.GetMask("Weight"));

        foreach (var h in hits)
        {
            var weight = h.GetComponent<Weight>();
            if (weight != null)
            {
                float surfaceDist = myCol.Distance(h).distance;

                if (surfaceDist <= 2f)
                {
                    EnterAttackWeightState(weight);
                    return;
                }
            }
        }



        EnterIdleState();
    }

    public void EnterAttackWeightState(Weight weight)
    {
        targetWeight = weight;
        state = States.ATTACK_WEIGHT;
        agent.isStopped = false;
        animator.SetBool("attack", false);
    }

    private void UpdateAttackWeightState()
    {
        if (targetWeight == null)
        {
            agent.isStopped = false;
            agent.ResetPath();
            animator.SetBool("attack", false);
            state = States.IDLE;
            return;
        }

        Collider2D myCol = GetComponent<Collider2D>();
        Collider2D weightCol = targetWeight.GetComponent<Collider2D>();

        float surfaceDist = myCol.Distance(weightCol).distance;

        Vector3 dirToWeight = (targetWeight.transform.position - transform.position).normalized;
        animator.SetFloat("x", dirToWeight.x);
        animator.SetFloat("y", dirToWeight.y);

        if (surfaceDist > 2f)
        {
            targetWeight.UnregisterAttacker(this);
            agent.isStopped = false;
            animator.SetBool("attack", false);
            state = States.IDLE;
            return;
        }

        if (surfaceDist <= 1f)
        {
            agent.isStopped = true;
            animator.SetBool("attack", true);
            targetWeight.RegisterAttacker(this);
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(targetWeight.transform.position);

            targetWeight.UnregisterAttacker(this);
            animator.SetBool("attack", false);
        }
    }

    // --- DISMISS State ---

    /* EnterDismissState
     * 
     * Switches to the dismiss state and sets its new moveToTarget
     * 
     * Parameters: Vector3 dismissSpot, where the SLG will go to when dismissed
     * 
     * Return: None
     * 
     */
    public void EnterDismissState(Vector3 dismissSpot)
    {
        state = States.DISMISS;
        moveToTarget = dismissSpot;
        agent.SetDestination(moveToTarget);
        animator.SetBool("idle", false);
    }

    /* UpdateDismissState
     * 
     * Updates the dismiss state and its animations
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void UpdateDismissState()
    {
        // If close enough to target
        if (Vector3.Distance(moveToTarget, transform.position) < 0.2)
        {
            Debug.Log("Ended dismiss");
            // Exit dismiss
            ExitDismissState();
            animator.SetBool("idle", true);
        }
        else
        {
            // Update movement direction for animation
            UpdateDirVector();
            animator.SetFloat("x", currentDir.x);
            animator.SetFloat("y", currentDir.y);
        }
    }

    /* ExitDismissState
     * 
     * Cleans up dismiss state and switches to idle
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void ExitDismissState()
    {
        EnterIdleState();
    }

    // --- ATTACK State --- 

    /* EnterAttackState
     * 
     * Switches to attack state and cleans up from follow state
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void EnterAttackState()
    {
        state = States.ATTACK;
        slgManager.RemoveFollowingSLG(this);
        animator.SetBool("idle", false);
        hasAttacked = false;
        attackTimer = 0f;
    }

    /* UpdateAttackState
     * 
     * Updates the SLG attack state operations, check if close enough to target and attack
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void UpdateAttackState()
    {
        // If there's a target
        if (targetEnemy != null)
        {
            // If there a enemy within SLG's attack range
            if (Vector3.Distance(transform.position, targetEnemy.transform.position) <= attackRange)
            {
                // Animation update
                animator.SetBool("attack", true);
                Vector3 attackDir = targetEnemy.transform.position - transform.position;
                animator.SetFloat("x", attackDir.x);
                animator.SetFloat("y", attackDir.y);

                // Process attack
                agent.isStopped = true; // Stop moving
                attackTimer += Time.deltaTime;
                if (attackTimer >= 1f)
                {
                    attackTimer = 0f;
                    hasAttacked = false;
                }
            }
            else // If an enemy is not close enough
            {
                animator.SetBool("attack", false);
                UpdateDirVector();
                animator.SetFloat("x", currentDir.x);
                animator.SetFloat("y", currentDir.y);

                // Keep chashing
                agent.isStopped = false;
                agent.SetDestination(targetEnemy.transform.position);
                attackTimer = 0f;
                hasAttacked = false;
            }
            
        }
        // If there's no target, switch to IdleState
        else
        {
            agent.SetDestination(transform.position);
            agent.isStopped = false;
            attackTimer = 0f;
            hasAttacked = false;
            ExitAttackState();
            Debug.Log("Exiting Attack");
        }
    }

    /* ExitAttackState
     * 
     * Cleans up attack state and switches to idle state
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void ExitAttackState()
    {
        state = States.IDLE;
        EnterIdleState();
    }

    public void EnterBossAttackState()
    {
        state = States.ATTACK_BOSS;
        slgManager.RemoveFollowingSLG(this);
        animator.SetBool("idle", false);
        hasAttacked = false;
        attackTimer = 0f;
    }

    public void UpdateBossAttackState()
    {
        if (boss != null)
        {
            // If there a enemy within SLG's attack range
            if (Vector3.Distance(transform.position, boss.transform.position) <= 2.3f)
            {
                // Animation update
                animator.SetBool("attack", true);
                Vector3 attackDir = boss.transform.position - transform.position;
                animator.SetFloat("x", attackDir.x);
                animator.SetFloat("y", attackDir.y);

                // Process attack
                agent.isStopped = true; // Stop moving
                attackTimer += Time.deltaTime;
                if (attackTimer >= 1f)
                {
                    attackTimer = 0f;
                    hasAttacked = false;
                }
            }
            else // If an enemy is not close enough
            {
                animator.SetBool("attack", false);
                UpdateDirVector();
                animator.SetFloat("x", currentDir.x);
                animator.SetFloat("y", currentDir.y);

                // Keep chashing
                agent.isStopped = false;
                agent.SetDestination(boss.transform.position);
                attackTimer = 0f;
                hasAttacked = false;
            }

        }
        // If there's no target, switch to IdleState
        else
        {
            agent.SetDestination(transform.position);
            agent.isStopped = false;
            attackTimer = 0f;
            hasAttacked = false;
            ExitAttackState();
            Debug.Log("Exiting Attack");
        }
    }

    // --- CARRY State --- 

    /* EnterCarryState
     * 
     * Switches SLG to carry state
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void EnterCarryState()
    {
        state = States.CARRY;
    }

    /* UpdateCarryState
     * 
     * Updates the nav agent to its new target
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void UpdateCarryState()
    {
        Vector3 carryDir = carryObject.transform.position - transform.position;
        animator.SetFloat("x", carryDir.x);
        animator.SetFloat("y", carryDir.y);
        agent.SetDestination(moveToTarget);
    }

    /* ExitCarryState
     * 
     * Cleans up carry state and switches to idle state
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void ExitCarryState()
    {
        state = States.IDLE;
    }

    // --- Command Calls ---

    /* OnWhistleCall
     * 
     * Switches the SLG to follow state and cleans up anything it was doing
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void OnWhistleCall()
    {
        if (targetWeight != null)
        {
            targetWeight.UnregisterAttacker(this);
            targetWeight = null;
        }

        // Clean up attack if SLG was attacking
        if (state == States.ATTACK || state == States.ATTACK_BOSS || state == States.ATTACK_WEIGHT)
        {
            agent.isStopped = false;
        }

        // Clean up carry object if SLG was carrying
        if (carryObject != null)
        {
            carryObject.RemoveCarrier(this);
            carryObject = null;
        }
        if (state == States.THROWN || isFalling)
        {
            return;
        }
        EnterFollowState();
    }
    
    // --- Animation Vector ---

    /* UpdateDirVector
     * 
     * Updates the current movement direction, last direction, and previous position
     * 
     * Parameters: None
     * 
     * Return: bool result, false if SLG did not move, true if moved
     * 
     */
    private bool UpdateDirVector()
    {
        bool result = false;
        // Check that SLG has moved
        if (transform.position.x != prevPosition.x || transform.position.y != prevPosition.y)
        {
            // The direction the SLG just moved in
            currentDir = (transform.position - prevPosition).normalized;
            // Update last direction to what they just moved int
            lastDir = currentDir;
            // Previous position becomes current position
            prevPosition = transform.position;
            result = true;
        }
        return result;
    }

    /* SpawnAttackHitbox
     * 
     * Create attack hitbox prefab on enemy position in sync with the attack timing.
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void SpawnAttackHitbox()
    {
        if (targetEnemy == null && boss == null)
            return;

        // To make sure creating one hitbox prefab for one attack
        if (hasAttacked)
        {
            return;
        }

        hasAttacked = true;

        if (state == States.ATTACK)
        {
            // Create attack hitbox prefab on enemy position
            Instantiate(AttackHitboxPrefab, targetEnemy.transform.position, Quaternion.identity);
        }
        else if(state == States.ATTACK_BOSS)
        {
            Instantiate(AttackHitboxPrefab, boss.transform.position, Quaternion.identity);
        }
    }

    public void BornStart()
    {
        isFalling = true;
        Debug.Log("BornStart!");
    }

    public void BornEnd()
    {
        sr.sortingLayerName = "Default";
        isFalling = false;

        Debug.Log("BornEND!");
    }
}
