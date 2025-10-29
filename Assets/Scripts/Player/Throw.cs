using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    private SLGManager slgManager;

    private SillyLittleGuys heldSLG = null;

    private bool throwing = false;

    private bool canceledThrow = false;

    // Start is called before the first frame update
    void Start()
    {
        slgManager = GameObject.FindObjectOfType<SLGManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !canceledThrow)
        {
            GrabSLG();
            if (Input.GetKeyDown(KeyCode.Q) && heldSLG != null)
            {
                heldSLG.ExitHeldState();
                heldSLG = null;
                canceledThrow = true;
            }
        }
        else if (!Input.GetMouseButton(0))
        {
            canceledThrow = false;
            if (throwing)
            {
                ThrowSLG();
            }
        }
    }

    private void GrabSLG()
    {
        if (heldSLG == null)
        {
            heldSLG = slgManager.GetNextSLGForThrow();
            if (heldSLG != null)
            {
                throwing = true;
                heldSLG.EnterHeldState();
            }
        }
    }

    private void ThrowSLG()
    {
        if (heldSLG != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            heldSLG.EnterThrownState(mousePosition);
            heldSLG = null;
            throwing = false;
        }

    }
}
