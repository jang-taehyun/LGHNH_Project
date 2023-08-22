using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover_Test : MonoBehaviour
{
    [Range(1f, 1000.0f)]
    public float speed = 100.0f;
    
    private Rigidbody2D camRb;

    //NPC���� ��ȭ ������ ������� ī�޶� �������� �����ؾ� �� ��
    private bool isFocus;


    private Vector2 clickPos;

    public GameObject cam;

    void Start()
    {
        // cam = GameObject.Find("Main Camera");
        camRb = cam.GetComponent<Rigidbody2D>();

        isFocus = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isFocus)
        {
            if (Input.GetMouseButtonDown(0))
                clickPos = Input.mousePosition;

            if (Input.GetMouseButton(0))
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint((Vector2)Input.mousePosition - clickPos);
                Vector3 move = pos * (Time.deltaTime * speed) * -1;
                camRb.AddForce(move);
            }
        }
        
    }

    public void FocusCamera()
    {
        isFocus = true;
    }

    public void FreeCamera()
    {
        isFocus = false;
    }
}
