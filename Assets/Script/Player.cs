using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, iPlayer
{
    float usedSpeed;
    public void GetPlayerInput()
    {
    }
    public void MovePlayer()
    {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * usedSpeed);

    }
    public void Sprint()
    {
        usedSpeed *= 1.3f;
    }
	// Use this for initialization
	void Start () {
        usedSpeed = Settings.Instance.speed;
	}
	
	// Update is called once per frame
	void Update () {
        MovePlayer();
	}
}
