using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [Range(0.1f, 5.3f)]
    public float speed = 0.25f;

    private Vector2 nowPos, prePos;
    private Vector3 movePos;
    private GameObject cam;
    private Rigidbody2D camRb;

    //NPC와의 대화 등으로 사용자의 카메라 움직임을 제한해야 할 때
    private bool isFocus;
    private Vector2 clickPos;


    void Start()
    {
        cam = GameObject.Find("Main Camera");
        camRb = cam.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFocus)
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
        else { Debug.Log("현재 카메라 포커싱 중입니다. 못 움직입니다."); }
    }

    public void FocusCamera()
    {
        isFocus = true;
        Debug.Log("포커싱 함수 호출");
    }

    public void FreeCamera()
    {
        isFocus = false;
        Debug.Log("프리 함수 호출");
    }
}
