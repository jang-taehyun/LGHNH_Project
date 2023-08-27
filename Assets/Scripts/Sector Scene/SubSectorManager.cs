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
        
        public int ClearNum;
        public string SubSector;
        public bool isActivateUI = false;

        // inspector setting //
        public GameObject SubSectorUI;
        public string SubSectorTitle;
        public string SubSectorExplanation;
        public GameObject CantEnterMessage;
        public Material DefaultMaterial;
        public Material GlowEffectMaterial;
        public GameObject camera;

        public AudioSource EnterSoundPlayer;
        public AudioClip EnterSound;

        // method //
        bool JudgeSectorActivation()
        {
            if (GameManager.GameManager.Inst.ReadClearNum() >= ClearNum)
            {
                Debug.Log(GameManager.GameManager.Inst.ReadClearNum());
                Debug.Log(ClearNum);

                return true;
            }
                

            CantEnterMessage.SetActive(true);
            return false;
        }
        void Start()
        {
            IsClear = false;
            isActivateUI = false;
            // camera = GameObject.Find("Main Camera");

            SubSectorUI.SetActive(false);
            SetCantEnterMessageDeactive();

            GetComponentInParent<SpriteRenderer>().material = DefaultMaterial;
        }

        void Update()
        {
            //// Only Execution when touchCount is 1
            //if (Input.touchCount > 0 && Input.touchCount <= 1)
            //{
            //    Debug.Log("-------------");
            //    // touch position, direction debug
            //    Debug.Log(Input.GetTouch(0).position);
            //    Debug.Log(Input.GetTouch(0).deltaPosition);

            //    // get touch state
            //    Touch touch = Input.GetTouch(0);


            //    Debug.Log(touch.position);
            //    Debug.Log((Vector2)this.transform.position);
            //    Debug.Log(Vector2.Distance(touch.position, (Vector2)this.transform.localPosition));
            //    Debug.Log("-------------");

            //    if (touch.phase == TouchPhase.Began &&
            //        (Vector2.Distance(touch.position, (Vector2)this.transform.position) <= GetComponentInParent<CircleCollider2D>().radius))
            //    {

            //        ClickSubSector();
            //    }
            //}

            if(!isActivateUI)
            {
                // raycast�� �̿��� �κ�
                // start -----------------
                if (Input.GetMouseButton(0))
                {
                    
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        // ���ϴ� code �ִ� �κ�
                        // Debug.Log(hit.transform.gameObject.name);
                        hit.transform.gameObject.GetComponent<SubSectorManager>().ClickSubSector();
                        isActivateUI = true;
                    }
                }
                else
                {
                    isActivateUI = false;
                    SubSectorUI.SetActive(false);
                    SetCantEnterMessageDeactive();
                }
                // end -----------------------------------
            }
            

        }

        // interface //
        public void ClickSubSector()
        {
            SubSectorUI.SetActive(true);
            camera.GetComponent<CameraMover_Test>().FocusCamera();

            GetComponentInParent<SpriteRenderer>().material = GlowEffectMaterial;
            
        }
        public void EnterSubSector()
        {
            if (!JudgeSectorActivation())
                return;

            EnterSoundPlayer.PlayOneShot(EnterSound);

            SubSectorUI.SetActive(false);
            GetComponentInParent<SpriteRenderer>().material = DefaultMaterial;
            camera.GetComponent<CameraMover_Test>().FreeCamera();
            SceneManager.LoadScene(SubSector);
            SceneManager.UnloadScene("일리야마운틴");
        }
        public void SetIsClear(bool _param) { IsClear = _param; }
        public void SetCantEnterMessageDeactive() { CantEnterMessage.SetActive(false); }
        public void DeactiveUI()
        {
            SubSectorUI.SetActive(false);
            isActivateUI = false;
            GetComponentInParent<SpriteRenderer>().material = DefaultMaterial;
        }
    }
}