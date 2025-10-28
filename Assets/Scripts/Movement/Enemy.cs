using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // --- Component References ---
    private Rigidbody2D rigid;

    // --- Variables ---
    private Vector3 startPos;
    private Vector3 targetPos;
    private float speed;

    // --- Timers ---
    private float idleTimer = 0f;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

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
    }

    private void Idle()
    { 
        float dist = Vector2.Distance(transform.position, startPos);

        // limit the monster's movement range to within 50 units
        if (dist < 50 && idleTimer <= 0f)
        {

            targetPos = transform.position;

            Vector2 randomDir = Random.insideUnitCircle.normalized;

            targetPos = transform.position + (Vector3)(randomDir * 0.5f);

            idleTimer = Random.Range(1f, 2f);
        }
        else if(dist >= 50)
        {
            targetPos = startPos;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 1f);
        idleTimer -= Time.deltaTime;
    }

    private void Sleep()
    {

    }

    private void WakeUp()
    {

    }

    private void Chase()
    {

    }

    private void Attack()
    {

    }

    private void Dead()
    {

    }

}
