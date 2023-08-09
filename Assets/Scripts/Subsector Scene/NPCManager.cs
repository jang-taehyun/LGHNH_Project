using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//참고 사이트 - https://textbox.tistory.com/

public class NPCManager : MonoBehaviour
{
    public GameObject quest;                            //이 NPC가 전달해야 하는 퀘스트
    private GameObject speechBubble;                    //NPC가 할 말이 있을 때 우측 상단에 떠야 하는 말풍선
    private DialogSystem dialogSystem;
    private bool isActive;                              //NPC 활성화 여부, 즉 대화하고 싶은 상태인지
    private int npcState;                               //NPC의 상태에 관한 변수, 0은 퀘스트 수락, 1은 퀘스트 완료


    private float npcPositionX,npcPositionY;            //카메라한테 넘겨주어야 하는 NPC의 위치 정보
    private Vector3 npcPositionForCamera;

    void Start()
    {
        speechBubble = transform.Find("Speech Bubble").gameObject; speechBubble.SetActive(false);
        Debug.Log("잘 꺼졌습니다");
        dialogSystem = GameObject.Find("DialogSystem").GetComponent<DialogSystem>();

        npcPositionX = transform.position.x;
        npcPositionY = transform.position.y;
        npcPositionForCamera = new Vector3(npcPositionX, npcPositionY, -10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TouchNPC( )     //NPC를 터치했을 때 TouchManger에서 호출시키는 함수.
    {
        if (isActive)
        {
            if (npcState == 0)
            {
                speechBubble.SetActive(false);
                dialogSystem.StartConversationSetting(npcPositionForCamera, gameObject.name);
            }
            else if (npcState == 1)
            {
                speechBubble.SetActive(false);
                dialogSystem.StartConversationSetting(npcPositionForCamera, gameObject.name);
            }
            
        }
        else
        {
            Debug.Log("활성화 되지 않은 상태입니다.");
        }
        
    }
     
    public void EndConversation( )
    {
        if (npcState == 0) 
        {
            quest.GetComponent<QuestManager>().ActivateThisQuest();
            isActive = false;
        }
        else if (npcState == 1)
        {
            isActive = false;
            GameObject.Find("Process Manager").GetComponent<ProcessManager>().DestroyOngoingObstacle();
        }
    }

    public void QuestCleared( ) 
    {
        isActive = true; 
        npcState = 1; 
        speechBubble.SetActive(true); 
    }

    public void ActiveThisNPC( ) 
    { 
        isActive = true; 
        speechBubble.SetActive(true); 
    }
}
