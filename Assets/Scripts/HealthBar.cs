using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Health health;
    private SpriteRenderer sr;
    private float currentHP;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponentInParent<Health>();
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health.IsHit())
        {
            sr.enabled = true;
            currentHP = health.GetCurrentHP() / 100;
            transform.localScale = new Vector3(currentHP, 1, 1);
        }
        else
        {
            sr.enabled = false;
        }
    }
}
