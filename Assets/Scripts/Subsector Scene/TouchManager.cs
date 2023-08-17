using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    private bool canTouch;

    // Start is called before the first frame update
    void Start()
    {
        canTouch = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("현재 canTouch의 상태" + canTouch);
        //마우스 클릭 시
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                //마우스로 클릭한 좌표 값을 가져오기
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                Vector3 dir = new Vector3(0, 0, 1);

                //해당 좌표에 있는 오브젝트 찾기
                RaycastHit hit;
                Physics.Raycast(pos, dir, out hit, 100f);

                //Hit한 오브젝트의 Collider에 접촉
                if (hit.collider != null)
                {
                    GameObject click_obj = hit.transform.gameObject;

                    if (click_obj.tag == "NPC" && canTouch)
                    {
                        GameObject.Find(click_obj.name).GetComponent<NPCManager>().TouchNPC();
                    }

                    else if (click_obj.tag == "Collection")
                    {
                        GameObject.Find(click_obj.name).GetComponent<CollectionManager>().TouchThisCollection();
                    }
                }

            }
            
        }
    }

    public void OnTouch() { canTouch = true; }
    public void OffTouch() { canTouch = false; }
}
