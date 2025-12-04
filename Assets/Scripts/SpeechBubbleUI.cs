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
    public void Show4() => ShowLine(4);
    public void Show5() => ShowLine(5);
    public void Show6() => ShowLine(6);
    public void Show7() => ShowLine(7);
    public void Show8() => ShowLine(8);
    public void Show9() => ShowLine(9);
    public void Show10() => ShowLine(10);
    public void Show11() => ShowLine(11);
    public void Show12() => ShowLine(12);
    public void Show13() => ShowLine(13);
}
