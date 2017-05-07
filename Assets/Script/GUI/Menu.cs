using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MainMenu
{
    Camera main;
    float escTimer = 0;
    bool decreaseTimer;
    public float EscTimer = 0.5f; //Ger en delay på hur ofta escape stänger av eller sätter på menyn. 
    void Start()
    {
        main = Camera.main;
    }
    public void SaveGame()
    {
        PlayerPrefs.SetInt("Level", Settings.Instance.Level);
    }
    public void Continue()
    {
        main.enabled = true;
        TurnOffMenu();
    }
    void FixedUpdate()
    {
        if (Input.GetButton("Cancel") && escTimer <= 0)
        {
            if (main.enabled == true)
            {
                main.enabled = false;
                TurnOnMenu();
            }
            else
            {
                main.enabled = true;
                TurnOffMenu();
            }
            escTimer = EscTimer;
            decreaseTimer = true;
        }
        if (escTimer <= 0)
            decreaseTimer = false;
        if (decreaseTimer)
            escTimer -= Time.deltaTime;
    }
}