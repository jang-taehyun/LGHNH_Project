using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Animation obsEliminationAnimation;            //��ֹ��� �����ϴ� �ִϸ��̼�
    public AudioSource eliminationSound;                 //��ֹ��� ������ �� ���� ������ �Ҹ�

    [SerializeField] private GameObject cameraLimitRect;                  //ī�޶� ���� ����
    [SerializeField] private GameObject cam;                              //ī�޶�
    [SerializeField] private Transform center;
    [SerializeField] private ProcessManager processManager;
    [SerializeField] private DialogSystem dialogSystem;
    private Vector3 vel;

    private float obsPositionX, obsPositionY;            //ī�޶����� �Ѱ��־�� �ϴ� NPC�� ��ġ ����
    private Vector3 obsPositionForCamera;
    private Vector3 previousCamPosition;
    private bool trig_CameraMoveToObs;


    private bool isDialogAuto;

    void Start()
    {
        trig_CameraMoveToObs = false;
        cameraLimitRect.SetActive(false);
        Debug.Log(transform.Find("Camera Limit").gameObject);
        vel = Vector3.zero;

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
        Debug.Log("ī�޶� ���� ��ֹ� ��ġ: " + obsPositionForCamera + " " + gameObject.name);
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

        Debug.Log("����� �� ���� ����˴ϴ�."); 
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
        processManager.increaseOngoingIndex();
        if (isDialogAuto)
        {
            dialogSystem.StartConversationSetting_Auto();
            isDialogAuto = false;
        }
        
        gameObject.SetActive(false);
    }

    public void SetDialogAuto() { isDialogAuto = true; }
}
