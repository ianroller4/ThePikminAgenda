
using UnityEngine;
using UnityEngine.Events;

// A simple timer that will call and event when it finishes. 
public class SimpleTimer : MonoBehaviour
{
    public UnityEvent onTimerComplete = new UnityEvent();

    [Header("Timer End Time")]
    public float maxTime = 1f;

    [Header("Settings to control flow")]
    public bool isRunning = true;
    public bool resetTimer = true;

    [Header("Private values for demonstration")]
    [SerializeField]
    private float currentTime = 0;
    [SerializeField]
    private float percentComplete = 0;

    // Some simple but handy methods. 
    public void ToggleIsRunning()
    {
        isRunning = !isRunning;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void ResetTimer()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            UpdateTimer();
        }
    }

    private void UpdateTimer()
    {
        currentTime = currentTime + Time.deltaTime;
        //currentTime += Time.deltaTime; // same thing

        // update proportion complete. This will be useful for lots of things later. 
        percentComplete = currentTime / maxTime;

        if (currentTime > maxTime)
        {
            onTimerComplete.Invoke();

            if (resetTimer)
            {
                currentTime = currentTime - maxTime;
            }
        }
    }
}
