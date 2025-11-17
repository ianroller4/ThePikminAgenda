using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelathBarUI : MonoBehaviour
{
    private SpriteRenderer sr;
    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        health = GetComponentInParent<Health>();
        sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(health.IsHit())
        {
            sr.enabled = true;
        }
        else
        {
            sr.enabled = false;
        }
    }
}
