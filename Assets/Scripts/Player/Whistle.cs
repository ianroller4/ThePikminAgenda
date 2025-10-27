using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whistle : MonoBehaviour
{
    private SLGManager slgManager;

    private bool isWhistling = false;

    // Start is called before the first frame update
    void Start()
    {
        slgManager = GameObject.FindObjectOfType<SLGManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ListenForInput();
        WhistleForSLG();
    }

    private void ListenForInput()
    {
        isWhistling = Input.GetMouseButton(1);
    }

    private void WhistleForSLG()
    {
        if (isWhistling)
        {

        }
    }
}
