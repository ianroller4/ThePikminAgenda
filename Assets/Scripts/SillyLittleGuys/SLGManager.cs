using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* SLGManager
 * 
 * Manages the SLGs
 * 
 */
public class SLGManager : MonoBehaviour
{
    // --- SLG Lists ---
    public List<SillyLittleGuys> SLGList;
    public List<SillyLittleGuys> followingSLG;

    // --- Follow Variables ---
    public GameObject slgFollowPoint;
    public List<Vector3> followPositions; // Positions in follow group
    public float[] followRingRadii; // Radii of possible rings to fill
    public int[] followPositionsInRings; // Number of positions in each ring

    // --- Dismiss Variables ---
    public List<Vector3> dismissPositions; // Positions in dismiss group
    public float[] dismissRingRadii; // Radii of possible rings to fill
    public int[] dismissPositionsInRings; // Number of positions in each ring

    // --- Player ---
    private GameObject player;

    // --- Grab Distance ---
    public float grabDistance = 3f;

    /* Awake
     * 
     * Called once when script is loaded in
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void Awake()
    {
        // Initialize lists
        SLGList = new List<SillyLittleGuys>();
        followingSLG = new List<SillyLittleGuys>();
        followPositions = new List<Vector3>();
        dismissPositions = new List<Vector3>();

        // Find and get player object
        player = GameObject.Find("Player");
    }

    /* Update
     * 
     * Called once per frame
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    private void Update()
    {
        if (followingSLG.Count > 0)
        {
            AssignFollowPositions();
        }   
    }

    /* AddSLG
     * 
     * Adds the passed SLG to the SLG master list
     * 
     * Parameters: SillyLittleGuys slg, SLG to add to list
     * 
     * Return: None
     * 
     */
    public void AddSLG(SillyLittleGuys slg)
    {
        if (!SLGList.Contains(slg))
        {
            SLGList.Add(slg);
        }
    }

    /* RemoveSLG
     * 
     * Remove the passed SLG from the SLG master list
     * 
     * Parameters: SillyLittleGuys slg, SLG to remove from list
     * 
     * Return: None
     * 
     */
    public void RemoveSLG(SillyLittleGuys slg)
    {
        if (SLGList.Contains(slg))
        {
            SLGList.Remove(slg);
        }
    }

    /* AddFollowingSLG
     * 
     * Adds the passed SLG to the SLG following list
     * 
     * Parameters: SillyLittleGuys slg, SLG to add to list
     * 
     * Return: None
     * 
     */
    public void AddFollowingSLG(SillyLittleGuys slg)
    {
        if (!followingSLG.Contains(slg))
        {
            followingSLG.Add(slg);
        }
    }

    /* RemoveFollowingSLG
     * 
     * Removes the passed SLG from the SLG following list
     * 
     * Parameters: SillyLittleGuys slg, SLG to remove from list
     * 
     * Return: None
     * 
     */
    public void RemoveFollowingSLG(SillyLittleGuys slg)
    {
        if (followingSLG.Contains(slg))
        {
            followingSLG.Remove(slg);
        }
    }

    /* AssignFollowPositions
     * 
     * Assigns positions from position list to SLGs following player starting with the center and moving outwards
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void AssignFollowPositions()
    {
        BuildPositionsList(slgFollowPoint.transform.position, followRingRadii, followPositionsInRings);
        for (int i = 0; i < followingSLG.Count; i++)
        {
            followingSLG[i].moveToTarget = followPositions[i];
        }
    }

    /* BuildPositionsList
     * 
     * Builds the positions list for following based around center with radii and positions in rings
     * 
     * Parameters: Vector3 center, center of dismiss area
     *             float[] ringRadii, the radii of each ring
     *             int[] positionsInRings, the number of positions in each ring
     * 
     * Return: None
     * 
     */
    private void BuildPositionsList(Vector3 center, float[] ringRadii, int[] positionsInRings)
    {
        followPositions.Clear();

        followPositions.Add(center);
        for (int i = 0; i < ringRadii.Length; i++)
        {
            followPositions.AddRange(GetPositionsInRing(center, ringRadii[i], positionsInRings[i]));
        }
    }

    /* BuildDismissPositionsList
     * 
     * Builds the dismiss positions list based around center with radii and positions in rings
     * 
     * Parameters: Vector3 center, center of dismiss area
     *             float[] ringRadii, the radii of each ring
     *             int[] positionsInRings, the number of positions in each ring
     * 
     * Return: None
     * 
     */
    public void BuildDismissPositionsList(Vector3 center, float[] ringRadii, int[] positionsInRings)
    {
        dismissPositions.Clear();

        dismissPositions.Add(center);
        for (int i = 0; i < ringRadii.Length; i++)
        {
            dismissPositions.AddRange(GetPositionsInRing(center, ringRadii[i], positionsInRings[i]));
        }
    }

    /* GetPositionsInRing
     * 
     * Builds x positions in a ring of radius r and adds them to positions vector, returns position vector
     * 
     * Parameters: Vector3 center, the center of the ring
     *             float ringRadius, the radius of the ring (distance from center)
     *             int positionsInRing, how many positions to create around the ring
     *             
     * Return: List<Vector3> positions, the positions in the ring
     * 
     */
    private List<Vector3> GetPositionsInRing(Vector3 center, float ringRadius, int positionsInRing)
    {
        List<Vector3> positions = new List<Vector3>();

        // Variables used in positions
        float angle;
        Vector3 dir;
        Vector3 position;

        for (int i = 0; i < positionsInRing; i++)
        {
            angle = i * (360f / positionsInRing); // Get angle of position
            dir = ApplyRotationToVector(Vector2.right, angle); // Apply rotation
            position = center + dir * ringRadius; // Create position
            positions.Add(position); // Add position
        }

        return positions;
    }

    /* ApplyRotationToVector
     * 
     * Rotates a vector v by angle
     * 
     * Parameters: Vector3 v, vector to rotate
     *             float angle, angle to rotate v to 
     * 
     * Return: Vector3, the rotated vector
     * 
     */
    private Vector3 ApplyRotationToVector(Vector3 v, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * v;
    }

    /* GetNextSLGForThrow
     * 
     * Gets the first SLG in the following list if following list is not empty
     * 
     * Parameters: None
     * 
     * Return: SillyLittleGuys slg, an SLG in the following list
     * 
     */
    public SillyLittleGuys GetNextSLGForThrow()
    {
        SillyLittleGuys slg = null;

        // Grab SLG
        for (int i = 0; i < followingSLG.Count; i++)
        {
            if (Vector3.Distance(followingSLG[i].transform.position, player.transform.position) <= grabDistance)
            {
                slg = followingSLG[i];
                break;
            }
        }
        

        return slg;
    }

    /* OnDimiss
     * 
     * Builds the dismiss positions and sends each following SLG its own dismiss position
     * Clears following list
     * 
     * Parameters: None
     * 
     * Return: None
     * 
     */
    public void OnDismiss()
    {
        // Get center of dismiss area randomly around player 
        Vector3 center = player.transform.position - ApplyRotationToVector(Vector2.right, Random.Range(0, 360)) * 2;
        // Build positions around center
        BuildDismissPositionsList(center, dismissRingRadii, dismissPositionsInRings);
        // Loop through following SLGs and send to dismiss state
        for (int i = 0; i < followingSLG.Count; i++)
        {
            followingSLG[i].EnterDismissState(dismissPositions[i]);
        }
        // Clear list of following
        followingSLG.Clear();
    }

    public int SLGCount()
    {
        return SLGList.Count;
    }

    public int followingCount()
    {
        return followingSLG.Count;
    }
}
