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

    private Rigidbody2D rigid;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
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
