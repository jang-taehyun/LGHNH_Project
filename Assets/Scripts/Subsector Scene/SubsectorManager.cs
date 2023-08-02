using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubsectorManager : MonoBehaviour
{
    private GameObject ongoingQuest;
    public GameObject[] npcs;
    public GameObject[] quests;

    // Start is called before the first frame update
    void Start()
    {
        ongoingQuest = null;
        npcs[0].GetComponent<NPCManager>().ActiveThisNPC();
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


}
