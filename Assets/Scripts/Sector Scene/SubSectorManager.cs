using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManager;
using ObstacleManager_SubSectorScene;
using UnityEngine.SceneManagement;

namespace SubSectorManager
{
    public class SubSectorManager : MonoBehaviour
    {
        // member variable //
        bool IsClear;
        // private GameObject Camera;
        public int ClearNum;
        public string SubSector;

        // inspector setting //
        public GameObject SubSectorEnterButton;
        public GameObject SubSectorExplanationUI;
        public string SubSectorTitle;
        public string SubSectorExplanation;
        public GameObject CantEnterMessage;
        public GameObject GlowEffect;

        // method //
        void ClickSubSector()
        {
            Debug.Log("click");
            if (!JudgeSectorActivation())
                return;

            Debug.Log("click2");

            SetSubSectorGlowEffect(true);
            SubSectorExplanationUI.SetActive(true);
            SubSectorEnterButton.SetActive(true);

            // UI    ?     touch    ' ??   ' execute
        }

        bool JudgeSectorActivation()
        {
            if (GameManager.GameManager.Inst.ReadClearNum() <= ClearNum)
                return true;

            CantEnterMessage.SetActive(true);

            object tmp = "error";
            Debug.Log(tmp);

            return false;
        }

        void SetSubSectorGlowEffect(bool _param)
        {
            GlowEffect.SetActive(_param);
        }
        void Start()
        {
            IsClear = false;
            // Camera = GameObject.Find("Main Camera");

            SubSectorEnterButton.SetActive(false);
            SubSectorExplanationUI.SetActive(false);
            CantEnterMessage.SetActive(false);
            GlowEffect.SetActive(false);
        }

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

                Debug.Log("mouse click");

                //Hit한 오브젝트의 Collider에 접촉
                if (hit.collider != null)
                {
                    GameObject click_obj = hit.transform.gameObject;

                    if (click_obj.tag == "Collection")
                    {
                        GameObject.Find(click_obj.name).GetComponent<SubSectorManager>().ClickSubSector();
                    }

                    else if (click_obj.tag == "NPC")
                    {
                        GameObject.Find(click_obj.name).GetComponent<SubSectorManager>().EnterSubSector();
                        
                    }
                    
                }
                else
                {
                    SubSectorEnterButton.SetActive(false);
                    SubSectorExplanationUI.SetActive(false);
                    GlowEffect.SetActive(false);
                }

            }

            // Only Execution when touchCount is 1
            if ((Input.touchCount > 0 && Input.touchCount <= 1))
            {
                // touch position, direction debug
                Debug.Log(Input.GetTouch(0).position);
                Debug.Log(Input.GetTouch(0).deltaPosition);

                // get touch state
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began && (touch.position == (Vector2)this.transform.localPosition))
                {
                    ClickSubSector();
                }
                else if (touch.phase == TouchPhase.Began)
                    Debug.Log("error_2");
            }
        }

        // interface //
        public void EnterSubSector()
        {
            //SubSectorExplanationUI.SetActive(false);
            //SubSectorEnterButton.SetActive(false);
            //SetSubSectorGlowEffect(false);

            SceneManager.LoadScene(SubSector);
        }
        public void SetIsClear(bool _param) { IsClear = _param; }
    }
}