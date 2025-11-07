using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField]
    private float damage = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("SLG"))
        {
            col.gameObject.GetComponent<Health>().TakeDamage(damage);
            Debug.Log(col.name + " hit!");
        }
    }
}
