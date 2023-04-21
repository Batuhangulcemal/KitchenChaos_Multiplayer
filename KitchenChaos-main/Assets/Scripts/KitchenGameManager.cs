using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State state;

    private float coundownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 10f;
    private bool isGamePaused = false;


    private void Awake()
    {
        Instance = this;
        SetState(State.WaitingToStart);
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state == State.WaitingToStart)
        {
            SetState(State.CountdownToStart);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }



    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:

                break;
            case State.CountdownToStart:
                coundownToStartTimer -= Time.deltaTime;
                if (coundownToStartTimer < 0f)
                {
                    gamePlayingTimer = gamePlayingTimerMax;
                    SetState(State.GamePlaying);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0f)
                {
                    SetState(State.GameOver);
                }
                break;
            case State.GameOver:
                break;
        }

    }

    private void SetState(State state)
    {
        if(this.state != state)
        {
            this.state = state;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }


    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;

        Time.timeScale = isGamePaused ? 0f : 1f;

        if (isGamePaused)
        {
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);

        }
    }


    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state == State.CountdownToStart;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public float GetCountdownToStartTimer()
    {
        return coundownToStartTimer;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

}
