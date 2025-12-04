using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingWarningSign : MonoBehaviour
{
    public Transform boss;
    public Transform target;
    public float warningLength = 15f;

    public float lifeTime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    private void Update()
    {
        if (boss == null || target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = boss.position;

        Vector2 dir = (target.position - boss.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        transform.localScale = new Vector3(warningLength, 1f, 1f);
    }
}
