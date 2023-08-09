using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManager;

public class ProcessManager : MonoBehaviour
{
    private GameObject ongoingQuest;
    private int ongoingIndex;
    public GameObject[] npcs;
    public GameObject[] obstacles;

    // Start is called before the first frame update
    void Start()
    {
        ongoingQuest = null;
        ongoingIndex = 0;
        npcs[ongoingIndex].GetComponent<NPCManager>().ActiveThisNPC();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetOngoingQuest(GameObject _quest) { ongoingQuest = _quest; }

    public GameObject GetOngoingQuest( ) { return ongoingQuest; }

    public bool IsActiveOngoingQuest( ) 
    {
        if ( ongoingQuest != null ) { return true; }
        else { return false; }
    }

    public void ClearOngoingQuest()
    {
        npcs[ongoingIndex].GetComponent<NPCManager>().QuestCleared();
    }
    public void DestroyOngoingObstacle()
    {
        obstacles[ongoingIndex].GetComponent<ObstacleManager>().DestroyThisObs();
    }

    public void increaseOngoingIndex()
    {
        if (ongoingIndex < npcs.Length - 1)
        {
            ongoingIndex++;
            npcs[ongoingIndex].GetComponent<NPCManager>().ActiveThisNPC();
        }
        else
        {
            //서브 섹터 씬이 다 클리어된 상태!
            Debug.Log("서브 섹터 내의 모든 퀘스트를 완료했습니다.");
            GameObject.Find("Canvas").GetComponent<UIManager>().OnClearUI();
        }
        
    }

}
