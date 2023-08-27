using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManager;

public class ProcessManager : MonoBehaviour
{
    private GameObject ongoingQuest;
    private int phase;
    private bool isSubsectorEnded;
    // private bool isFirst;


    [SerializeField] private int maxPhase;
    [SerializeField] private int subsectorNum;

    [SerializeField] private DialogSystem dialogSystem;
    [SerializeField] private GameObject collections_obj;
    [SerializeField] private GameObject obstacles_obj;
    [SerializeField] private UIManager uiManager;

    public GameObject[] npcs;
    public GameObject[] quests;
    public GameObject[] obstacles;

    // Start is called before the first frame update
    void Start()
    {
        isSubsectorEnded = false;

        Debug.Log("Process Manager start");

        if (GameManager.GameManager.Inst.ReadClearNum() >= maxPhase)
        {
            dialogSystem.gameObject.SetActive(false);
            collections_obj.SetActive(false);
            obstacles_obj.SetActive(false);

        }
        ongoingQuest = null;
        // isFirst = true;
        phase = 0;

        if (dialogSystem.autoStartBranch[0] == false)
        {
            npcs[phase].GetComponent<NPCManager>().ActiveThisNPC();
            //npcs[phase].ActiveThisNPC();
        }

        Debug.Log("Process Manager end");
    }

    // Update is called once per frame
    void Update()
    {
        //if (isFirst && GameObject.Find("DialogSystem").GetComponent<DialogSystem>().autoStartBranch[0] == false)
        //{
        //    isFirst = false;
        //    npcs[phase].ActiveThisNPC();

        //}

    }

    public void SetOngoingQuest(GameObject _quest) { ongoingQuest = _quest; }

    public GameObject GetOngoingQuest() { return ongoingQuest; }

    public bool IsActiveOngoingQuest()
    {
        if (ongoingQuest != null) { return true; }
        else { return false; }
    }

    public void ClearOngoingQuest()
    {
        if (dialogSystem.autoStartBranch[ReadPhase() * 2 + 1] == false)
        {
            npcs[phase].GetComponent<NPCManager>().QuestCleared();
            //npcs[phase].GetComponent<NPCManager>().ActiveThisNPC();
        }
        else
        {

        }


    }
    public void DestroyOngoingObstacle()
    {
        obstacles[phase].GetComponent<ObstacleManager>().DestroyThisObs();
    }

    public void increasePhase()
    {
        if (phase < quests.Length - 1)
        {
            phase++;
            if (dialogSystem.autoStartBranch[phase * 2] == false) { npcs[phase].GetComponent<NPCManager>().ActiveThisNPC(); }
            if (dialogSystem.autoStartBranch[phase * 2] == true) { SetOngoingQuest(quests[phase]); }
        }
        else
        {
            //서브 섹터 씬이 다 클리어된 상태!
            Debug.Log("서브 섹터 내의 모든 퀘스트를 완료했습니다.");
            isSubsectorEnded = true;
            uiManager.OnClearUI();
        }
    }

    public void ActivateQuest()     //NPC에게 받는 퀘스트가 아닌, 자동으로 실행된 스크립트에 의해서 활성화될 퀘스트
    {
        quests[phase].GetComponent<QuestManager>().ActivateThisQuest();
    }


    public int ReadPhase() { return phase; }
    public int ReadSubsectorNum() { return subsectorNum; }
    public bool IsSubsectorEnded() {  return isSubsectorEnded; }

}
