using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionManager : MonoBehaviour
{
    private bool isActive;                     //ä�� ������Ʈ�� Ȱ��ȭ ����
    public int index;                          //ä�� ������Ʈ�� �ش��ϴ� �ε���
    public GameObject quest;                   //ä�� ������Ʈ�� �ʿ�� �ϴ� ����Ʈ
    public GameObject playerCollectEffect;    //ä������ �� ��Ÿ���� �÷��̾��� ä�� �ִϸ��̼�, ������ �ҽ��� ���� ���ؼ� GameObject�� ����.

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

    
    public void TouchThisCollection()       //ä�� ������Ʈ�� ��ġ���� �� ȣ��Ǵ� �Լ�.
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
        else { Debug.Log("Ȱ��ȭ���� ���� ä�����Դϴ�."); }
        

    }
    
    public void ActivateThisCollection()    //ä�� ������Ʈ�� Ȱ��ȭ��Ű�� �Լ�. 
    { isActive = true; }                    //�� ä�� ������Ʈ�� �ʿ�� �ϴ� ����Ʈ���� �̸� ȣ��.


    IEnumerator PlayerShow( )               
    {
        yield return new WaitForSeconds(0.0f);
        playerCollectEffect.SetActive(false);
        gameObject.SetActive(false);

    }
}
