using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data", order = 1)]
public class LevelData : ScriptableObject
{
    [Header("Level Info")]
    public string levelName;
    public string sceneName;
    public int levelIndex;
    
    [Header("Requirements")]
    public int keysRequired;
    
    [Header("Next Level")]
    public LevelData nextLevel;
    
    [Header("Display")]
    public Sprite levelPreview;
    [TextArea]
    public string levelDescription;
}
