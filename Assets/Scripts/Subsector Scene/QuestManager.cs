using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject[] activeCollection;   //Ȱ��ȭ���Ѿ� �ϴ� ä�� ������Ʈ��
    public int[] itemIndices;               //����Ʈ���� ä���� �䱸�ϴ� ������Ʈ���� �ε��� ����
    public int[] requireNums;               //ä�� ������Ʈ�� �䱸 ����

    private int[] hasNums;                  //ä�� ������Ʈ�� ���� ����, �̴� GameManager�� �����ؼ� ���ۼ� ����
    private bool isActive;                  //����Ʈ�� Ȱ��ȭ ����

    private GameObject questUI;              //����Ʈ UI
    private GameObject collectionImage;      //ä���ؾ� �ϴ� ä�� ������Ʈ�� �̹���

    void Start()
    {
        isActive = false;
        hasNums = new int[itemIndices.Length]; //���Ŀ� ������ ������ ����

        questUI = GameObject.Find("Quest UI");                 questUI.SetActive(false);
        collectionImage = GameObject.Find("Collection Image"); collectionImage.SetActive(false);

    }

    
    void Update()
    {
        
    }

    public bool CheckQuestCondition( )  //����Ʈ �Ϸ� ���� �Ǵ��� �ϴ� �Լ�. 
    {                                   //������ ���������� ����Ʈ �Ϸ� �Լ��� ȣ���ϴ� ����.
        bool isCleared = true;

        for (int i = 0; i < itemIndices.Length; i++)
        {
            if (requireNums[i] != hasNums[i]) { isCleared = false; break; } 
        }
        
        if (isCleared) { ClearThisQuest(); }
        return isCleared;
    }

   
    public void ActivateThisQuest()     //�� ����Ʈ�� Ȱ��ȭ��Ű�� �Լ�. NPC�� �̸� ȣ���ϴ� ����.
    {
        for (int i = 0; i < activeCollection.Length; i++)
        {
            activeCollection[i].GetComponent<CollectionManager>().ActivateThisCollection();
        }

        isActive = true;
        questUI.SetActive(true);
        collectionImage.SetActive(true);
        GameObject.Find("Subsector Scene Manager").GetComponent<SubsectorManager>().SetOngoingQuest(gameObject);
    }

    private void ClearThisQuest( )        //����Ʈ�� Ŭ�������� �� ȣ���ؾ� �ϴ� �Լ�. 
    {
        Debug.Log("����Ʈ �Ϸ�");
        isActive = false;
    }

    public void IncreaseHasNum(int index) //����Ʈ���� �䱸�ϴ� ������Ʈ�� ���� ���� ������ ������Ű�� �Լ�. 
    {                                     //�̰͵� ���߿� GameManager�� �����ؼ� ���ۼ��ؾ� �Ѵ�.
        int i = -1;
        for (int k = 0; k < itemIndices.Length; k++)
        {
            if (itemIndices[k] == index) { i = k; }
        }

        if (i == -1) { Debug.Log("���� ����Ʈ���� �䱸���� �ʴ� �ε����̰ų�, ������ �߸��Ǿ����ϴ�."); }
        else { hasNums[i]++; Debug.Log(index + "��° �������� ������ ���������� 1 �����߽��ϴ�."); }

    }

    public bool isThisActive( ) { return isActive; }
}
