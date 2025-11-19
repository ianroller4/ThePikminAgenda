using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubbleUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TextMeshProUGUI textUI;
    [SerializeField]
    private SpeechBubbleSO[] speechBubbleSO;

    public void ShowLine(int index)
    {
        panel.SetActive(true);
        textUI.text = speechBubbleSO[index].line;
    }

    public void Show0() => ShowLine(0);
    public void Show1() => ShowLine(1);
    public void Show2() => ShowLine(2);
    public void Show3() => ShowLine(3);
}
