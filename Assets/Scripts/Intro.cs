using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class Intro : MonoBehaviour
{
    private PlayableDirector director;
    private int wiggleCount = 0;
    private bool isWiggled = false;
    private bool isHold = false;
    private bool isThrew = false;
    private bool isCallback = false;
    [Header("---------Intro Skip---------")]
    [SerializeField]
    private bool introSkip = false;
    [Header("--------References--------")]
    [SerializeField]
    private PlayerMovement player;
    [SerializeField]
    private SillyLittleGuys SLG;
    [SerializeField] 
    private SillyLittleGuys SLGIntroDummy;
    [SerializeField]
    private Sprite playerSpinning;
    [SerializeField]
    private Transform throwPoint;
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private SLGManager slgManager;


    // Start is called before the first frame update
    void Start()
    {
        if (introSkip)
        {
            IntroSkip();
            return;
        }

        director = GetComponent<PlayableDirector>();
        player.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWiggled && Input.GetKeyDown(KeyCode.Space))
        {
            OnWiggleInput();
        }

        if (isHold && Input.GetMouseButton(0))
        {
            OnHoldInput();
        }

        if (isThrew && Input.GetMouseButtonUp(0))
        {
            OnThrowInput();
        }

        if (isCallback && Input.GetMouseButton(1))
        {
            OnCallbackInput();
        }
    }

    public void WigglePauseTimeline()
    {
        director.Pause();
        isWiggled = true;
        Debug.Log("Wiggle Timeline Paused");
    }

    public void HoldPauseTimeline()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        isHold = true;
        player.transform.Find("Cursor").gameObject.SetActive(true);
        Debug.Log("Hold Timeline Paused");
    }

    public void ThrowPauseTimeline()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        isThrew = true;
        Debug.Log("Throw Timeline Paused");
    }

    public void CallbackPauseTimeline()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        isCallback = true;
        player.transform.Find("Cursor").gameObject.SetActive(true);
        Debug.Log("Throw Timeline Paused");
    }

    public void OnWiggleInput()
    {
        wiggleCount++;
        DoWiggle();

        if (wiggleCount >= 10)
        {
            isWiggled = false;
            player.GetComponent<SpriteRenderer>().sprite = playerSpinning;
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

        player.transform.localPosition = new Vector3(0f, 2f, 0f);
        player.transform.localRotation = startRot;
        player.GetComponent<Animator>().enabled = true;
    }

    public void OnHoldInput()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        SLG.GetComponent<SpriteRenderer>().sortingLayerName = "UI";
        isHold = false;
    }

    public void OnThrowInput()
    {
        SLG.GetComponent<SpriteRenderer>().enabled = false;
        SLGIntroDummy.transform.position = SLG.transform.position;
        player.GetComponent<Animator>().Play("PlayerIdleThrowRight");
        SLGIntroDummy.EnterThrownState(throwPoint.position);
        StartCoroutine(ThrowAndResume());
        isThrew = false;
    }

    IEnumerator ThrowAndResume()
    {
        yield return new WaitForSeconds(0.5f);
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        player.transform.Find("Cursor").gameObject.SetActive(false);
        slgManager.RemoveSLG(SLGIntroDummy);
        slgManager.RemoveFollowingSLG(SLGIntroDummy);
        Destroy(SLGIntroDummy.gameObject);
        SLG.GetComponent<SpriteRenderer>().enabled = true;
        SLG.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
    }

    public void OnCallbackInput()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        player.transform.Find("Cursor").gameObject.SetActive(false);
        isCallback = false;
    }

    public void GameStart()
    {
        mainCamera.GetComponent<SimpleCameraFollow>().enabled = true;
        mainCamera.GetComponent<CameraUpgrade>().enabled = true;
        mainCamera.GetComponent<CinemachineBrain>().enabled = false;
        player.transform.Find("Cursor").gameObject.SetActive(true);
        player.GetComponent<Animator>().Play("Idle_BlendTree");
        SLG.GetComponent<Animator>().Play("Idle");
        StartCoroutine(EndIntro());
    }

    IEnumerator EndIntro()
    {
        yield return new WaitForSeconds(2.1f);
        this.gameObject.SetActive(false);
        SLG.EnterFollowState();
        player.GetComponent<PlayerMovement>().enabled = true;
    }

    public void IntroSkip()
    {
        player.transform.Find("Canvas/IntroUI/BlackScreen").gameObject.SetActive(false);
        slgManager.RemoveSLG(SLG);
        slgManager.RemoveFollowingSLG(SLG);
        Destroy(SLG.gameObject);
        mainCamera.GetComponent<SimpleCameraFollow>().enabled = true;
        mainCamera.GetComponent<CameraUpgrade>().enabled = true;
        mainCamera.GetComponent<CinemachineBrain>().enabled = false;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.transform.Find("Cursor").gameObject.SetActive(true);
        player.GetComponent<Animator>().Play("Idle_BlendTree");
        SLG.GetComponent<Animator>().Play("Idle");
        this.gameObject.SetActive(false);
    }
}