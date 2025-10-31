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
    private Transform player;
    [SerializeField]
    private GameObject target;


    // --- Variables ---
    private Vector3 startPos;
    private Vector3 targetPos;
    private float speed = 1f;
    [SerializeField]
    private float chaseDistance = 1f;

    // --- Timers ---
    private float idleTimer = 0f;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if(player == null)
        {
            Debug.LogWarning("player is NULL");
        }

        rigid = GetComponent<Rigidbody2D>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        startPos = transform.position;

        currentState = EnemyState.Idle;
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

        Debug.Log(currentState);
    }

    private void Idle()
    { 
        float dist = Vector2.Distance(transform.position, startPos);

        // limit the monster's movement range to within 50 units
        if (dist < 50 && idleTimer <= 0f)
        {

            targetPos = transform.position;

            Vector2 randomDir = Random.insideUnitCircle.normalized;

            float randomDist = Random.Range(0.5f, 2f);

            targetPos = transform.position + (Vector3)(randomDir * randomDist);

            idleTimer = Random.Range(1f, 2f);
        }
        else if(dist >= 50)
        {
            targetPos = startPos;
        }

        float randomSpeed = Random.Range(0.5f, 2f);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * randomSpeed);
        idleTimer -= Time.deltaTime;

        // check if the player is closed enough to chase
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= chaseDistance)
        {
            currentState = EnemyState.Chase;
        }

        Debug.Log("Distance to Player: " + distanceToPlayer);
    }

    private void Sleep()
    {

    }

    private void WakeUp()
    {

    }

    private void Chase()
    {
        agent.SetDestination(target.transform.position);
    }

    private void Attack()
    {

    }

    private void Dead()
    {

    }

}
