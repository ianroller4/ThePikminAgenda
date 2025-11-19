using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeechBubbleSO", menuName = "SLG/SpeechBubbleSO")]
public class SpeechBubbleSO : ScriptableObject
{
    [TextArea]
    public string line;
}
