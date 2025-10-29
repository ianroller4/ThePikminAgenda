using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SLGManager : MonoBehaviour
{

    public List<SillyLittleGuys> SLGList;
    public List<SillyLittleGuys> followingSLG;

    public GameObject slgFollowPoint;

    public List<Vector3> followPositions;

    public float[] followRingRadii;
    public int[] followPositionsInRings;

    public List<Vector3> dismissPositions;

    public float[] dismissRingRadii;
    public int[] dismissPositionsInRings;

    private GameObject player;

    private void Awake()
    {
        SLGList = new List<SillyLittleGuys>();
        followingSLG = new List<SillyLittleGuys>();
        followPositions = new List<Vector3>();
        dismissPositions = new List<Vector3>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (followingSLG.Count > 0)
        {
            AssignFollowPositions();
        }   
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

    public void AddFollowingSLG(SillyLittleGuys slg)
    {
        if (!followingSLG.Contains(slg))
        {
            followingSLG.Add(slg);
            Debug.Log(followingSLG.Count);
        }
    }

    public void RemoveFollowingSLG(SillyLittleGuys slg)
    {
        if (followingSLG.Contains(slg))
        {
            followingSLG.Remove(slg);
        }
    }

    public void AssignFollowPositions()
    {
        BuildPositionsList(slgFollowPoint.transform.position, followRingRadii, followPositionsInRings);
        for (int i = 0; i < followingSLG.Count; i++)
        {
            followingSLG[i].moveToTarget = followPositions[i];
        }
    }

    private void BuildPositionsList(Vector3 center, float[] ringRadii, int[] positionsInRings)
    {
        followPositions.Clear();

        followPositions.Add(center);
        for (int i = 0; i < ringRadii.Length; i++)
        {
            followPositions.AddRange(GetPositionsInRing(center, ringRadii[i], positionsInRings[i]));
        }
    }

    public void BuildDismissPositionsList(Vector3 center, float[] ringRadii, int[] positionsInRings)
    {
        dismissPositions.Clear();

        dismissPositions.Add(center);
        for (int i = 0; i < ringRadii.Length; i++)
        {
            dismissPositions.AddRange(GetPositionsInRing(center, ringRadii[i], positionsInRings[i]));
        }
    }

    private List<Vector3> GetPositionsInRing(Vector3 center, float ringRadius, int positionsInRing)
    {
        List<Vector3> positions = new List<Vector3>();

        // Variables used in positions
        float angle;
        Vector3 dir;
        Vector3 position;

        for (int i = 0; i < positionsInRing; i++)
        {
            angle = i * (360f / positionsInRing);
            dir = ApplyRotationToVector(Vector2.right, angle);
            position = center + dir * ringRadius;
            positions.Add(position);
        }

        return positions;
    }

    private Vector3 ApplyRotationToVector(Vector3 v, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * v;
    }

    public SillyLittleGuys GetNextSLGForThrow()
    {
        SillyLittleGuys slg = null;

        if (followingSLG.Count != 0)
        {
            slg = followingSLG[0];
        } 

        return slg;
    }

    public void OnDismiss()
    {
        Vector3 center = player.transform.position - ApplyRotationToVector(Vector2.right, Random.Range(0, 360)) * 2;
        BuildDismissPositionsList(center, dismissRingRadii, dismissPositionsInRings);
        for (int i = 0; i < followingSLG.Count; i++)
        {
            followingSLG[i].EnterDismissState(dismissPositions[i]);
        }
        followingSLG.Clear();
        Debug.Log(followingSLG.Count);
    }

    public void SendAllCommand()
    {
        foreach (SillyLittleGuys slg in SLGList)
        {
            // Send command
        }
    }
}
