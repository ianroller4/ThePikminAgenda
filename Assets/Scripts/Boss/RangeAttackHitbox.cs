using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackHitbox : MonoBehaviour
{
    [SerializeField]
    private float damage = 10f;

    [SerializeField]
    private float speed = 10f;

    private Vector2 moveDir;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3f);
    }

    public void Init(Vector2 dir)
    {
        moveDir = dir.normalized;
    }

    private void Update()
    {
        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("SLG"))
        {
            col.gameObject.GetComponent<Health>().TakeDamage(damage);
            Debug.Log(col.name + " get hit by the boss's range attack!");
        }
    }
}
