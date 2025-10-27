using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLGManager : MonoBehaviour
{

    public List<SillyLittleGuys> SLGList;

    // Start is called before the first frame update
    void Start()
    {
        SLGList = new List<SillyLittleGuys>();    
    }

    public void AddSLG(SillyLittleGuys slg)
    {
        if (!SLGList.Contains(slg))
        {
            SLGList.Add(slg);
        }
    }

    public void RemoveSLG(SillyLittleGuys slg)
    {
        if (SLGList.Contains(slg))
        {
            SLGList.Remove(slg);
        }
    }

    public SillyLittleGuys GetNextSLG()
    {
        SillyLittleGuys slg = null;

        if (SLGList.Count != 0)
        {
            slg = SLGList[0];
        } 

        return slg;
    }

    public void SendAllCommand()
    {
        foreach (SillyLittleGuys slg in SLGList)
        {
            // Send command
        }
    }
}
