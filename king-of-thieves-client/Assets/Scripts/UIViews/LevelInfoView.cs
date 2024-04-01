using System;
using LevelMechanics.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoView : MonoBehaviour, ILevelInfoView
{
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button replayButton;

    [SerializeField]
    private TMP_Text currentScoreLabel;
    [SerializeField]
    private TMP_Text maxScoreLabel;

    public event Action PlayPressed;
    public event Action ReplayPressed;

    private void Awake()
    {
        this.playButton.onClick.AddListener(OnPLayButtonPressed);
        this.replayButton.onClick.AddListener(OnReplayButtonPressed);
    }

    private void OnDestroy()
    {
        this.playButton.onClick.RemoveAllListeners();
        this.replayButton.onClick.RemoveAllListeners();
    }

    public void ShowWelcome(int maxScore)
    {
        maxScoreLabel.text = maxScore.ToString();
        currentScoreLabel.text = "0";
        this.replayButton.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void ShowGameOver(int score, int maxScore)
    {
        currentScoreLabel.text = score.ToString();
        maxScoreLabel.text = maxScore.ToString();
        maxScoreLabel.enabled = true;
        this.replayButton.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    private void OnPLayButtonPressed()
    {
        PlayPressed?.Invoke();
    }

    private void OnReplayButtonPressed()
    {
        this.ReplayPressed?.Invoke();
    }
}
