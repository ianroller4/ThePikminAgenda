using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SacrificeFire : MonoBehaviour
{
    [SerializeField] private GameObject altarOne;
    [SerializeField] private GameObject altarTwo;
    [SerializeField] private GameObject altarThree;
    [SerializeField] private GameObject altarFour;

    [SerializeField] private int altarOneKillRequired = 1;
    [SerializeField] private int altarTwoKillRequired = 2;
    [SerializeField] private int altarThreeKillRequired = 3;
    [SerializeField] private int altarFourKillRequired = 4;

    private int killed = 0;

    private int altarsLit = 0;

    [SerializeField] private GameObject part;

    private void Start()
    {
        part.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject slg = collision.gameObject;
        if (slg != null)
        {
            if (slg.GetComponent<SillyLittleGuys>() != null)
            {
                slg.GetComponent<Health>().KillWithFire();
                killed++;
                UpdateAltars();
                EnablePart();
            }
        }
    }

    private void UpdateAltars()
    {
        // Turn on altar fire animations based on killed
        if (killed == altarOneKillRequired)
        {
            altarOne.GetComponent<Animator>().SetBool("Lit", true);
            altarsLit++;
        }
        if (killed == altarTwoKillRequired)
        {
            altarTwo.GetComponent<Animator>().SetBool("Lit", true);
            altarsLit++;
        }
        if (killed == altarThreeKillRequired)
        {
            altarThree.GetComponent<Animator>().SetBool("Lit", true);
            altarsLit++;
        }
        if (killed == altarFourKillRequired)
        {
            altarFour.GetComponent<Animator>().SetBool("Lit", true);
            altarsLit++;
        }
    }

    private void EnablePart()
    {
        if (altarsLit == 4)
        {
            part.SetActive(true);
        }
    }
}
