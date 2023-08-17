using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ����Ʈ - https://textbox.tistory.com/

public class NPCManager : MonoBehaviour
{
    public GameObject quest;                            //�� NPC�� �����ؾ� �ϴ� ����Ʈ
    private GameObject speechBubble;                 //NPC�� �� ���� ���� �� ���� ��ܿ� ���� �ϴ� ��ǳ��
    private DialogSystem dialogSystem;
    private bool isActive;                              //NPC Ȱ��ȭ ����, �� ��ȭ�ϰ� ���� ��������
    private int npcState;                               //NPC�� ���¿� ���� ����, 0�� ����Ʈ ����, 1�� ����Ʈ �Ϸ�


    private float npcPositionX, npcPositionY;            //ī�޶����� �Ѱ��־�� �ϴ� NPC�� ��ġ ����
    private Vector3 npcPositionForCamera;

    void Start()
    {
        Debug.Log("NPC Manager start");
        // speechBubble = transform.Find("Speech Bubble").gameObject.GetComponent<SpriteRenderer>();

        speechBubble = transform.GetChild(0).gameObject;
        speechBubble.SetActive(false);

        dialogSystem = GameObject.Find("DialogSystem").GetComponent<DialogSystem>();

        npcPositionX = transform.position.x;
        npcPositionY = transform.position.y;
        npcPositionForCamera = new Vector3(npcPositionX, npcPositionY, -10);
        Debug.Log("NPC Manager end");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TouchNPC()     //NPC�� ��ġ���� �� TouchManger���� ȣ���Ű�� �Լ�.
    {
        if (isActive)
        {
            if (npcState == 0)
            {
                Debug.Log("���� NPC ���� " + npcState);
                // speechBubble.color = Color.clear;
                speechBubble.SetActive(false);
                GameObject.Find("Touch Manager").GetComponent<TouchManager>().OffTouch();
                dialogSystem.StartConversationSetting(npcPositionForCamera, gameObject.name);
            }
            else if (npcState == 1)
            {
                Debug.Log("���� NPC ���� " + npcState);
                // speechBubble.color = Color.clear;
                speechBubble.SetActive(false);
                GameObject.Find("Touch Manager").GetComponent<TouchManager>().OffTouch();
                dialogSystem.StartConversationSetting(npcPositionForCamera, gameObject.name);
            }

        }
        else
        {
            Debug.Log("Ȱ��ȭ ���� ���� �����Դϴ�.");
        }

    }

    public void EndConversation()
    {
        if (npcState == 0)
        {
            quest.GetComponent<QuestManager>().ActivateThisQuest();
            GameObject.Find("Touch Manager").GetComponent<TouchManager>().OnTouch();
            isActive = false;
        }
        else if (npcState == 1)
        {
            isActive = false;
            GameObject.Find("Touch Manager").GetComponent<TouchManager>().OnTouch();
            GameObject.Find("Process Manager").GetComponent<ProcessManager>().DestroyOngoingObstacle();
        }
    }

    public void QuestCleared()
    {
        isActive = true;
        npcState = 1;
        // speechBubble.color = Color.white;

        speechBubble.SetActive(true);
    }

    public void ActiveThisNPC()
    {
        isActive = true;
        //if (speechBubble == null)
        //{
        //    speechBubble.color = Color.white;
        //}
        //else
        //{
        //    speechBubble.color = Color.white;
        //}

        Debug.Log(this.name);
        speechBubble.SetActive(true);
    }
}
