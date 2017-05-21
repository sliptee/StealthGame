using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MainMenu
{
    float escTimer = 0;
    bool decreaseTimer;
    public Text playerfield;
    string userID;
    public float EscTimer = 0.5f; //Ger en delay på hur ofta escape stänger av eller sätter på menyn. 

    public void SaveGame()
    {
        PlayerPrefs.SetInt("Level", Settings.Instance.Level);
        PlayerPrefs.SetInt("Score", Settings.Instance.TotalScore); 
    }
    public void SaveHighScore()
    {
        foreach (Transform child in MenuCanvas.transform)  //Avaktiverar alla aktiva knappar och aktiverar input-fältet. 
        {
            if (child.tag == "Button")
                child.gameObject.SetActive(false);
            if (child.name == "Input Name")
                child.gameObject.SetActive(true);
            else
                child.gameObject.SetActive(false);
        }
        userID = playerfield.text.ToString();
        if (userID.Length <= 0) //Om namnet är tomt, gör inget 
            return;
        if (userID.Length > 15)
            userID = userID.Substring(0, 15);
        print("Saving highscore for name: " + userID + " and score: " + Settings.Instance.TotalScore);
        LevelManager.SaveHighScore(userID);
        SceneManager.LoadSceneAsync(1);
        Time.timeScale = 1;
        Settings.Instance.TotalScore = 0;
    }
    public void Continue()
    {
        TurnOffMenu();
    }
    void Update()
    {
        if (Input.GetButton("Cancel") && escTimer <= 0 && !LevelManager.GameIsOver)
        {
            if (!LevelManager.IsInMenu)
            {
                TurnOnMenu();
            }
            else
            {
                TurnOffMenu();
            }
            escTimer = EscTimer;
            decreaseTimer = true;
        }
        if (escTimer <= 0)
            decreaseTimer = false;
        if (decreaseTimer)
            escTimer -= Time.unscaledDeltaTime;
    }
}