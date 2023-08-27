using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    private bool isActive;                     //채집 오브젝트의 활성화 여부
    public int index;                          //채집 오브젝트에 해당하는 인덱스
    public GameObject quest;                   //채집 오브젝트를 필요로 하는 퀘스트
    public GameObject playerCollectEffect;    //채집했을 때 나타나는 플레이어의 채집 애니메이션, 지금은 소스가 도착 안해서 GameObject로 선언.

    public AudioSource CollectSoundPlayer;
    public AudioClip CollectSound;

    void Start()
    {
        isActive = false;
        // playerCollectEffect = transform.Find("Catch").gameObject;
        playerCollectEffect.SetActive(false);
    }


    void Update()
    {
        
    }

    
    public void TouchThisCollection()       //채집 오브젝트를 터치했을 때 호출되는 함수.
    {
        if (isActive)
        {
            CollectSoundPlayer.PlayOneShot(CollectSound);

            playerCollectEffect.SetActive(true);
            StartCoroutine(PlayerShow());
            quest.GetComponent<QuestManager>().IncreaseHasNum();
            quest.GetComponent<QuestManager>().CheckQuestCondition();

            isActive = false;
        }
        else { Debug.Log("활성화되지 않은 채집물입니다."); }
        

    }
    
    public void ActivateThisCollection()    //채집 오브젝트를 활성화시키는 함수. 
    { isActive = true; }                    //이 채집 오브젝트를 필요로 하는 퀘스트에서 이를 호출.


    IEnumerator PlayerShow( )               
    {
        yield return new WaitForSeconds(0.0f);
        playerCollectEffect.SetActive(false);
        gameObject.SetActive(false);

    }
}
