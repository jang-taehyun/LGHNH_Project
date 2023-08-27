using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Animation obsEliminationAnimation;            //장애물을 제거하는 애니메이션
    public AudioSource eliminationSound;                 //장애물을 제거할 때 나는 경쾌한 소리
    

    [SerializeField] private GameObject cameraLimitRect;                  //카메라 제한 영역
    [SerializeField] private GameObject cam;                              //카메라
    private CameraMover cam_CameraMover;
    [SerializeField] private Transform center;
    [SerializeField] private ProcessManager processManager;
    [SerializeField] private DialogSystem dialogSystem;
    private Vector3 vel;

    private float obsPositionX, obsPositionY;            //카메라한테 넘겨주어야 하는 NPC의 위치 정보
    private Vector3 obsPositionForCamera;
    private Vector3 previousCamPosition;
    private bool trig_CameraMoveToObs;
    public bool isNotMovetoObs;


    private bool isDialogAuto;

    public Vector3 NPCPosition;

    void Start()
    {
        trig_CameraMoveToObs = false;
        //cameraLimitRect.SetActive(false); Debug.Log(transform.Find("Camera Limit").gameObject);
        vel = Vector3.zero;
        cam_CameraMover = cam.GetComponent<CameraMover>();

        obsPositionX = center.position.x;
        obsPositionY = center.position.y; 
        center.gameObject.SetActive(false);

        //장애물이 맵 경계와 붙어 있으면 카메라가 떨리는 문제를 해결하기 위함.
        //솔라 넥타르 호수(subsectorNum = 4)는 맵 크기가 달라서 특별한 조치가 필요하다.
        if (processManager.ReadSubsectorNum() == 4)
        {
            if (obsPositionX <= -15f || obsPositionX >= 15f)
            {
                if (obsPositionX <= -15f) { obsPositionX = -15f; }
                else { obsPositionX = 15f; }
            }

            if (obsPositionY <= -30f || obsPositionY >= 30f)
            {
                if (obsPositionY <= -30f) { obsPositionY = -30f; }
                else { obsPositionY = 30f; }
            }
        }
        else if (processManager.ReadSubsectorNum() == 3)
        {
            if (obsPositionX <= -24.8f || obsPositionX >= 24.8f)
            {
                if (obsPositionX <= -24.8f) { obsPositionX = -24.8f; }
                else { obsPositionX = 24.8f; }
            }

            if (obsPositionY <= -30f || obsPositionY >= 30f)
            {
                if (obsPositionY <= -30f) { obsPositionY = -30f; }
                else { obsPositionY = 30f; }
            }
        }
        else if (processManager.ReadSubsectorNum() == 2)
        {
            if (obsPositionX <= -12f || obsPositionX >= 12f)
            {
                if (obsPositionX <= -12f) { obsPositionX = -12f; }
                else { obsPositionX = 12f; }
            }

            if (obsPositionY <= -8f || obsPositionY >= 8f)
            {
                if (obsPositionY <= -8f) { obsPositionY = -8f; }
                else { obsPositionY = 8f; }
            }
        }
        else
        {
            if (obsPositionX <= -12.45f || obsPositionX >= 12.45f)
            {
                if (obsPositionX <= -12.45f) { obsPositionX = -12.45f; }
                else { obsPositionX = 12.45f; }
            }

            if (obsPositionY <= -5.45f || obsPositionY >= 5.45f)
            {
                if (obsPositionY <= -5.45f) { obsPositionY = -5.45f; }
                else { obsPositionY = 5.45f; }
            }
        } 

        obsPositionForCamera = new Vector3(obsPositionX, obsPositionY, -10);
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
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, obsPositionForCamera, ref vel, 0.5f);
    }
    public void CameraMoveToNPC()
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

        cameraLimitRect.SetActive(false);
        previousCamPosition = cam.transform.position;
        trig_CameraMoveToObs = true;
        cam.GetComponent<CameraMover>().FocusCamera();
        StartCoroutine(AfterDestroyThisObs_Delay(3.0f));
    }
    
    IEnumerator AfterDestroyThisObs_Delay(float delayTime) 
    {
        if (isNotMovetoObs == false)
        {
            yield return new WaitForSeconds(delayTime);
            trig_CameraMoveToObs = false;
            cam.GetComponent<CameraMover>().FreeCamera();
            //cam.transform.position = previousCamPosition;

            processManager.increasePhase();

            //서브섹터가 끝나지 않았을 경우에만 실행
            if (processManager.IsSubsectorEnded() == false)
            {
                Debug.Log("현재 ReadPhase의 값 : " + processManager.ReadPhase());

                //카메라 줌 다음 진행 NPC한테 안 해도 된다는 뜻
                if (cam_CameraMover.SetTrig_ShootAfterDestroy(true) == false)
                {
                    if (isDialogAuto)
                    {
                        dialogSystem.StartConversationSetting_Auto();
                        isDialogAuto = false;
                    }
                }
            }
            else
            {

            }

            gameObject.SetActive(false);
        }
        else
        {
            DontDestroyObsSetting();
        }

        
    }

    private void DontDestroyObsSetting()
    {
        trig_CameraMoveToObs = false;
        cam.GetComponent<CameraMover>().FreeCamera();
        //cam.transform.position = previousCamPosition;

        processManager.increasePhase();

        //서브섹터가 끝나지 않았을 경우에만 실행
        if (processManager.IsSubsectorEnded() == false)
        {
            Debug.Log("현재 ReadPhase의 값 : " + processManager.ReadPhase());

            //카메라 줌 다음 진행 NPC한테 안 해도 된다는 뜻
            if (cam_CameraMover.SetTrig_ShootAfterDestroy(true) == false)
            {
                if (isDialogAuto)
                {
                    dialogSystem.StartConversationSetting_Auto();
                    isDialogAuto = false;
                }
            }
        }
        gameObject.SetActive(false);
    }

    public void SetDialogAuto(bool val = true) { isDialogAuto = val; }
}
