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

    public void SetCoinCount(int newBalance)
    {
        this.coinsText.text = newBalance.ToString();
    }
}
