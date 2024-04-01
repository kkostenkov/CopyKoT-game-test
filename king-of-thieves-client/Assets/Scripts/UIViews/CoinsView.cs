using LevelMechanics.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinsView : MonoBehaviour, ICoinsView
{
    [SerializeField]
    private Image coinsImage;
    [SerializeField]
    private TMP_Text coinsText;

    private void Awake()
    {
        SetCoinCount(0);
    }

    public void SetCoinCount(int newBalance)
    {
        this.coinsText.text = newBalance.ToString();
    }
}
