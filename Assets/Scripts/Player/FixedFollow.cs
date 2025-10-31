using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFollow : MonoBehaviour
{
    public float followDistance = 2f;

    private Transform parentTransform;
    private Rigidbody2D parentRB;

    private Vector3 distanceBehindPlayer;

    // Start is called before the first frame update
    void Start()
    {
        parentTransform = transform.parent;
        parentRB = parentTransform.gameObject.GetComponent<Rigidbody2D>();
        transform.position = parentTransform.position - Vector3.up * followDistance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDistanceBehindPlayer();
    }

    private void UpdateDistanceBehindPlayer()
    {
        if (parentRB != null)
        {
            if (parentRB.velocity != Vector2.zero)
            {
                distanceBehindPlayer = new Vector3(parentRB.velocity.normalized.x, parentRB.velocity.normalized.y, 0);
                transform.position = parentTransform.position - distanceBehindPlayer * followDistance;
            }
        }
    }

}
