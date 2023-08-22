using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [Range(0.1f, 5.3f)] public float speed;

    private Vector2 nowPos, prePos;
    private Vector3 movePos;
    
    private Rigidbody2D camRb;

    //NPC���� ��ȭ ������ ������� ī�޶� �������� �����ؾ� �� ��
    private bool isFocus;
    private Vector2 clickPos;

    public GameObject cam;


    void Start()
    {
        // cam = GameObject.Find("Main Camera");
        camRb = cam.GetComponent<Rigidbody2D>();
        speed = 0.4f;
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
        else {  }
    }

    public void FocusCamera()
    {
        isFocus = true;
        //Debug.Log("��Ŀ�� �Լ� ȣ��");
    }

    public void FreeCamera()
    {
        isFocus = false;
        //Debug.Log("���� �Լ� ȣ��");
    }
}
