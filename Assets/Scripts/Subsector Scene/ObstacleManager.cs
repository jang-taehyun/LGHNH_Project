using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Animation obsEliminationAnimation;            //장애물을 제거하는 애니메이션
    public AudioSource eliminationSound;                 //장애물을 제거할 때 나는 경쾌한 소리

    private GameObject cameraLimitRect;                  //카메라 제한 영역
    private GameObject cam;                              //카메라
    
    private Vector3 vel;

    private float obsPositionX, obsPositionY;            //카메라한테 넘겨주어야 하는 NPC의 위치 정보
    private Vector3 obsPositionForCamera;
    private Vector3 previousCamPosition;
    private bool trig_CameraMoveToObs;

    private bool isDialogAuto;

    void Start()
    {
        trig_CameraMoveToObs = false;
        cameraLimitRect = transform.Find("Camera Limit").gameObject; cameraLimitRect.SetActive(false);
        Debug.Log(transform.Find("Camera Limit").gameObject);
        cam = GameObject.Find("Main Camera");
        vel = Vector3.zero;

        obsPositionX = transform.Find("center").position.x;
        obsPositionY = transform.Find("center").position.y; 
        transform.Find("center").gameObject.SetActive(false);

        //장애물이 맵 경계와 붙어 있으면 카메라가 떨리는 문제를 해결하기 위함.
        if (obsPositionX <= -12.45f || obsPositionX >= 12.45f)
        {
            if (obsPositionX <= -12.45f) { obsPositionX = -12.45f; }
            else { obsPositionX = 12.45f; }
        }

        if (obsPositionY <= -5.45f || obsPositionY >= 5.45f)
        {
            if (obsPositionY <= -5.45f) { obsPositionY = -5.45f; }
            else { obsPositionY = 5.45f;  }
        }

        
        

        obsPositionForCamera = new Vector3(obsPositionX, obsPositionY, -10);
        Debug.Log("카메라를 위한 장애물 위치: " + obsPositionForCamera + " " + gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (trig_CameraMoveToObs) { CameraMoveToObs(); }

    }
    public void DestroyThisObs( )
    {
        StartCoroutine(DestroyThisObs_Delay(1.0f));
    }

    public void CameraMoveToObs()
    {
        Debug.Log("카메라 이동중~");
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, obsPositionForCamera, ref vel, 0.5f);
    }

    /* (2023.08.06자 수정사항) 서브 섹터 씬 내 장애물을 파괴할 때 확대시키면 잘 안 보여서,
       결국에 장애물 줌인/줌아웃은 안 하기로 했다. 근데 혹시 몰라서 코드는 남겨 놓았다. */
    IEnumerator CameraZoomIn()
    {

        for (int i = 100; i >= 50; i--)
        {
            cam.GetComponent<Camera>().orthographicSize = i * 0.1f;
            cam.GetComponent<BoxCollider2D>().size = new Vector2(i * 0.1f, i * 0.2f);

            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    IEnumerator CameraZoomOut()
    {

        for (int i = 50; i <= 100; i++)
        {
            cam.GetComponent<Camera>().orthographicSize = i * 0.1f;
            cam.GetComponent<BoxCollider2D>().size = new Vector2(i * 0.1f, i * 0.2f);

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator DestroyThisObs_Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Debug.Log("여기는 한 번만 실행됩니다."); 
        cameraLimitRect.SetActive(false);
        previousCamPosition = cam.transform.position;
        trig_CameraMoveToObs = true;
        cam.GetComponent<CameraMover>().FocusCamera();
        StartCoroutine(AfterDestroyThisObs_Delay(3.0f));
    }
    
    IEnumerator AfterDestroyThisObs_Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        trig_CameraMoveToObs = false;
        cam.GetComponent<CameraMover>().FreeCamera();
        cam.transform.position = previousCamPosition;
        GameObject.Find("Process Manager").GetComponent<ProcessManager>().increaseOngoingIndex();
        if (isDialogAuto)
        {
            GameObject.Find("DialogSystem").GetComponent<DialogSystem>().StartConversationSetting_Auto();
            isDialogAuto = false;
        }
        
        gameObject.SetActive(false);
    }

    public void SetDialogAuto() { isDialogAuto = true; }
}
