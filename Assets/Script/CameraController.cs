using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Update is called once per frame
    float moveY;

    void Update ()
    {

        moveY = Input.GetAxis("Mouse ScrollWheel") * 15f;
        if (transform.position.y <= 15 && moveY > 0)        //Begränsar camerans höjd mellan 15 och 30
        {
            moveY = 0;
        }
        else if(transform.position.y >= 40 && moveY < 0)
        {
            moveY = 0;
        }
        transform.Translate(new Vector3(Input.GetAxis("HorizontalCamera"), Input.GetAxis("VerticalCamera"), moveY) * Settings.Instance.CameraSpeed);

    }
}
