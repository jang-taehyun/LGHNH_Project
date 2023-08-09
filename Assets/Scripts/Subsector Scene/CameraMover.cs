using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [Range(0.1f, 0.3f)]
    public float speed = 0.25f;

    private Vector2 nowPos, prePos;
    private Vector3 movePos;
    private GameObject cam;



    void Start()
    {
        cam = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                prePos = touch.position - touch.deltaPosition;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                nowPos = touch.position - touch.deltaPosition;
                movePos = (Vector3)(prePos - nowPos) * Time.deltaTime * speed;
                cam.transform.Translate(movePos);
                prePos = touch.position - touch.deltaPosition;
            }
        }
    }
}
