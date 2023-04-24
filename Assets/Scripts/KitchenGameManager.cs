using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

public class KitchenGameManager : NetworkBehaviour
{
    public static KitchenGameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    public event EventHandler OnLocalPlayerReadyChanged;
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private NetworkVariable<State> state = new(State.WaitingToStart);
    private bool isLocalPlayerReady;
    private NetworkVariable<float> coundownToStartTimer = new(3f);
    private NetworkVariable<float> gamePlayingTimer = new(0f);
    private float gamePlayingTimerMax = 90f;
    private bool isGamePaused = false;
    private Dictionary<ulong, bool> playerReadyDictionary;


    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
    }


    private void State_OnValueChanged(State previousState, State newState)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state.Value == State.WaitingToStart)
        {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
            SetplayerReadyServerRpc();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void SetplayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;

        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                //this player is not ready
                allClientsReady = false;
            }
        }

        if (allClientsReady)
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
        if (!IsServer) return;
        switch (state.Value)
        {
            case State.WaitingToStart:

                break;
            case State.CountdownToStart:
                coundownToStartTimer.Value -= Time.deltaTime;
                if (coundownToStartTimer.Value < 0f)
                {
                    gamePlayingTimer.Value = gamePlayingTimerMax;
                    SetState(State.GamePlaying);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                if (gamePlayingTimer.Value < 0f)
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
        this.state.Value = state;       
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
        return state.Value == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state.Value == State.CountdownToStart;
    }

    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }

    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }

    public float GetCountdownToStartTimer()
    {
        return coundownToStartTimer.Value;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer.Value / gamePlayingTimerMax);
    }

}
