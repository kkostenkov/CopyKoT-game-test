using LevelMechanics.UI;
using TMPro;
using UnityEngine;

public class LevelTimeView : MonoBehaviour, ILevelTimeView
{
    [SerializeField]
    private TMP_Text time; 
        
    public void SetTimeText(string timeText)
    {
        time.text = timeText;
    }
}
