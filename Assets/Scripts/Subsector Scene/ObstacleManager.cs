using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public Animation obsEliminationAnimation;            //��ֹ��� �����ϴ� �ִϸ��̼�
    public AudioSource eliminationSound;                 //��ֹ��� ������ �� ���� ������ �Ҹ�

    private GameObject cameraLimitRect;                  //ī�޶� ���� ����
    private GameObject cam;                              //ī�޶�
    private Vector3 vel;

    private float obsPositionX, obsPositionY;            //ī�޶����� �Ѱ��־�� �ϴ� NPC�� ��ġ ����
    private Vector3 obsPositionForCamera;
    private Vector3 previousCamPosition;
    private bool trig_CameraMoveToObs;



    void Start()
    {
        trig_CameraMoveToObs = false;
        cameraLimitRect = transform.Find("Camera Limit").gameObject;
        cam = GameObject.Find("Main Camera");
        vel = Vector3.zero;

        obsPositionX = transform.position.x;
        obsPositionY = transform.position.y;

        //��ֹ��� �� ���� �پ� ������ ī�޶� ������ ������ �ذ��ϱ� ����.
        if (transform.position.y <= -5.5f || transform.position.y >= 5.5f)
        {
            if (transform.position.y < 0) { obsPositionY = -5.5f; }
            else { obsPositionY = 5.5f; }

        }
        if (transform.position.x <= -12.5f || transform.position.x >= 12.5f)
        {
            if (transform.position.x < 0) { obsPositionX = -12.5f; }
            else { obsPositionX = -12.5f; }
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
        StartCoroutine(AfterDestroyThisObs_Delay(3.0f));
    }
    
    IEnumerator AfterDestroyThisObs_Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        trig_CameraMoveToObs = false;
        cam.transform.position = previousCamPosition;
        GameObject.Find("Process Manager").GetComponent<ProcessManager>().increaseOngoingIndex();
        gameObject.SetActive(false);
    }
}
