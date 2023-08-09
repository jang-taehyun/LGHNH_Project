using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ConversationManager : MonoBehaviour
{
    private GameObject cam;                     //카메라

    private GameObject conversationUI;          //대화 UI
    private GameObject npcNameUI;               //NPC 이름의 UI
    private GameObject characterImage;          //캐릭터 일러스트 이미지
    private GameObject nextButton;              //다음 대화로 넘어가는 버튼, 투명한 것이 특징

    //Test
    private GameObject conversation;            //진짜 대화 내용, 엑셀로 구현해야 해
    private GameObject forQuest;                //퀘스트 UI 모음집

    //캐릭터 일러스트 이미지의 좌표
    private float characterImage_PosX;  
    private float characterImage_PosY;

    private Vector3 calledNPCPosition;
    private Vector3 correctedPosition;
    private string calledNPCName;

    private bool trig_CamMoveToNPC;              //NPC 위치로 카메라를 이동시키는 작업의 트리거.
    private bool trig_CamMoveForCorrection;      //NPC 위치로 이동한 카메라가 줌 아웃할 때 경계에 위치한 NPC 때문에 카메라 흔들림 현상을 해결하기 위함


    private float questUIWidth;                  //퀘스트UI의 Width와 Height 값. 점점 커지는 이펙트를 위해서 존재한다.
    private float questUIHeight;

    Vector3 vel = Vector3.zero;                  //SmoothDump( ) 함수의 인자로 쓸 목적으로 선언했지, 딱히 무슨 역할을 하는 건 아니야.

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        conversationUI = GameObject.Find("Conversation UI");        conversationUI.SetActive(false);
        conversation = GameObject.Find("Conversation");             //conversation.SetActive(false);
        npcNameUI = GameObject.Find("NPC Name");                    //npcNameUI.SetActive(false);
        nextButton = GameObject.Find("Next Button");                nextButton.SetActive(false);
        characterImage = GameObject.Find("Character Image");        characterImage.SetActive(false);

        trig_CamMoveToNPC = false;

        questUIWidth = conversationUI.GetComponent<RectTransform>().rect.width;
        questUIHeight = conversationUI.GetComponent<RectTransform>().rect.height;

        characterImage_PosX = characterImage.GetComponent<RectTransform>().position.x;
        characterImage_PosY = characterImage.GetComponent<RectTransform>().position.y;

        forQuest = GameObject.Find("For Quest");    
    }

    // Update is called once per frame
    void Update()
    {
        if (trig_CamMoveToNPC) { CameraMoveToNPC(calledNPCPosition); }
        if (trig_CamMoveForCorrection) { CameraMoveForCorrection(); }
    }

    public void StartConversationSetting(Vector3 _npcPositionForCamera, string _npcName)
    {
        if (GameObject.Find("Process Manager").GetComponent<ProcessManager>().IsActiveOngoingQuest()) 
        { forQuest.SetActive(false); }
        
        calledNPCPosition = _npcPositionForCamera;
        calledNPCName = _npcName;
        trig_CamMoveToNPC = true;
        cam.GetComponent<CameraMover_Test>().FocusCamera();

        StartCoroutine(CameraZoomIn());
    }


    public void EndConversationSetting()    //Next Button을 통해서 호출되는 함수
    {
        characterImage.SetActive(false);
        conversationUI.SetActive(false);
        npcNameUI.SetActive(false);
        conversation.SetActive(false);
        nextButton.SetActive(false);

        if (GameObject.Find("Process Manager").GetComponent<ProcessManager>().IsActiveOngoingQuest()) { forQuest.SetActive(true); }

        trig_CamMoveToNPC = false; 
        cam.GetComponent<CameraMover_Test>().FreeCamera();
        StartCoroutine(CameraZoomOut());
        GameObject.Find(calledNPCName).GetComponent<NPCManager>().EndConversation();
        Debug.Log("대화 종료 함수가 호출되었습니다.");
        
    }

    public void CameraMoveToNPC(Vector3 _npcPosition_Camera)
    {
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, _npcPosition_Camera, ref vel, 0.5f);
        if (Vector2.Distance(cam.transform.position, _npcPosition_Camera) <= 0.1f) 
        { 
            trig_CamMoveToNPC = false;
            Debug.Log("성공적으로 trig_CamMoveToNPC 값이 false로 할당되었습니다."); 
        }
    }

    public void CameraMoveForCorrection()
    {
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, correctedPosition, ref vel, 0.25f);
        Debug.Log("카메라 보정을 위해 열심히 이동중.");
        if (Vector2.Distance(cam.transform.position, correctedPosition) <= 0.1f)
        {
            trig_CamMoveForCorrection = false;
            Debug.Log("성공적으로 trig_CamMoveForCorrection 값이 false로 할당되었습니다.");
        }
    }
    

 
    IEnumerator CameraZoomIn()
    {
        
        for (int i = 100; i >= 50; i--)
        {
            cam.GetComponent<Camera>().orthographicSize = i * 0.1f;
            cam.GetComponent<BoxCollider2D>().size = new Vector2(i * 0.1f, i * 0.2f);

            if (i == 70) 
            { 
                conversationUI.SetActive(true); 
                StartCoroutine(ConversWindowPop()); 
            }

            yield return new WaitForSecondsRealtime(0.01f);
            
        }
    }
    IEnumerator CameraZoomOut()
    {
        //NPC가 경계에 위치해 있어서, 위치 보정이 필요한 경우
        {
            if (cam.transform.position.x < -12.5f)
            {
                correctedPosition = cam.transform.position;
                correctedPosition.x = -12.45f;
                trig_CamMoveForCorrection = true;
            }
            if (cam.transform.position.x > 12.5f)
            {
                correctedPosition = cam.transform.position;
                correctedPosition.x = 12.45f;
                trig_CamMoveForCorrection = true;
            }

            if (cam.transform.position.y < -5.5f)
            {
                correctedPosition = cam.transform.position;
                correctedPosition.y = -5.45f;
                trig_CamMoveForCorrection = true;
            }
            if (cam.transform.position.y > 5.5f)
            {
                correctedPosition = cam.transform.position;
                correctedPosition.y = 5.45f;
                trig_CamMoveForCorrection = true;
            }
        }
        

        for (int i = 50; i <= 100; i++)
        {
            cam.GetComponent<Camera>().orthographicSize = i * 0.1f;
            cam.GetComponent<BoxCollider2D>().size = new Vector2(i * 0.1f, i * 0.2f);

            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator ConversWindowPop()
    {
        for (int i = 0; i <= 100; i++)
        {
            conversationUI.GetComponent<RectTransform>().sizeDelta = new Vector2(questUIWidth + questUIWidth *i*0.01f, 
                                                                                 questUIHeight + questUIHeight * i * 0.01f);
            if (i == 100)
            {
                npcNameUI.SetActive(true); 
                characterImage.SetActive(true);
                for (int k = 0; k < characterImage.transform.childCount; k++)
                {
                    characterImage.transform.GetChild(k).gameObject.SetActive(false);
                }
                characterImage.transform.Find(calledNPCName).gameObject.SetActive(true);
                StartCoroutine(NameFadeIn());
                StartCoroutine(CharacterImageUp());
            }
            yield return new WaitForSecondsRealtime(0.001f);
        }
    }
    IEnumerator NameFadeIn()
    {
        npcNameUI.GetComponent<TextMeshProUGUI>().text = calledNPCName;

        for (float a = 0; a <= 1; a += 0.01f)
        {
            npcNameUI.GetComponent<TMP_Text>().color = new Color(0, 0, 0, a);
            yield return new WaitForSeconds(0.001f);
        }
    }
    IEnumerator CharacterImageUp()
    {
        for (int i = 0; i <= 140; i++)
        {
            characterImage.GetComponent<RectTransform>().position = new Vector3(characterImage_PosX, 
                                                                                characterImage_PosY + i, 0);

            if (i == 140) 
            {
                conversation.SetActive(true);
                nextButton.SetActive(true);
                GameObject.Find("DialogSystem").GetComponent<DialogSystem>().UpdateDialog();
            }
            yield return new WaitForSeconds(0.001f);
        }
    }
}
