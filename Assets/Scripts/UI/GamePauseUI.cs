using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() => MainMenuButtonPressed());
        resumeButton.onClick.AddListener(() => ResumeButtonPressed());
        optionsButton.onClick.AddListener(() => OptionsButtonPressed());
    }


    private void Start()
    {
        
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

        Hide();
    }
    private void MainMenuButtonPressed()
    {
        Loader.Load(Loader.Scene.MainMenuScene);
    }

    private void ResumeButtonPressed()
    {
        KitchenGameManager.Instance.TogglePauseGame();
    }

    private void OptionsButtonPressed()
    {
        Hide();
        OptionsUI.Instance.Show(Show);
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();

    }

    private void Show()
    {
        gameObject.SetActive(true);
        resumeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);

    }
}
