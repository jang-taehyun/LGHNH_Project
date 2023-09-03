using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Animation obsEliminationAnimation;            //장애물을 제거하는 애니메이션
    public AudioSource eliminationSound;                 //장애물을 제거할 때 나는 경쾌한 소리(player)
    public AudioClip soundClip;                          // sound effect
    

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
    public bool needToNotFadeOut;



    private bool isDialogAuto;

    public Vector3 NPCPosition;

    [Header("장애물 파괴를 위한 SpriteRenders")]
    public SpriteRenderer[] obsChildSpriteRenderers;

    private ObstacleAnimation obstacleAnimation;


    [Header ("애니메이션 부분")]
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;
    private float Duration;                         // 재생 시간
    public string AnimationName;                    // 재생할 애니메이션

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
            if (obsPositionX <= -22.95f || obsPositionX >= 22.95f)
            {
                if (obsPositionX <= -22.95f) { obsPositionX = -22.95f; }
                else { obsPositionX = 22.95f; }
            }

            if (obsPositionY <= -24.5f || obsPositionY >= 24.5f)
            {
                if (obsPositionY <= -24.5f) { obsPositionY = -24.5f; }
                else { obsPositionY = 24.5f; }
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

        
        if (needToNotFadeOut == true)
        {
            skeletonAnimation = GetComponent<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.AnimationState;
            skeleton = skeletonAnimation.Skeleton;

            // 재생 시간 backUp
            Duration = skeletonAnimation.timeScale;

            // 애니메이션 일시 정지
            spineAnimationState.TimeScale = 0.0f;

            StartCoroutine(PlayAnim_First());
        }

        
        
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
        if (Vector3.Distance(cam.transform.position, obsPositionForCamera) <= 0.1f)
        {
            if (needToNotFadeOut == false)
            {
                StartCoroutine(ObsFadeOut());
            }
            else
            {
                StartCoroutine(PlayAnim());
                StartCoroutine(AfterDestroyThisObs_Delay(1.0f));
            }
            trig_CameraMoveToObs = false;
        }
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

        
        // sound effect play
        eliminationSound.PlayOneShot(soundClip);
        
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

            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("파괴되지 말아야 할 장애물");
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

   IEnumerator ObsFadeOut()
    {
        for (int k = 50; k >= 0; k--)
        {
            for (int i = 0; i < obsChildSpriteRenderers.Length; i++)
            {
                obsChildSpriteRenderers[i].color = new Color(1, 1, 1, k * 0.02f);
            }

            if (k == 0)
            {
                StartCoroutine(AfterDestroyThisObs_Delay(1.0f));
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator PlayAnim()
    {
        // 애니메이션의 재생 시간을 백업한 시간으로 되돌리기
        spineAnimationState.TimeScale = Duration;

        try
        {
            // 애니메이션 재생(코루틴 이용)
            spineAnimationState.SetAnimation(0, "Disappear", false);
        }
        catch
        {
            try
            {
                spineAnimationState.SetAnimation(0, "disappear", false);
            }
            catch
            {
                spineAnimationState.SetAnimation(0, "desappear", false);
            }
            
        }
        
        /*
         * 1 parameter : 트랙
         * 2 parameter : 재생할 애니메이션
         * 3 parameter : loop 여부
        **/
        yield return new WaitForSeconds(Duration);
    }

    IEnumerator PlayAnim_First()
    {
        // 애니메이션의 재생 시간을 백업한 시간으로 되돌리기
        spineAnimationState.TimeScale = Duration;

        // 애니메이션 재생(코루틴 이용)
        spineAnimationState.SetAnimation(0, "idle", true);

        /*
         * 1 parameter : 트랙
         * 2 parameter : 재생할 애니메이션
         * 3 parameter : loop 여부
        **/
        yield return new WaitForSeconds(Duration);
    }


    public void SetDialogAuto(bool val = true) { isDialogAuto = val; }
}
