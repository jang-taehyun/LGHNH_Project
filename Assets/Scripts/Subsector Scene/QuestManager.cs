using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject[] activeCollection;   //활성화시켜야 하는 채집 오브젝트들
    public int[] itemIndices;               //퀘스트에서 채집을 요구하는 오브젝트들의 인덱스 값들
    public int[] requireNums;               //채집 오브젝트의 요구 수량

    private int[] hasNums;                  //채집 오브젝트의 소유 수량, 이는 GameManager와 연계해서 재작성 예정
    private bool isActive;                  //퀘스트의 활성화 여부

    private GameObject questUI;              //퀘스트 UI
    private GameObject collectionImage;      //채집해야 하는 채집 오브젝트의 이미지

    void Start()
    {
        isActive = false;
        hasNums = new int[itemIndices.Length]; //이후에 수정될 여지가 있음

        questUI = GameObject.Find("Quest UI");                 questUI.SetActive(false);
        collectionImage = GameObject.Find("Collection Image"); collectionImage.SetActive(false);

    }

    
    void Update()
    {
        
    }

    public bool CheckQuestCondition( )  //퀘스트 완료 조건 판단을 하는 함수. 
    {                                   //조건을 만족했으면 퀘스트 완료 함수를 호출하는 구조.
        bool isCleared = true;

        for (int i = 0; i < itemIndices.Length; i++)
        {
            if (requireNums[i] != hasNums[i]) { isCleared = false; break; } 
        }
        
        if (isCleared) { ClearThisQuest(); }
        return isCleared;
    }

   
    public void ActivateThisQuest()     //이 퀘스트를 활성화시키는 함수. NPC가 이를 호출하는 구조.
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

    private void ClearThisQuest( )        //퀘스트를 클리어했을 때 호출해야 하는 함수. 
    {
        Debug.Log("퀘스트 완료");
        isActive = false;
    }

    public void IncreaseHasNum(int index) //퀘스트에서 요구하는 오브젝트의 현재 소유 개수를 증가시키는 함수. 
    {                                     //이것도 나중에 GameManager와 연계해서 재작성해야 한다.
        int i = -1;
        for (int k = 0; k < itemIndices.Length; k++)
        {
            if (itemIndices[k] == index) { i = k; }
        }

        if (i == -1) { Debug.Log("현재 퀘스트에서 요구하지 않는 인덱스이거나, 뭔가가 잘못되었습니다."); }
        else { hasNums[i]++; Debug.Log(index + "번째 아이템의 개수가 성공적으로 1 증가했습니다."); }

    }

    public bool isThisActive( ) { return isActive; }
}
