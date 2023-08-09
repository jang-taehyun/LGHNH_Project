using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ����Ʈ - https://textbox.tistory.com/

public class NPCManager : MonoBehaviour
{
    public GameObject quest;                            //�� NPC�� �����ؾ� �ϴ� ����Ʈ
    private GameObject speechBubble;                    //NPC�� �� ���� ���� �� ���� ��ܿ� ���� �ϴ� ��ǳ��
    private DialogSystem dialogSystem;
    private bool isActive;                              //NPC Ȱ��ȭ ����, �� ��ȭ�ϰ� ���� ��������
    private int npcState;                               //NPC�� ���¿� ���� ����, 0�� ����Ʈ ����, 1�� ����Ʈ �Ϸ�


    private float npcPositionX,npcPositionY;            //ī�޶����� �Ѱ��־�� �ϴ� NPC�� ��ġ ����
    private Vector3 npcPositionForCamera;

    void Start()
    {
        speechBubble = transform.Find("Speech Bubble").gameObject; speechBubble.SetActive(false);
        Debug.Log("�� �������ϴ�");
        dialogSystem = GameObject.Find("DialogSystem").GetComponent<DialogSystem>();

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
            Debug.Log("Ȱ��ȭ ���� ���� �����Դϴ�.");
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
