using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Intro : MonoBehaviour
{
    private PlayableDirector director;
    private int wiggleCount = 0;
    private bool isPaused = false;
    [SerializeField]
    private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
        player.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused && Input.GetKeyDown(KeyCode.Space))
        {
            OnWiggleInput();
        }
    }

    public void PauseTimeline()
    {
        director.Pause();
        isPaused = true;
        Debug.Log("Timeline Paused");
    }

    public void OnWiggleInput()
    {
        wiggleCount++;
        DoWiggle();

        if (wiggleCount >= 10)
        {
            isPaused = false;
            StartCoroutine(PopUpAndResume());
            Debug.Log("Timeline Resumed");
        }
    }

    public void DoWiggle()
    {
        StartCoroutine(WiggleCoroutine());
    }

    IEnumerator WiggleCoroutine()
    {
        Vector3 original = player.transform.localPosition;

        float xOffset = Random.Range(-0.12f, 0.12f);
        float yOffset = Random.Range(-0.02f, 0.03f);

        Vector3 offset = new Vector3(xOffset, yOffset, 0);

        player.transform.localPosition = original + offset;

        yield return new WaitForSeconds(0.05f);

        player.transform.localPosition = original;
    }

    IEnumerator PopUpAndResume()
    {
        yield return StartCoroutine(PopUpCoroutine());

        director.Resume();
    }

    IEnumerator PopUpCoroutine()
    {
        Vector3 start = player.transform.localPosition;
        Quaternion startRot = player.transform.localRotation;
        Vector3 peak = start + Vector3.up * 0.8f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 3f;
            float height = Mathf.Sin(t * Mathf.PI);
            player.transform.localPosition = Vector3.Lerp(start, peak, height);
            player.transform.localRotation = Quaternion.Euler(0, 0, 180f * t);
            yield return null;
        }

        player.transform.localPosition = new Vector3(0f,2f,0f);
        player.transform.localRotation = startRot;
        player.GetComponent<Animator>().enabled = true;
    }
}
