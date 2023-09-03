using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Animation obsEliminationAnimation;            //��ֹ��� �����ϴ� �ִϸ��̼�
    public AudioSource eliminationSound;                 //��ֹ��� ������ �� ���� ������ �Ҹ�(player)
    public AudioClip soundClip;                          // sound effect
    

    [SerializeField] private GameObject cameraLimitRect;                  //ī�޶� ���� ����
    [SerializeField] private GameObject cam;                              //ī�޶�
    private CameraMover cam_CameraMover;
    [SerializeField] private Transform center;
    [SerializeField] private ProcessManager processManager;
    [SerializeField] private DialogSystem dialogSystem;
    private Vector3 vel;

    private float obsPositionX, obsPositionY;            //ī�޶����� �Ѱ��־�� �ϴ� NPC�� ��ġ ����
    private Vector3 obsPositionForCamera;
    private Vector3 previousCamPosition;
    private bool trig_CameraMoveToObs;
    public bool isNotMovetoObs;
    public bool needToNotFadeOut;



    private bool isDialogAuto;

    public Vector3 NPCPosition;

    [Header("��ֹ� �ı��� ���� SpriteRenders")]
    public SpriteRenderer[] obsChildSpriteRenderers;

    private ObstacleAnimation obstacleAnimation;


    [Header ("�ִϸ��̼� �κ�")]
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;
    private float Duration;                         // ��� �ð�
    public string AnimationName;                    // ����� �ִϸ��̼�

    void Start()
    {
        trig_CameraMoveToObs = false;
        //cameraLimitRect.SetActive(false); Debug.Log(transform.Find("Camera Limit").gameObject);
        vel = Vector3.zero;
        cam_CameraMover = cam.GetComponent<CameraMover>();

        obsPositionX = center.position.x;
        obsPositionY = center.position.y; 
        center.gameObject.SetActive(false);

        //��ֹ��� �� ���� �پ� ������ ī�޶� ������ ������ �ذ��ϱ� ����.
        //�ֶ� ��Ÿ�� ȣ��(subsectorNum = 4)�� �� ũ�Ⱑ �޶� Ư���� ��ġ�� �ʿ��ϴ�.
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

            // ��� �ð� backUp
            Duration = skeletonAnimation.timeScale;

            // �ִϸ��̼� �Ͻ� ����
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
        Debug.Log("ī�޶� �̵���~");
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, obsPositionForCamera, ref vel, 0.5f);
    }

    /* (2023.08.06�� ��������) ���� ���� �� �� ��ֹ��� �ı��� �� Ȯ���Ű�� �� �� ������,
       �ᱹ�� ��ֹ� ����/�ܾƿ��� �� �ϱ�� �ߴ�. �ٵ� Ȥ�� ���� �ڵ�� ���� ���Ҵ�. */
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

            //���꼽�Ͱ� ������ �ʾ��� ��쿡�� ����
            if (processManager.IsSubsectorEnded() == false)
            {
                Debug.Log("���� ReadPhase�� �� : " + processManager.ReadPhase());

                //ī�޶� �� ���� ���� NPC���� �� �ص� �ȴٴ� ��
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
            Debug.Log("�ı����� ���ƾ� �� ��ֹ�");
            DontDestroyObsSetting();
        }

        
    }

    private void DontDestroyObsSetting()
    {
        trig_CameraMoveToObs = false;
        cam.GetComponent<CameraMover>().FreeCamera();
        //cam.transform.position = previousCamPosition;

        processManager.increasePhase();

        //���꼽�Ͱ� ������ �ʾ��� ��쿡�� ����
        if (processManager.IsSubsectorEnded() == false)
        {
            Debug.Log("���� ReadPhase�� �� : " + processManager.ReadPhase());

            //ī�޶� �� ���� ���� NPC���� �� �ص� �ȴٴ� ��
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
        // �ִϸ��̼��� ��� �ð��� ����� �ð����� �ǵ�����
        spineAnimationState.TimeScale = Duration;

        try
        {
            // �ִϸ��̼� ���(�ڷ�ƾ �̿�)
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
         * 1 parameter : Ʈ��
         * 2 parameter : ����� �ִϸ��̼�
         * 3 parameter : loop ����
        **/
        yield return new WaitForSeconds(Duration);
    }

    IEnumerator PlayAnim_First()
    {
        // �ִϸ��̼��� ��� �ð��� ����� �ð����� �ǵ�����
        spineAnimationState.TimeScale = Duration;

        // �ִϸ��̼� ���(�ڷ�ƾ �̿�)
        spineAnimationState.SetAnimation(0, "idle", true);

        /*
         * 1 parameter : Ʈ��
         * 2 parameter : ����� �ִϸ��̼�
         * 3 parameter : loop ����
        **/
        yield return new WaitForSeconds(Duration);
    }


    public void SetDialogAuto(bool val = true) { isDialogAuto = val; }
}
