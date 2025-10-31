using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dismiss : MonoBehaviour
{
    private SLGManager slgManager;

    // Start is called before the first frame update
    private void Start()
    {
        slgManager = GameObject.FindObjectOfType<SLGManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            slgManager.OnDismiss();
        }
    }
}
