using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ����Ʈ - https://textbox.tistory.com/

public class NPCManager : MonoBehaviour
{
    public GameObject quest;                            //�� NPC�� �����ؾ� �ϴ� ����Ʈ
    public GameObject speechBubble;                     //NPC�� �� ���� ���� �� ���� ��ܿ� ���� �ϴ� ��ǳ��
    private ConversationManager conversationManager;    //��� ������ �ٽ� ������ ���� �ڴ�.

    private bool isFirst;                               //ù ��ȭ�ΰ�?
    private bool isQuestCleared;                        //����Ʈ�� �Ϸ�� ���¿����� ��ȭ�ΰ�?
    private bool isActive;                              //Ȱ��ȭ�� �Ǿ���?

    private float npcPositionX,npcPositionY;            //ī�޶����� �Ѱ��־�� �ϴ� NPC�� ��ġ ����
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

    public void TouchNPC( )     //NPC�� ��ġ���� �� TouchManger���� ȣ���Ű�� �Լ�.
    {
        if (isActive)
        {
            if (isFirst)
            {
                speechBubble.SetActive(false);
                conversationManager.StartConversationSetting(npcPositionForCamera, gameObject.name);
                conversationManager.SetScript("�ȳ��ϼ���. ��ī���Դϴ�. ���⿡ ��簡 ��µ˴ϴ�. ����Ʈ�� �帳�ϴ�.");
            }
            else
            {
                if (!isQuestCleared)
                {
                    conversationManager.StartConversationSetting(npcPositionForCamera, gameObject.name);
                    conversationManager.SetScript("���� ����Ʈ �������Դϴ�.");
                }
                else
                {
                    speechBubble.SetActive(false);
                    conversationManager.StartConversationSetting(npcPositionForCamera, gameObject.name);
                    conversationManager.SetScript("����Ʈ �Ϸ� �޼����� ��µ˴ϴ�. �� �޼����� �� ���� ��µ˴ϴ�.");
                }
            }
            
        }
        else
        {
            Debug.Log("Ȱ��ȭ ���� ���� �����Դϴ�.");
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
