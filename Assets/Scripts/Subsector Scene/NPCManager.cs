using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//참고 사이트 - https://textbox.tistory.com/

public class NPCManager : MonoBehaviour
{
    public GameObject quest;                            //이 NPC가 전달해야 하는 퀘스트
    public GameObject speechBubble;                     //NPC가 할 말이 있을 때 우측 상단에 떠야 하는 말풍선
    private ConversationManager conversationManager;    //얘는 구현을 다시 생각해 봐야 겠다.

    private bool isFirst;                               //첫 대화인가?
    private bool isQuestCleared;                        //퀘스트가 완료된 상태에서의 대화인가?
    private bool isActive;                              //활성화가 되었나?

    private float npcPositionX,npcPositionY;            //카메라한테 넘겨주어야 하는 NPC의 위치 정보
    private Vector3 npcPositionForCamera;

    void Start()
    {
        conversationManager = GameObject.Find("Conversation Manager").GetComponent<ConversationManager>();

        isFirst = true;
        isQuestCleared = false;

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
            if (isFirst)
            {
                speechBubble.SetActive(false);
                conversationManager.StartConversationSetting(npcPositionForCamera, gameObject.name);
                conversationManager.SetScript("안녕하세요. 피카츄입니다. 여기에 대사가 출력됩니다. 퀘스트도 드립니다.");
            }
            else
            {
                if (!isQuestCleared)
                {
                    conversationManager.StartConversationSetting(npcPositionForCamera, gameObject.name);
                    conversationManager.SetScript("현재 퀘스트 진행중입니다.");
                }
                else
                {
                    speechBubble.SetActive(false);
                    conversationManager.StartConversationSetting(npcPositionForCamera, gameObject.name);
                    conversationManager.SetScript("퀘스트 완료 메세지가 출력됩니다. 이 메세지는 한 번만 출력됩니다.");
                }
            }
            
        }
        else
        {
            Debug.Log("활성화 되지 않은 상태입니다.");
        }
        
    }
     
    public void EndConversation( )
    {
        if (isFirst) 
        {
            quest.GetComponent<QuestManager>().ActivateThisQuest();
            
            isFirst = false; 
        }
    }

    public void QuestCleared( ) { isQuestCleared = true; }
    public void ActiveThisNPC( ) { isActive = true; }
}
