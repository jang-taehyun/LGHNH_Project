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
        private GameObject Camera;
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
            return false;
        }

        void SetSubSectorGlowEffect(bool _param)
        {
            GlowEffect.SetActive(_param);
        }
        void Start()
        {
            IsClear = false;
            Camera = GameObject.Find("Main Camera");
        }

        void Update()
        {
            // touch position, direction debug
            Debug.Log(Input.GetTouch(0).position);
            Debug.Log(Input.GetTouch(0).deltaPosition);

            // Only Execution when touchCount is 1
            if (Input.touchCount > 0 && Input.touchCount <= 1)
            {
                // get touch state
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began && (touch.position == (Vector2)this.transform.localPosition))
                {
                    ClickSubSector();
                }
            }
        }

        // interface //
        public void EnterSubSector()
        {
            SubSectorExplanationUI.SetActive(false);
            SubSectorEnterButton.SetActive(false);
            SetSubSectorGlowEffect(false);

            // write 'enter SubSector Scene' code
            SceneManager.LoadScene(SubSector);
        }
        public void SetIsClear(bool _param) { IsClear = _param; }
    }
}