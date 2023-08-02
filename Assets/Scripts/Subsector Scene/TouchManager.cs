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
        //���콺 Ŭ�� ��
        if (Input.GetMouseButtonDown(1))
        {
            //���콺�� Ŭ���� ��ǥ ���� ��������
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = new Vector3(0, 0, 1);

            //�ش� ��ǥ�� �ִ� ������Ʈ ã��
            RaycastHit hit;
            Physics.Raycast(pos, dir, out hit, 100f);

            //Hit�� ������Ʈ�� Collider�� ����
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
