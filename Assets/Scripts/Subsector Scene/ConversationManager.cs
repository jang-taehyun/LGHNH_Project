using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ConversationManager : MonoBehaviour
{
    private GameObject camera;                  //ī�޶�

    private GameObject conversationUI;          //��ȭ UI
    private GameObject npcNameUI;               //NPC �̸��� UI
    private GameObject characterImage;          //ĳ���� �Ϸ���Ʈ �̹���
    private GameObject nextButton;              //���� ��ȭ�� �Ѿ�� ��ư, ������ ���� Ư¡

    //Test
    private GameObject conversation;            //��¥ ��ȭ ����, ������ �����ؾ� ��
    private GameObject forQuest;                //����Ʈ UI ������

    //ĳ���� �Ϸ���Ʈ �̹����� ��ǥ
    private float characterImage_PosX;  
    private float characterImage_PosY;

    private Vector3 calledNPCPosition;
    private string calledNPCName;

    private bool trig_CamMoveToNPC;              //NPC ��ġ�� ī�޶� �̵���Ű�� �۾��� Ʈ����.

    private float questUIWidth;                  //����ƮUI�� Width�� Height ��. ���� Ŀ���� ����Ʈ�� ���ؼ� �����Ѵ�.
    private float questUIHeight;

    Vector3 vel = Vector3.zero;                  //SmoothDump( ) �Լ��� ���ڷ� �� �������� ��������, ���� ���� ������ �ϴ� �� �ƴϾ�.

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
