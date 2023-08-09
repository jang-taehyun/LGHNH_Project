using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    private int branch;
    [SerializeField] private DialogDB dialogDB;

    private int DialogTotalnum;                         //전체 대화 개수
    //[SerializeField] private Speaker[] speakers;      //대화에 참여하는 캐릭터들의; UI 배열
    [SerializeField] private DialogData[] dialogs;      //현재 분기의 대사 목록 배열
    [SerializeField]
    private bool isAutoStart = true;                    //자동 시작 여부
    private bool isFirst = true;                        //최초 1회만 호출하기 위한 변수
    private int currentDialogIndex = -1;                //현재 대사 순번
    //private int currentSpeakerIndex = 0;                //현재 말을 하는 화자(Speaker)의 speakers 배열 순번
    private float typingSpeed = 0.1f;                   //텍스트 타이핑 효과의 재생속도
    private bool isTypingEffect = false;                //텍스트 타이핑 효과를 재생중인지

    private GameObject objectArrow;              //대사가 완료되었을 때 출력되는 커서 오브젝트

    //--------------------------여기까지가 기존 DialogSystem.cs의 변수들

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
        conversationUI = GameObject.Find("Conversation UI"); conversationUI.SetActive(false);

        conversation = GameObject.Find("Conversation");     //speakers[0].textDialogue = conversation.GetComponent<TextMeshProUGUI>(); 
        conversation.SetActive(false);
        npcNameUI = GameObject.Find("NPC Name");            //speakers[0].textName = npcNameUI.GetComponent<TextMeshProUGUI>(); 
        npcNameUI.SetActive(false);
        nextButton = GameObject.Find("Next Button");        //speakers[0].objectArrow = nextButton; 
        nextButton.SetActive(false);
        characterImage = GameObject.Find("Character Image"); characterImage.SetActive(false);

        trig_CamMoveToNPC = false;

        questUIWidth = conversationUI.GetComponent<RectTransform>().rect.width;
        questUIHeight = conversationUI.GetComponent<RectTransform>().rect.height;

        characterImage_PosX = characterImage.GetComponent<RectTransform>().position.x;
        characterImage_PosY = characterImage.GetComponent<RectTransform>().position.y;

        forQuest = GameObject.Find("For Quest");
        objectArrow = GameObject.Find("Arrow"); objectArrow.SetActive(false);
        branch = 1;
        refresh();          //엑셀 값을 받아와서 넣어주기
        Setup();

        
    }

        private void Awake()
    {
        /*refresh();          //엑셀 값을 받아와서 넣어주기
        Setup();

        branch = 0;
        isActive = false;*/
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
            /*
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
            */
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
            conversationUI.GetComponent<RectTransform>().sizeDelta = new Vector2(questUIWidth + questUIWidth * i * 0.01f,
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

        for (int i = 0; i <= 100; i++)
        {
            npcNameUI.GetComponent<TMP_Text>().color = new Color(0, 0, 0, i*0.01f);
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





    private void refresh()      //엑셀 값을 받아오는 작업
    {
        int index = 0;

        Array.Resize(ref dialogs, 100);
        for (int i = 0; i < dialogDB.Entities.Count; ++i)
        {
            if (dialogDB.Entities[i].branch == branch)
            {
                dialogs[index].name = dialogDB.Entities[i].name;
                dialogs[index].Expression = dialogDB.Entities[i].expression;
                dialogs[index].dialogue = dialogDB.Entities[i].dialog;

                index++;
                DialogTotalnum = index;
            }
            
        }
        Array.Resize(ref dialogs, DialogTotalnum);

    }

    private void Setup()
    {
        SetActiveObjects(false);
    }


    public void UpdateDialog()
    {
        if (isFirst == true)
        {
            //초기화, 캐릭터이미지는 활성화하고, 대사 관련 UI는 모두 비활성화
            Setup();
            //자동재생(isAutoStart=true)으로  설정되어 있으면 첫번째 대사 재생
            if (isAutoStart) SetNextDialog();
            isFirst = false;
        }

        if (true/*Input.GetMouseButtonDown(0)*/)
        {
            if (isTypingEffect == true)
            {
                isTypingEffect = false;
                //타이핑 효과를 중지하고, 현재 대사를 출력한다.
                StopCoroutine("OnTypingText");
                //speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;
                
                //return false;
            }
            
            //대사가 남아있을경우 다음대사 진행
            if(dialogs.Length > currentDialogIndex + 1) 
            {
                SetNextDialog();
                
            }
            //대사가 더 이상 없을경우 모든 오브젝트를 비활성화하고 true 반환
            else
            {
                SetActiveObjects(false);

                EndConversationSetting();
                branch++;
                refresh();
                currentDialogIndex = -1;

            }
        }

        //return false;
    }

    public void SetNextDialog()
    {
        //이전 화자의 대화 관련 오브젝트 비활성화
        //SetActiveObjects(speakers[currentSpeakerIndex], false);
        //다음 대사 진행
        currentDialogIndex++;
        //현재 화자 순번 설정
        //currentSpeakerIndex = dialogs[currentDialogIndex].speakerIndex;
        //현재 화자의 대화 관련 오브젝트 활성화
        //SetActiveObjects(speakers[currentSpeakerIndex], true);
        npcNameUI.SetActive(true);
        conversation.SetActive(true);
        //현재 화자 이름,텍스트 설정
        npcNameUI.GetComponent<TextMeshProUGUI>().text = dialogs[currentDialogIndex].name;
        //현재 화자의 대사 텍스트 설정
        conversation.GetComponent<TextMeshProUGUI>().text = dialogs[currentDialogIndex].dialogue;
        StartCoroutine("OnTypingText");
    }

    private void SetActiveObjects(bool visible)
    {
        npcNameUI.SetActive(visible);
        conversation.SetActive(visible);
        objectArrow.gameObject.SetActive(visible);
        
        //화살표는 대사가 종료되었을때만 활성화하기위해 항상 false
        objectArrow.SetActive(false);

    }

    
    private IEnumerator OnTypingText()
    {
        int index = 0;
        objectArrow.SetActive(false);
        isTypingEffect = true;
        //텍스트를 한글자씩 타이핑치듯 재생
        while (index <= dialogs[currentDialogIndex].dialogue.Length)
        {
            conversation.GetComponent<TextMeshProUGUI>().text = dialogs[currentDialogIndex].dialogue.Substring(0, index);
            index++;
            yield return new WaitForSeconds(typingSpeed);

        }

        isTypingEffect = false;
        //대사가 완료되었을때 출력되는 커서 활성화
        objectArrow.SetActive(true);
    }
    
}

    //[System.Serializable]
    /*public struct Speaker
    {
        public TextMeshProUGUI textName;            //현재 대사중인 캐릭터 이름 출력 Text UI
        public TextMeshProUGUI textDialogue;        //현재 대사 출력 Text UI
       
    }*/

    [System.Serializable]
    public struct DialogData
    {
        public int speakerIndex;        //이름과 대사를 출력할 현재 DialogSystem의 speakers 배열 순번
        public string name;             //캐릭터이름
        public string Expression;
        [TextArea(3, 10)]
        public string dialogue;         //대사
    }
    

