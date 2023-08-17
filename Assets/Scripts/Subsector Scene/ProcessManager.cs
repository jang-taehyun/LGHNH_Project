using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManager;

public class ProcessManager : MonoBehaviour
{
    private GameObject ongoingQuest;
    private int phase;
    private bool isFirst;
    private DialogSystem dialogSystem;

    

    [SerializeField] private int maxPhase;
    [SerializeField] private int subsectorNum;

    public NPCManager[] npcs;
    public GameObject[] quests;
    public GameObject[] obstacles;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.GameManager.Inst.ReadClearNum() >= maxPhase)
        {
            GameObject.Find("DialogSystem").SetActive(false);
            GameObject.Find("Collections").SetActive(false);
            GameObject.Find("Obstacles").SetActive(false);

        }
        ongoingQuest = null;
        isFirst = true;
        phase = 0;

        dialogSystem = GameObject.Find("DialogSystem").GetComponent<DialogSystem>();

        if (GameObject.Find("DialogSystem").GetComponent<DialogSystem>().autoStartBranch[0] == false)
        {
            Debug.Log("�� ����˴ϴ�!");
            npcs[phase].ActiveThisNPC();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isFirst && GameObject.Find("DialogSystem").GetComponent<DialogSystem>().autoStartBranch[0] == false)
        {
            isFirst = false;
            npcs[phase].ActiveThisNPC();
            
        }
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
        npcs[phase].QuestCleared();
    }
    public void DestroyOngoingObstacle()
    {
        obstacles[phase].GetComponent<ObstacleManager>().DestroyThisObs();
    }

    public void increaseOngoingIndex()
    {
        if (phase < quests.Length - 1)
        {
            phase++;
            if (dialogSystem.autoStartBranch[phase * 2] == false) { npcs[phase].GetComponent<NPCManager>().ActiveThisNPC(); }
            if (dialogSystem.autoStartBranch[phase * 2] == true) { SetOngoingQuest(quests[phase]); }
        }
        else
        {
            //���� ���� ���� �� Ŭ����� ����!
            Debug.Log("���� ���� ���� ��� ����Ʈ�� �Ϸ��߽��ϴ�.");
            GameObject.Find("Canvas").GetComponent<UIManager>().OnClearUI();
        }
    }

    public void ActivateQuest()     //NPC���� �޴� ����Ʈ�� �ƴ�, �ڵ����� ����� ��ũ��Ʈ�� ���ؼ� Ȱ��ȭ�� ����Ʈ
    {
        quests[phase].GetComponent<QuestManager>().ActivateThisQuest();
    }


    public int ReadPhase() { return phase; }
    public int ReadSubsectorNum() { return subsectorNum; }

}
