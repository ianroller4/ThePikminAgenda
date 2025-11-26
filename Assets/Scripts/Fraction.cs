using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Fraction : MonoBehaviour
{
    [SerializeField] private TextMeshPro numerator;
    [SerializeField] private TextMeshPro denominator;

    public void SetNumerator(string text)
    {
        numerator.text = text;
    }

    public void SetDenominator(string text)
    {
        denominator.text = text;
    }
}
