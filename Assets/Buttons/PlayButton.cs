using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour
{
    private Transform Camera;
    void OnMouseUp()
    {
        Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        Camera.GetComponent<Transform>().position = new Vector3(3.5f, 3.5f, -100);    
    }
}
