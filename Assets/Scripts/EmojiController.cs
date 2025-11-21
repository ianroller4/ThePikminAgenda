using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmojiController : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField]
    private Sprite surprised;
    [SerializeField]
    private Sprite question;
    [SerializeField]
    private Sprite angry;
    [SerializeField]
    private Sprite gross;
    [SerializeField]
    private Sprite excited;
    [SerializeField]
    private Sprite devil;
    [SerializeField]
    private Sprite unsure;
    [SerializeField]
    private Sprite happy;

    public void ShowAngry()
    {
        sr.sprite = angry;
    }

    public void ShowQuestion()
    {
        sr.sprite = question;
    }

    public void ShowSurprised()
    {
        sr.sprite = surprised;
    }

    public void ShowGross()
    {
        sr.sprite = gross;
    }

    public void ShowExcited()
    {
        sr.sprite = excited;
    }

    public void ShowDevil()
    {
        sr.sprite = devil;
    }

    public void ShowUnsure()
    {
        sr.sprite = unsure;
    }

    public void ShowHappy()
    {
        sr.sprite = happy;
    }
}