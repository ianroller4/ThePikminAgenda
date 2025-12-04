using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orange : MonoBehaviour
{
    private SLGManager slgManager;
    [SerializeField]
    private int capacity = 3;

    // Start is called before the first frame update
    void Start()
    {
        slgManager = FindObjectOfType<SLGManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            slgManager.IncreaseMaxCapacity(capacity);
            Destroy(gameObject);
        }
    }
}
