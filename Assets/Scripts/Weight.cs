using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Weight : MonoBehaviour
{
    public int weightNeeded = 5;

    [SerializeField]
    private Fraction fraction;

    private Animator animator;

    private AudioSource audioSource;

    private List<SillyLittleGuys> attackers = new List<SillyLittleGuys>();

    private void Start()
    {
        InitFraction();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void InitFraction()
    {
        fraction.transform.position = transform.position + Vector3.up;
        fraction.SetDenominator(weightNeeded.ToString());
        fraction.SetNumerator("0");
        fraction.gameObject.SetActive(false);
    }

    private void UpdateFraction()
    {
        int count = attackers.Count;
        fraction.transform.position = transform.position + Vector3.up * 2.5f;
        if (count > 0)
        {
            fraction.gameObject.SetActive(true);
            fraction.SetNumerator(count.ToString());
        }
        else
        {
            fraction.SetNumerator("0");
            fraction.gameObject.SetActive(false);
        }
    }

    private void ClearObject()
    {
        audioSource.Play();
        Destroy(gameObject, 1.5f);
    }

    public void RegisterAttacker(SillyLittleGuys slg)
    {
        if (!attackers.Contains(slg))
        {
            attackers.Add(slg);
            UpdateFraction();
        }

        if (attackers.Count >= weightNeeded)
        {
            BreakWall();
        }
    }

    public void UnregisterAttacker(SillyLittleGuys slg)
    {
        if (attackers.Contains(slg))
        {
            attackers.Remove(slg);
            UpdateFraction();
        }
    }

    private void BreakWall()
    {
        animator.SetBool("break", true);
        ClearObject();
    }

}
