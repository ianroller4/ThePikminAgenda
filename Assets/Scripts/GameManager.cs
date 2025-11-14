using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Playing,
        Paused,
    }

    private GameState currentState = GameState.Playing;

    [SerializeField]
    private GameObject controlsPanel;
    [SerializeField]
    private GameObject escForControls;
    private bool isOpen = false;
    private float timer = 0f;
    private bool isDone = false;

    // Start is called before the first frame update
    void Start()
    {
        currentState = GameState.Playing;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen)
            {
                isOpen = !isOpen;
                controlsPanel.SetActive(false);
            }
            else
            {
                if (currentState == GameState.Playing)
                {
                    PauseGame();
                }
                else if (currentState == GameState.Paused)
                {
                    ResumeGame();
                }
            }
        }

        if (!isDone)
        {
            timer += Time.deltaTime;

            if (timer >= 3f)
            {
                escForControls.SetActive(false);
                isDone = true;
            }
        }
    }

    public void PauseGame()
    {
        currentState = GameState.Paused;
        Time.timeScale = 0f;
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(true);
        }
        Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        controlsPanel.SetActive(false);
        Debug.Log("Game Resumed");
    }
}
