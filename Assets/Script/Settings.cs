using UnityEngine;
public class Settings : Singleton<Settings>
{
    protected Settings() { } // guarantee this will be always a singleton only - can't use the constructor!

    public float speed = 0.5f;

    //Searchwidth and height for A* pathfinding algorithm
    public int SearchWidth = 10;
    public int SearchHeight = 10;
    public Vector2 TileSize = new Vector2(1, 1);
    public LayerMask UnpassableLayer;

    public string HighScoreFilePath = "HighScore.txt";

    public int Level;

    public int Score;

    public int MaxNumberofHighScores;

    void Awake()
    {
        Reload();
        HighScoreFilePath = Application.persistentDataPath + "/" + HighScoreFilePath;
        // Your initialization code here
    }

    // (optional) allow runtime registration of global objects
    static public T RegisterComponent<T>() where T : Component
    {
        return Instance.GetOrAddComponent<T>();
    }
}

