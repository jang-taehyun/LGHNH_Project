using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ConversationManager : MonoBehaviour
{
    private GameObject camera;                  //카메라

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
    private string calledNPCName;

    private bool trig_CamMoveToNPC;              //NPC 위치로 카메라를 이동시키는 작업의 트리거.

    private float questUIWidth;                  //퀘스트UI의 Width와 Height 값. 점점 커지는 이펙트를 위해서 존재한다.
    private float questUIHeight;

    Vector3 vel = Vector3.zero;                  //SmoothDump( ) 함수의 인자로 쓸 목적으로 선언했지, 딱히 무슨 역할을 하는 건 아니야.

    void Start()
    {
        camera = GameObject.Find("Main Camera");
        conversationUI = GameObject.Find("Conversation UI");        conversationUI.SetActive(false);
        conversation = GameObject.Find("Conversation");             conversation.SetActive(false);
        npcNameUI = GameObject.Find("NPC Name");                    npcNameUI.SetActive(false);
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
    }

    public void StartConversationSetting(Vector3 _npcPositionForCamera, string _npcName)
    {
        if (GameObject.Find("Subsector Scene Manager").GetComponent<SubsectorManager>().IsActiveOngoingQuest()) 
        { forQuest.SetActive(false); }
        
        calledNPCPosition = _npcPositionForCamera;
        calledNPCName = _npcName;
        trig_CamMoveToNPC = true;
        camera.GetComponent<CameraMover_Test>().FocusCamera();

        StartCoroutine(CameraZoomIn());
    }


    public void EndConversationSetting()
    {
        characterImage.SetActive(false);
        conversationUI.SetActive(false);
        npcNameUI.SetActive(false);
        conversation.SetActive(false);
        nextButton.SetActive(false);

        if (GameObject.Find("Subsector Scene Manager").GetComponent<SubsectorManager>().IsActiveOngoingQuest()) { forQuest.SetActive(true); }

        trig_CamMoveToNPC = false; 
        camera.GetComponent<CameraMover_Test>().FreeCamera();
        GameObject.Find(calledNPCName).GetComponent<NPCManager>().EndConversation();
        StartCoroutine(CameraZoomOut());
    }

    public void CameraMoveToNPC(Vector3 _npcPosition_Camera)
    {
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, _npcPosition_Camera, ref vel, 0.5f);
    }

    IEnumerator CameraZoomIn()
    {
        
        for (int i = 100; i >= 50; i--)
        {
            camera.GetComponent<Camera>().orthographicSize = i * 0.1f;

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

        for (int i = 50; i <= 100; i++)
        {
            camera.GetComponent<Camera>().orthographicSize = i * 0.1f;

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
            }
            yield return new WaitForSeconds(0.001f);
        }
    }

    public void SetScript(string input)
    {
        conversation.GetComponent<TextMeshProUGUI>().text = input;
    }


}
