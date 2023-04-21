using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        SetBarImageFillAmount(0f);
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        SetBarImageFillAmount(e.progressNormalized);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);

    }

    private void SetBarImageFillAmount(float amount)
    {
        barImage.fillAmount = amount;
        if(amount == 0f || amount == 1f)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
}
