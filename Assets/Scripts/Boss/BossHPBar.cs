using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHPBar : MonoBehaviour
{
    private SpriteRenderer sr;
    private float currentHP;
    [SerializeField]
    private Health bossHealth;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHP = bossHealth.GetCurrentHP() / 500;
        transform.localScale = new Vector3(currentHP, 1, 1);

        if(currentHP <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
