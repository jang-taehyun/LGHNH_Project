using System;                       
using System.Collections;            
using System.Collections.Generic;   
using UnityEngine;                  
using UnityEngine.UI;               
using TMPro;
using Unity.VisualScripting;

public class DialogSystem : MonoBehaviour
{
    private int branch;
    private bool isAuto;
    private bool isQuestClearedAuto;
    public bool[] autoStartBranch;                      //자동으로 시작하는 Branch라면 체크해주세요.

    [SerializeField] private ProcessManager processManager;
    [SerializeField] private DialogDB_Sky dialogDB_sky;
    [SerializeField] private DialogDB_Pumping dialogDB_pumping;
    [SerializeField] private DialogDB_Ice dialogDB_ice;
    [SerializeField] private DialogDB_Lake dialogDB_lake;
    [SerializeField] private DialogDB_Pink dialogDB_pink;

    private int dialogTotalNum;                         //전체 대화 개수
    [SerializeField] private DialogData[] dialogs;      //현재 분기의 대사 목록 배열
    //[SerializeField] private bool isAutoStart = true;   //자동 시작 여부
    private int currentDialogIndex = -1;                //현재 대사 순번

    private float typingSpeed = 0.02f;                   //텍스트 타이핑 효과의 재생속도
    private bool isTypingEffect = false;                //텍스트 타이핑 효과를 재생중인지
    [SerializeField] private GameObject arrow;                           //대화가 다 출력되었을 때 다음 대화로 넘어가도 된다는 화살표

    //-------------------------여기까지가 기존 DialogSystem.cs의 변수들-------------------------

    [SerializeField] private GameObject cam;                     //카메라
    private Camera cam_Camera;
    private BoxCollider2D cam_BoxCollider2D;

    [SerializeField] private GameObject conversationUI;          //대화 UI
    private RectTransform conversationUI_Rect;

    [SerializeField] private GameObject npcNameUI;               //NPC 이름의 UI
    private TextMeshProUGUI npcNameUI_TextMesh;
    private TMP_Text npcNameUI_TMP_Text;

    [SerializeField] private GameObject characterImage;          //캐릭터 일러스트 이미지
    private RectTransform characterImage_Rect;
    [SerializeField] private GameObject nextButton;              //다음 대화로 넘어가는 버튼, 투명한 것이 특징

    //Test
    [SerializeField] private GameObject conversation;            //진짜 대화 내용, 엑셀로 구현해야 해
    [SerializeField] private GameObject forQuest;                //퀘스트 UI 모음집

    //캐릭터 일러스트 이미지의 좌표
    private float characterImage_PosX;
    private float characterImage_PosY;

    //감정 표현에 따른 캐릭터 일러스트 이미지
    [SerializeField] private GameObject feeling_default;
    [SerializeField] private GameObject feeling_joy;
    [SerializeField] private GameObject feeling_sad;
    [SerializeField] private GameObject feeling_tired;

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
        cam_Camera = cam.GetComponent<Camera>();
        cam_BoxCollider2D = cam.GetComponent<BoxCollider2D>(); 

        npcNameUI_TextMesh = npcNameUI.GetComponent<TextMeshProUGUI>();
        npcNameUI_TMP_Text = npcNameUI.GetComponent<TMP_Text>();
        conversationUI_Rect = conversationUI.GetComponent<RectTransform>();
        characterImage_Rect = characterImage.GetComponent<RectTransform>();

        conversationUI.SetActive(false);
        conversation.SetActive(false);
        npcNameUI.SetActive(false);
        nextButton.SetActive(false);
        
        trig_CamMoveToNPC = false;

        isAuto = false;

        questUIWidth = conversationUI.GetComponent<RectTransform>().rect.width;
        questUIHeight = conversationUI.GetComponent<RectTransform>().rect.height;

        feeling_default.SetActive(false);
        feeling_joy.SetActive(false);
        feeling_sad.SetActive(false);
        feeling_tired.SetActive(false);
        characterImage.SetActive(false);
        characterImage_PosX = characterImage.GetComponent<RectTransform>().localPosition.x;
        characterImage_PosY = characterImage.GetComponent<RectTransform>().localPosition.y;

        
        arrow.SetActive(false);
        branch = 1;
        refresh();              //엑셀 값을 받아와서 넣어주기
        Setup();

        
        //branch가 1일 때
        if (autoStartBranch[0])
        { StartConversationSetting_Auto(); }

        
      


    }

    // Update is called once per frame
    void Update()
    {
        if (trig_CamMoveToNPC) { CameraMoveToNPC(calledNPCPosition); }
        if (trig_CamMoveForCorrection) { CameraMoveForCorrection(); }

        /* 테스트하려고 잠깐만든거
        if (Input.GetMouseButtonDown(0))
        {
            UpdateDialog();
        }
        */
    }
    
    public void StartConversationSetting(Vector3 _npcPositionForCamera, string _npcName)
    {
        if (processManager.IsActiveOngoingQuest())
        { forQuest.SetActive(false); }

        calledNPCPosition = _npcPositionForCamera;
        calledNPCName = _npcName;
        trig_CamMoveToNPC = true;
        cam.GetComponent<CameraMover>().FocusCamera();

        StartCoroutine(CameraZoomIn());
        
    }
    public void StartConversationSetting_Auto(bool val = false)
    {
        isAuto = true;
        if (val) { isQuestClearedAuto = true; }
        if (processManager.IsActiveOngoingQuest())
        { forQuest.SetActive(false); }
        
        
        //StartCoroutine(CameraZoomIn());
        conversationUI.SetActive(true);
        StartCoroutine(ConversWindowPop());
        cam.GetComponent<CameraMover>().FocusCamera();
    }

    public void EndConversationSetting()    //Next Button을 통해서 호출되는 함수
    {
        characterImage.SetActive(false);
        conversationUI.SetActive(false);
        npcNameUI.SetActive(false);
        conversation.SetActive(false);
        nextButton.SetActive(false);

        if (processManager.IsActiveOngoingQuest()) { forQuest.SetActive(true); }

        trig_CamMoveToNPC = false;
        cam.GetComponent<CameraMover>().FreeCamera();
        StartCoroutine(CameraZoomOut());
        //어쩔 수 없는 사항, 2023-08-22
        if (null != calledNPCName)
            GameObject.Find(calledNPCName).GetComponent<NPCManager>().EndConversation();
    }
    public void EndConversationSetting_Auto()    //Next Button을 통해서 호출되는 함수
    {
        characterImage.SetActive(false);
        conversationUI.SetActive(false);
        npcNameUI.SetActive(false);
        conversation.SetActive(false);
        nextButton.SetActive(false);

        if (processManager.IsActiveOngoingQuest()) { forQuest.SetActive(true); }
        if (isQuestClearedAuto) { processManager.DestroyOngoingObstacle(); }
        else { processManager.ActivateQuest(); }
        //trig_CamMoveToNPC = false;
        cam.GetComponent<CameraMover>().FreeCamera();
        
        isAuto = false; isQuestClearedAuto = false;
        //StartCoroutine(CameraZoomOut());
        //GameObject.Find(calledNPCName).GetComponent<NPCManager>().EndConversation();

    }

    public void CameraMoveToNPC(Vector3 _npcPosition_Camera)
    {
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, _npcPosition_Camera, ref vel, 0.5f);
        if (Vector2.Distance(cam.transform.position, _npcPosition_Camera) <= 0.1f)
        {
            trig_CamMoveToNPC = false;
        }
    }

    public void CameraMoveForCorrection()
    {
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, correctedPosition, ref vel, 0.25f);

        if (Vector2.Distance(cam.transform.position, correctedPosition) <= 0.1f)
        {
            trig_CamMoveForCorrection = false;
        }
    }

    IEnumerator CameraZoomIn()
    {
        for (int i = 100; i >= 50; i--)
        {
            cam_Camera.orthographicSize = i * 0.1f;
            cam_BoxCollider2D.size = new Vector2(i * 0.1f, i * 0.2f);

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
            cam_Camera.orthographicSize = i * 0.1f;
            cam_BoxCollider2D.size = new Vector2(i * 0.1f, i * 0.2f);

            if (i == 100) { cam_BoxCollider2D.size = new Vector2(11.3f, 20f); }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator ConversWindowPop()
    {
        for (int i = 0; i <= 100; i++)
        {
            conversationUI_Rect.sizeDelta = new Vector2(questUIWidth + questUIWidth * i * 0.01f,
                                                                                 questUIHeight + questUIHeight * i * 0.01f);
            if (i == 100)
            {
                npcNameUI.SetActive(true);
                characterImage.SetActive(true);

                npcNameUI_TextMesh.text = dialogs[0].name;
                OffFeeling();
                if (dialogs[0].expression == "기본") { OffFeeling(); feeling_default.SetActive(true); }
                if (dialogs[0].expression == "기쁨") { OffFeeling(); feeling_joy.SetActive(true); }
                if (dialogs[0].expression == "우울") { OffFeeling(); feeling_sad.SetActive(true); }
                if (dialogs[0].expression == "피곤") { OffFeeling(); feeling_tired.SetActive(true); }


                StartCoroutine(NameFadeIn());
                StartCoroutine(CharacterImageUp());
            }

            yield return new WaitForSecondsRealtime(0.001f);
            
        }
    }
    IEnumerator NameFadeIn()
    {
        for (int i = 0; i <= 100; i++)
        {
            npcNameUI_TMP_Text.color = new Color(0, 0, 0, i * 0.01f);
            yield return new WaitForSeconds(0.001f);
        }
    }
    IEnumerator CharacterImageUp()
    {
        for (int i = 0; i <= 460; i++)
        {
            characterImage_Rect.localPosition = new Vector3(characterImage_PosX, characterImage_PosY + i, 0);


            if (i == 460)
            {
                conversation.SetActive(true);
                UpdateDialog();
            }
            yield return new WaitForSeconds(0.001f);
        }
    }

    private void refresh()          //엑셀 값을 받아오는 작업
    {
        
        int index = 0;

        Array.Resize(ref dialogs, 100);

        int subsectorNum = processManager.ReadSubsectorNum();

        switch (subsectorNum)
        {
            case 0:
                for (int i = 0; i < dialogDB_sky.sky.Count; ++i)
                {
                    if (dialogDB_sky.sky[i].branch == branch)
                    {
                        dialogs[index].name = dialogDB_sky.sky[i].name;
                        dialogs[index].expression = dialogDB_sky.sky[i].expression;
                        dialogs[index].dialogue = dialogDB_sky.sky[i].dialog;

                        index++;
                        dialogTotalNum = index;
                    }
                }
                break;

            case 1:
                for (int i = 0; i < dialogDB_pumping.pumping.Count; ++i)
                {
                    if (dialogDB_pumping.pumping[i].branch == branch)
                    {
                        dialogs[index].name = dialogDB_pumping.pumping[i].name;
                        dialogs[index].expression = dialogDB_pumping.pumping[i].expression;
                        dialogs[index].dialogue = dialogDB_pumping.pumping[i].dialog;

                        index++;
                        dialogTotalNum = index;
                    }
                }
                break;

            case 2:
                for (int i = 0; i < dialogDB_pink.pink.Count; ++i)
                {
                    if (dialogDB_pink.pink[i].branch == branch)
                    {
                        dialogs[index].name = dialogDB_pink.pink[i].name;
                        dialogs[index].expression = dialogDB_pink.pink[i].expression;
                        dialogs[index].dialogue = dialogDB_pink.pink[i].dialog;

                        index++;
                        dialogTotalNum = index;
                    }
                }
                break;
                
            case 3:
                for (int i = 0; i < dialogDB_ice.ice.Count; ++i)
                {
                    if (dialogDB_ice.ice[i].branch == branch)
                    {
                        dialogs[index].name = dialogDB_ice.ice[i].name;
                        dialogs[index].expression = dialogDB_ice.ice[i].expression;
                        dialogs[index].dialogue = dialogDB_ice.ice[i].dialog;

                        index++;
                        dialogTotalNum = index;
                    }
                }
                break;

            case 4:
                for (int i = 0; i < dialogDB_lake.lake.Count; ++i)
                {
                    if (dialogDB_lake.lake[i].branch == branch)
                    {
                        dialogs[index].name = dialogDB_lake.lake[i].name;
                        dialogs[index].expression = dialogDB_lake.lake[i].expression;
                        dialogs[index].dialogue = dialogDB_lake.lake[i].dialog;

                        index++;
                        dialogTotalNum = index;
                    }
                }
                break;
                
        }

        Array.Resize(ref dialogs, dialogTotalNum);
    }

    private void Setup()
    {
        SetActiveObjects(false);
    }

    public void UpdateDialog()
    {
            if (isTypingEffect)
            {
                isTypingEffect = false;
                //타이핑 효과를 중지하고, 현재 대사를 출력한다.
                StopCoroutine("OnTypingText");
                conversation.GetComponent<TextMeshProUGUI>().text = dialogs[currentDialogIndex].dialogue;
                

            }
            else if (!isTypingEffect)
            {



                //대사가 남아있을경우 다음대사 진행
                if (dialogs.Length > currentDialogIndex + 1)
                {
                    SetNextDialog();

                }
                //대사가 더 이상 없을경우 모든 오브젝트를 비활성화하고 true 반환
                else
                {
                    SetActiveObjects(false);
                    if (isAuto)
                    {
                        EndConversationSetting_Auto();
                    }
                    else
                    {
                        EndConversationSetting();
                    }

                    branch++;
                    refresh();
                    currentDialogIndex = -1;

                    npcNameUI.GetComponent<TextMeshProUGUI>().text = dialogs[0].name;

                    if (dialogs[0].expression == "기본")
                    {
                        OffFeeling();
                        feeling_default.SetActive(true);
                    }

                    if (dialogs[0].expression == "기쁨")
                    {
                        OffFeeling();
                        feeling_joy.SetActive(true);
                    }

                    if (dialogs[0].expression == "우울")
                    {
                        OffFeeling();
                        feeling_sad.SetActive(true);
                    }

                    if (dialogs[0].expression == "피곤")
                    {
                        OffFeeling();
                        feeling_tired.SetActive(true);
                    }

                    if (branch <= autoStartBranch.Length && autoStartBranch[branch - 1] == true)
                    {
                        processManager.obstacles[
                                processManager.ReadPhase()]
                            .GetComponent<ObstacleManager>().SetDialogAuto();
                    }
                }
            }


    }

    public void SetNextDialog()
    {
        //다음 대사 진행
        currentDialogIndex++;
        npcNameUI.GetComponent<TextMeshProUGUI>().text = dialogs[currentDialogIndex].name;
        
        if (dialogs[currentDialogIndex].expression == "") { characterImage.SetActive(false); }
        else { characterImage.SetActive(true); }

        if (currentDialogIndex != 0)
        {
            if (dialogs[currentDialogIndex].expression == "기본") { OffFeeling(); feeling_default.SetActive(true); }
            if (dialogs[currentDialogIndex].expression == "기쁨") { OffFeeling(); feeling_joy.SetActive(true); }
            if (dialogs[currentDialogIndex].expression == "우울") { OffFeeling(); feeling_sad.SetActive(true); }
            if (dialogs[currentDialogIndex].expression == "피곤") { OffFeeling(); feeling_tired.SetActive(true); }
        }
        


        conversation.GetComponent<TextMeshProUGUI>().text = dialogs[currentDialogIndex].dialogue;


        StartCoroutine("OnTypingText");
    }

    private void SetActiveObjects(bool visible)
    {
        npcNameUI.SetActive(visible);
        conversation.SetActive(visible);
        arrow.gameObject.SetActive(visible);
        
        //화살표는 대사가 종료되었을때만 활성화하기위해 항상 false
        arrow.SetActive(false);

    }
    
    private IEnumerator OnTypingText()
    {
        int index = 0;
        arrow.SetActive(false);
        //nextButton.SetActive(false);
        isTypingEffect = true;

        //텍스트를 한글자씩 타이핑치듯 재생
        while (index <= dialogs[currentDialogIndex].dialogue.Length)
        {
            conversation.GetComponent<TextMeshProUGUI>().text = dialogs[currentDialogIndex].dialogue.Substring(0, index);
            index++;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;
        arrow.SetActive(true);
        nextButton.SetActive(true);
    }

    private void OffFeeling()
    {
        for (int i = 0; i < characterImage.transform.childCount; i++)
        {
            characterImage.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

}

    [System.Serializable]
    public struct DialogData
    {
        public int speakerIndex;        //이름과 대사를 출력할 현재 DialogSystem의 speakers 배열 순번
        public string name;             //캐릭터이름
        public string expression;       //감정 표현
        [TextArea(3, 10)]
        public string dialogue;         //대사
    }

