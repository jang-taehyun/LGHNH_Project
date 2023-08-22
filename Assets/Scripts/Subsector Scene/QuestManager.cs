using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject[] activeCollection;   //Ȱ��ȭ���Ѿ� �ϴ� ä�� ������Ʈ��
    public int itemIndex;                   //����Ʈ���� ä���� �䱸�ϴ� ������Ʈ���� �ε��� ����
    public int requireNum;                  //ä�� ������Ʈ�� �䱸 ����
    public GameObject testing;

    private int hasNum;                     //ä�� ������Ʈ�� ���� ����, �̴� GameManager�� �����ؼ� ���ۼ� ����
    //private bool isActive;                  //����Ʈ�� Ȱ��ȭ ����
    [SerializeField] private UIManager uiManager;
    [SerializeField] private ProcessManager processManager;



    void Start()
    {

    }

    
    void Update()
    {
        
    }

    public void CheckQuestCondition( )  //����Ʈ �Ϸ� ���� �Ǵ��� �ϴ� �Լ�. 
    {                                   //������ ���������� ����Ʈ �Ϸ� �Լ��� ȣ���ϴ� ����.
        if (requireNum == hasNum) 
        {
            Debug.Log("����Ʈ �Ϸ�");
            uiManager.OffQuestUI();
            processManager.ClearOngoingQuest();
            //isActive = false;
        }
        else { Debug.Log("���� ����Ʈ ������ �������� ���߽��ϴ�."); }
    }

   
    public void ActivateThisQuest()     //�� ����Ʈ�� Ȱ��ȭ��Ű�� �Լ�. NPC�� �̸� ȣ���ϴ� ����.
    {
        for (int i = 0; i < activeCollection.Length; i++)
        {
            activeCollection[i].GetComponent<CollectionManager>().ActivateThisCollection();
        }

        //isActive = true;
        // GameObject.Find("Canvas").GetComponent<UIManager>().OnQuestUI();
        testing.GetComponent<UIManager>().OnQuestUI();
        uiManager.SetRequireNumText(requireNum - hasNum);
        processManager.SetOngoingQuest(gameObject);
    }

    public void IncreaseHasNum() 
    { 
        hasNum++;
        uiManager.SetRequireNumText(requireNum - hasNum);
    }
}
