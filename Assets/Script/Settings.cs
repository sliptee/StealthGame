using UnityEngine;
/// <summary>
/// Klassen behöver köras EN gång (under Main Menu). 
/// </summary>
public class Settings : Singleton<Settings>
{
    protected Settings() { } // guarantee this will be always a singleton only - can't use the constructor!

    public float CameraSpeed = 0.5f; 
    public float EnemySpeed = 10f;
    public float PlayerSpeed = 8f;
    public float PlayerTurnSpeed = 8f;
    public float EnemyTurnSpeed = 8;
    public float TimeUntilPlayerSpotted = 0.3f;

    public Vector2 TileSize = new Vector2(1,1);

    public string HighScoreFilePath = "HighScore.txt";

    public int Level = 1;

    public int TotalScore = 0;
    public int TemporaryScore = 0;

    public int MaxNumberofHighScores = 5;
    public static Settings settings;
    protected override void Awake()
    {
        Reload();
        if (settings == null)
        {
            DontDestroyOnLoad(gameObject);
            settings = this;
        }
        HighScoreFilePath = Application.persistentDataPath + "/" + HighScoreFilePath;
        // Your initialization code here
    }
}

