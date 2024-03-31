using System;
using LevelMechanics.UI;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoView : MonoBehaviour, ILevelInfoView
{
    [SerializeField]
    private Button playButton;

    public event Action PlayPressed;

    private void Awake()
    {
        this.playButton.onClick.AddListener(OnPLayButtonPressed);
    }

    private void OnDestroy()
    {
        this.playButton.onClick.RemoveAllListeners();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void OnPLayButtonPressed()
    {
        PlayPressed?.Invoke();
    }
}
