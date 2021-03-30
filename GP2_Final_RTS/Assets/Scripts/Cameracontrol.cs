using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontrol : MonoBehaviour
{
    Vector3 pos;
    float moveSpeed = 20f;
    float ScreenEdge = 10.0f;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        pos = this.transform.position;
        Move();
    }

    void Move()
    {
        if (Input.mousePosition.y >= Screen.height - ScreenEdge)
        {
            pos.x -= moveSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.y <= Screen.height - Screen.height + ScreenEdge)
        {
            pos.x += moveSpeed * Time.deltaTime;
        }
        else if (Input.mousePosition.x <= Screen.width - Screen.width + ScreenEdge)
            pos.z -= moveSpeed * Time.deltaTime;
        else if (Input.mousePosition.x >= Screen.width - ScreenEdge)
            pos.z += moveSpeed * Time.deltaTime;
        transform.position = pos;
    }
}
