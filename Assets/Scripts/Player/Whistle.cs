using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whistle : MonoBehaviour
{
    private SLGManager slgManager;

    private bool isWhistling = false;

    public float whistleRadiusStart = 0.5f;
    public float whistleRadiusMax = 2.5f;
    private float currentWhistleRadius;

    // Start is called before the first frame update
    private void Start()
    {
        slgManager = GameObject.FindObjectOfType<SLGManager>();
        currentWhistleRadius = whistleRadiusStart;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdatePosition();
        ListenForInput();
        WhistleForSLG();
    }

    private void UpdatePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.position = mousePosition;
        
    }

    private void ListenForInput()
    {
        isWhistling = Input.GetMouseButton(1);
    }

    private void WhistleForSLG()
    {
        if (isWhistling)
        {
            currentWhistleRadius += Time.deltaTime;
            if (currentWhistleRadius > whistleRadiusMax)
            {
                currentWhistleRadius = whistleRadiusMax;
            }
            transform.localScale = new Vector3(currentWhistleRadius * 2, currentWhistleRadius * 2, 1);
            foreach (SillyLittleGuys slg in slgManager.SLGList)
            {
                if (Vector2.Distance(slg.transform.position, transform.position) < currentWhistleRadius)
                {
                    slg.OnWhistleCall();
                }
            }
        }
        else
        {
            currentWhistleRadius = whistleRadiusStart;
            transform.localScale = new Vector3(currentWhistleRadius * 2, currentWhistleRadius * 2, 1);
        }
    }
}
