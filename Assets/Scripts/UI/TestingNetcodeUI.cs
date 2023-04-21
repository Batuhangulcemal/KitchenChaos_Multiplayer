using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startClientButton;

    private void Awake()
    {
        startHostButton.onClick.AddListener(() => StartHost());
        startClientButton.onClick.AddListener(() => StartClient());

        
    }

    private void StartHost()
    {
        Debug.Log("Host");
        NetworkManager.Singleton.StartHost();
        Hide();
    }

    private void StartClient()
    {
        Debug.Log("Client");
        NetworkManager.Singleton.StartClient();
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
