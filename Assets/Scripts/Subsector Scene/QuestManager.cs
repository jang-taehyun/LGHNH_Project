using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject[] activeCollection;   //활성화시켜야 하는 채집 오브젝트들
    public int itemIndex;                   //퀘스트에서 채집을 요구하는 오브젝트들의 인덱스 값들
    public int requireNum;                  //채집 오브젝트의 요구 수량
    public GameObject testing;

    private int hasNum;                     //채집 오브젝트의 소유 수량, 이는 GameManager와 연계해서 재작성 예정
    //private bool isActive;                  //퀘스트의 활성화 여부
    [SerializeField] private UIManager uiManager;
    [SerializeField] private ProcessManager processManager;



    void Start()
    {

    }

    
    void Update()
    {
        
    }

    public void CheckQuestCondition( )  //퀘스트 완료 조건 판단을 하는 함수. 
    {                                   //조건을 만족했으면 퀘스트 완료 함수를 호출하는 구조.
        if (requireNum == hasNum) 
        {
            Debug.Log("퀘스트 완료");
            uiManager.OffQuestUI();
            processManager.ClearOngoingQuest();
            //isActive = false;
        }
        else { Debug.Log("아직 퀘스트 조건을 충족하지 못했습니다."); }
    }

   
    public void ActivateThisQuest()     //이 퀘스트를 활성화시키는 함수. NPC가 이를 호출하는 구조.
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
