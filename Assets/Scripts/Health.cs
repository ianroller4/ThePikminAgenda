using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    /* ---- Object HP ---- */
    [SerializeField]
    private float currentHP = 0f; // current HP
    [SerializeField]
    private float maxHP = 100f; // Maximum HP

    // initialize references and set current HP to max
    void Start()
    {
        currentHP = maxHP;
    }

    public bool TakeDamage(float damage)
    {
        currentHP -= damage;

        // check if HP dropped to zero or below
        if (currentHP <= 0)
        {
            DeathFromDamage(); // process death
            return true;
        }

        Debug.Log(gameObject.name + " got hit. CurrentHP: " + currentHP);
       
        return false;
    }

    private void DeathFromDamage()
    {
        Debug.Log(gameObject.name + "is dead");
    }
}
