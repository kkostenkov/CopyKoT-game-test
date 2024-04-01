using UnityEngine;

[CreateAssetMenuAttribute(fileName = "GameSettings", menuName = "GameData/GameSettings")]
public class GameSettingsScriptable : ScriptableObject
{
    public int ChestChancePercent = 10;
    public int MinSpawnDelaySeconds = 3;
    public int MaxSpawnDelaySeconds = 10;
    public int LevelTimeLimitSeconds = 60;
}