using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [Range(0.1f, 100.0f)] public float speed;

    private Vector2 nowPos, prePos;
    private Vector3 movePos;
    private Vector3 vel;
    
    

    private bool isFocus;
    private bool isDialogAuto;
    private Vector2 clickPos;

    public ProcessManager processManager;
    public DialogSystem dialogSystem;
    public GameObject cam;
    private Rigidbody2D camRb;
    private BoxCollider2D cam_BoxCollider2D;
    private bool isCam_BoxCollider2DOff;

    [Space(10f)]
    [Header("장애물 파괴 전, 퀘스트 완료 후에 보여줘야 할 오브젝트들 모음")]
    public Transform[] forShoot_BeforeDestroy;
    public bool[] checkShoot_BeforeDestroy;
    private Vector3[] forShoot_BeforeDestroy_CameraPos;
    private bool trigShoot_BeforeDestroy;

    [Space (10f)]
    [Header ("장애물 파괴 후에 보여줘야 하는 오브젝트들 모음")]
    public Transform[] forShoot_AfterDestroy;
    public bool[] checkShoot_AfterDestroy;
    private Vector3[] forShoot_AfterDestroy_CameraPos;
    private bool trigShoot_AfterDestroy;
    


    void Start()
    {
        // cam = GameObject.Find("Main Camera");
        camRb = cam.GetComponent<Rigidbody2D>();
        cam_BoxCollider2D = cam.GetComponent<BoxCollider2D>();
        vel = Vector3.zero;
        speed = 30.0f;

        forShoot_AfterDestroy_CameraPos = new Vector3[forShoot_AfterDestroy.Length];
        for (int i = 0; i < forShoot_AfterDestroy.Length; i++) 
        {
            forShoot_AfterDestroy_CameraPos[i] = new Vector3(forShoot_AfterDestroy[i].position.x,
                                                             forShoot_AfterDestroy[i].position.y,
                                                             -10);

            if (processManager.ReadSubsectorNum() == 4)
            {
                if (forShoot_AfterDestroy_CameraPos[i].x <= -15f || forShoot_AfterDestroy_CameraPos[i].x >= 15f)
                {
                    if (forShoot_AfterDestroy_CameraPos[i].x <= -15f) { forShoot_AfterDestroy_CameraPos[i].x = -15f; }
                    else { forShoot_AfterDestroy_CameraPos[i].x = 15f; }
                }

                if (forShoot_AfterDestroy_CameraPos[i].y <= -30f || forShoot_AfterDestroy_CameraPos[i].y >= 30f)
                {
                    if (forShoot_AfterDestroy_CameraPos[i].y <= -30f) { forShoot_AfterDestroy_CameraPos[i].y = -30f; }
                    else { forShoot_AfterDestroy_CameraPos[i].y = 30f; }
                }
            }
            else if (processManager.ReadSubsectorNum() == 3)
            {
                if (forShoot_AfterDestroy_CameraPos[i].x <= -24.8f || forShoot_AfterDestroy_CameraPos[i].x >= 24.8f)
                {
                    if (forShoot_AfterDestroy_CameraPos[i].x <= -24.8f) { forShoot_AfterDestroy_CameraPos[i].x = -24.8f; }
                    else { forShoot_AfterDestroy_CameraPos[i].x = 24.8f; }
                }

                if (forShoot_AfterDestroy_CameraPos[i].y <= -30f || forShoot_AfterDestroy_CameraPos[i].y >= 30f)
                {
                    if (forShoot_AfterDestroy_CameraPos[i].y <= -30f) { forShoot_AfterDestroy_CameraPos[i].y = -30f; }
                    else { forShoot_AfterDestroy_CameraPos[i].y = 30f; }
                }
            }
            else if (processManager.ReadSubsectorNum() == 2)
            {
                if (forShoot_AfterDestroy_CameraPos[i].x <= -12f || forShoot_AfterDestroy_CameraPos[i].x >= 12f)
                {
                    if (forShoot_AfterDestroy_CameraPos[i].x <= -12f) { forShoot_AfterDestroy_CameraPos[i].x = -12f; }
                    else { forShoot_AfterDestroy_CameraPos[i].x = 12f; }
                }

                if (forShoot_AfterDestroy_CameraPos[i].y <= -8f  || forShoot_AfterDestroy_CameraPos[i].y >= 8f)
                {
                    if (forShoot_AfterDestroy_CameraPos[i].y <= -8f) { forShoot_AfterDestroy_CameraPos[i].y = -8f; }
                    else { forShoot_AfterDestroy_CameraPos[i].y = 8f; }
                }
            }
            else
            {
                if (forShoot_AfterDestroy_CameraPos[i].x <= -12f || forShoot_AfterDestroy_CameraPos[i].x >= 12f)
                {
                    if (forShoot_AfterDestroy_CameraPos[i].x <= -12f) { forShoot_AfterDestroy_CameraPos[i].x = -12f; }
                    else { forShoot_AfterDestroy_CameraPos[i].x = 12f; }
                }

                if (forShoot_AfterDestroy_CameraPos[i].y <= -5f || forShoot_AfterDestroy_CameraPos[i].y >= 5f)
                {
                    if (forShoot_AfterDestroy_CameraPos[i].y <= -5f) { forShoot_AfterDestroy_CameraPos[i].y = -5f; }
                    else { forShoot_AfterDestroy_CameraPos[i].y = 5f; }
                }
            }
        }

        forShoot_BeforeDestroy_CameraPos = new Vector3[forShoot_BeforeDestroy.Length];
        for (int i = 0; i < forShoot_BeforeDestroy.Length; i++)
        {
            forShoot_BeforeDestroy_CameraPos[i] = new Vector3(forShoot_BeforeDestroy[i].position.x,
                                                             forShoot_BeforeDestroy[i].position.y,
                                                             -10);

            if (processManager.ReadSubsectorNum() == 4)
            {
                if (forShoot_BeforeDestroy_CameraPos[i].x <= -15f || forShoot_BeforeDestroy_CameraPos[i].x >= 15f)
                {
                    if (forShoot_BeforeDestroy_CameraPos[i].x <= -15f) { forShoot_BeforeDestroy_CameraPos[i].x = -15f; }
                    else { forShoot_BeforeDestroy_CameraPos[i].x = 15f; }
                }

                if (forShoot_BeforeDestroy_CameraPos[i].y <= -30f || forShoot_BeforeDestroy_CameraPos[i].y >= 30f)
                {
                    if (forShoot_BeforeDestroy_CameraPos[i].y <= -30f) { forShoot_BeforeDestroy_CameraPos[i].y = -30f; }
                    else { forShoot_BeforeDestroy_CameraPos[i].y = 30f; }
                }
            }
            else if (processManager.ReadSubsectorNum() == 3)
            {
                if (forShoot_BeforeDestroy_CameraPos[i].x <= -22.95f || forShoot_BeforeDestroy_CameraPos[i].x >= 22.95f)
                {
                    if (forShoot_BeforeDestroy_CameraPos[i].x <= -22.95f) { forShoot_BeforeDestroy_CameraPos[i].x = -22.95f; }
                    else { forShoot_BeforeDestroy_CameraPos[i].x = 22.95f; }
                }

                if (forShoot_BeforeDestroy_CameraPos[i].y <= -24.5f || forShoot_BeforeDestroy_CameraPos[i].y >= 24.5f)
                {
                    if (forShoot_BeforeDestroy_CameraPos[i].y <= -24.5f) { forShoot_BeforeDestroy_CameraPos[i].y = -24.5f; }
                    else { forShoot_BeforeDestroy_CameraPos[i].y = 24.5f; }
                }
            }
            else if (processManager.ReadSubsectorNum() == 2)
            {
                if (forShoot_BeforeDestroy_CameraPos[i].x <= -12f || forShoot_BeforeDestroy_CameraPos[i].x >= 12f)
                {
                    if (forShoot_BeforeDestroy_CameraPos[i].x <= -12f) { forShoot_BeforeDestroy_CameraPos[i].x = -12f; }
                    else { forShoot_BeforeDestroy_CameraPos[i].x = 12f; }
                }

                if (forShoot_BeforeDestroy_CameraPos[i].y <= -8f || forShoot_BeforeDestroy_CameraPos[i].y >= 8f)
                {
                    if (forShoot_BeforeDestroy_CameraPos[i].y <= -8f) { forShoot_BeforeDestroy_CameraPos[i].y = -8f; }
                    else { forShoot_BeforeDestroy_CameraPos[i].y = 8f; }
                }
            }
            else
            {
                if (forShoot_BeforeDestroy_CameraPos[i].x <= -12f || forShoot_BeforeDestroy_CameraPos[i].x >= 12f)
                {
                    if (forShoot_BeforeDestroy_CameraPos[i].x <= -12f) { forShoot_BeforeDestroy_CameraPos[i].x = -12f; }
                    else { forShoot_BeforeDestroy_CameraPos[i].x = 12f; }
                }

                if (forShoot_BeforeDestroy_CameraPos[i].y <= -5f || forShoot_BeforeDestroy_CameraPos[i].y >= 5f)
                {
                    if (forShoot_BeforeDestroy_CameraPos[i].y <= -5f) { forShoot_BeforeDestroy_CameraPos[i].y = -5f; }
                    else { forShoot_BeforeDestroy_CameraPos[i].y = 5f; }
                }
            }
        }
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
                    camRb.AddForce(movePos);
                    
                    prePos = touch.position - touch.deltaPosition;
                }
            }
        }

        if (trigShoot_AfterDestroy)
        {
            if (!isFocus) { isFocus = true; }
            if (isCam_BoxCollider2DOff == false) 
            {
                isCam_BoxCollider2DOff = true;
                cam_BoxCollider2D.enabled = false;
            }
            ShootAfterDestroy();

            if (Vector2.Distance(cam.transform.position, forShoot_AfterDestroy_CameraPos[processManager.ReadPhase()]) <= 0.1f)
            {
                trigShoot_AfterDestroy = false;
                isFocus = false;

                if (isCam_BoxCollider2DOff == true)
                {
                    isCam_BoxCollider2DOff = false;
                    cam_BoxCollider2D.enabled = true;
                }
                if (dialogSystem.autoStartBranch[processManager.ReadPhase()*2] == true)
                {
                    dialogSystem.StartConversationSetting_Auto();
                }
            }
        }
        if (trigShoot_BeforeDestroy)
        {
            if (!isFocus) { isFocus = true; }
            if (isCam_BoxCollider2DOff == false)
            {
                isCam_BoxCollider2DOff = true;
                cam_BoxCollider2D.enabled = false;
            }
            ShootBeforeDestroy();

            if (Vector2.Distance(cam.transform.position, forShoot_BeforeDestroy_CameraPos[processManager.ReadPhase()]) <= 0.1f)
            {
                trigShoot_BeforeDestroy = false;
                isFocus = false;

                if (isCam_BoxCollider2DOff == true)
                {
                    isCam_BoxCollider2DOff = false;
                    cam_BoxCollider2D.enabled = true;
                }
                if (dialogSystem.autoStartBranch[processManager.ReadPhase() * 2+1] == true)
                {
                    dialogSystem.StartConversationSetting_Auto(true);
                }
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

    public void ShootAfterDestroy()
    {
        cam.transform.position = Vector3.SmoothDamp(
            cam.transform.position, forShoot_AfterDestroy_CameraPos[processManager.ReadPhase()], ref vel, 0.5f);
    }

    public void ShootBeforeDestroy()
    {
        cam.transform.position = Vector3.SmoothDamp(
            cam.transform.position, forShoot_BeforeDestroy_CameraPos[processManager.ReadPhase()], ref vel, 0.5f);
    }

    public bool SetTrig_ShootAfterDestroy(bool val)
    {
        if (checkShoot_AfterDestroy[processManager.ReadPhase()] == true)
        {
            trigShoot_AfterDestroy = val;
            return true;
        }
        else {  return false; }
    }

    public bool SetTrig_ShootBeforeDestroy(bool val)
    {
        if (checkShoot_BeforeDestroy[processManager.ReadPhase()] == true)
        {
            trigShoot_BeforeDestroy = val;
            return true;
        }
        else { return false; }
    }
}
