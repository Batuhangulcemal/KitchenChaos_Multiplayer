using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() => PlayButtonPressed());
        quitButton.onClick.AddListener(() => QuitButtonPressed());

        Time.timeScale = 1f;
    }

    private void PlayButtonPressed()
    {
        Loader.Load(Loader.Scene.GameScene);
    }

    private void QuitButtonPressed()
    {
        Application.Quit();
    }
}
