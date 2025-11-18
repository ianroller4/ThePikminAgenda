using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private GameObject SLG_MANAGER_OBJECT;
    private SLGManager slgManager;

    [SerializeField]
    private GameObject totalSLG;
    [SerializeField]
    private GameObject followingSLG;

    // Start is called before the first frame update
    void Start()
    {
        slgManager = SLG_MANAGER_OBJECT.GetComponent<SLGManager>();
    }

    // Update is called once per frame
    void Update()
    {
        totalSLG.GetComponent<Text>().text = slgManager.SLGCount().ToString();
        followingSLG.GetComponent<Text>().text = slgManager.followingCount().ToString();
    }
}
