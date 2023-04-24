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
        KitchenGameMultiplayer.Instance.StartHost();
        Hide();
    }

    private void StartClient()
    {
        Debug.Log("Client");
        KitchenGameMultiplayer.Instance.StartClient();

        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
