using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLGAttackHitbox : MonoBehaviour
{
    [SerializeField]
    private float damage = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            col.gameObject.GetComponent<Health>().TakeDamage(damage);
            Debug.Log(col.name + " hit!");
            Destroy(gameObject);
        }
    }
}
