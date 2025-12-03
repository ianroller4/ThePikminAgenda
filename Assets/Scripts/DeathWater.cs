using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathWater : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject slg = collision.gameObject;
        if (slg != null)
        {
            if (slg.GetComponent<SillyLittleGuys>() != null)
            {
                slg.GetComponent<Health>().KillWithWater();
            }
        }
    }
}
