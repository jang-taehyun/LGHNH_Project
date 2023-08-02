using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //마우스 클릭 시
        if (Input.GetMouseButtonDown(1))
        {
            //마우스로 클릭한 좌표 값을 가져오기
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = new Vector3(0, 0, 1);

            //해당 좌표에 있는 오브젝트 찾기
            RaycastHit hit;
            Physics.Raycast(pos, dir, out hit, 100f);

            //Hit한 오브젝트의 Collider에 접촉
            if (hit.collider != null)
            {
                GameObject click_obj = hit.transform.gameObject;

                if (click_obj.tag == "NPC")
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
