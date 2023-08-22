using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SectorManager : MonoBehaviour
{
    // member variable //
    bool IsClear;
    private GameObject camera;
        
    public string Sector;
    public bool isActivateUI = false;

    // inspector setting //
    public GameObject SectorUI;
    public string SectorTitle;
    public string SectorExplanation;
    public GameObject CantEnterMessage;
    public Material DefaultMaterial;
    public Material GlowEffectMaterial;
        

        // method //
        void JudgeSectorActivation()
        {
            
            CantEnterMessage.SetActive(true);
            
        }
        void Start()
        {
            IsClear = false;
            isActivateUI = false;
            camera = GameObject.Find("Main Camera");

            SectorUI.SetActive(false);
            SetCantEnterMessageDeactive();

            GetComponentInParent<SpriteRenderer>().material = DefaultMaterial;
        }

        void Update()
        {
           
            

            if(!isActivateUI)
            {
                
                if (Input.GetMouseButton(0))
                {
                    
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        
                        hit.transform.gameObject.GetComponent<SectorManager>().ClickSector();
                        isActivateUI = true;
                    }
                }
              
            }
            

        }

        
        public void ClickSector()
        {
            SectorUI.SetActive(true);
            camera.GetComponent<CameraMover>().FocusCamera();
            

            GetComponentInParent<SpriteRenderer>().material = GlowEffectMaterial;
            
        }
        public void EnterSector()
        {
            

            SectorUI.SetActive(false);
            GetComponentInParent<SpriteRenderer>().material = DefaultMaterial;
            camera.GetComponent<CameraMover_Test>().FreeCamera();
            SceneManager.LoadScene(Sector);
        }
        public void SetIsClear(bool _param) { IsClear = _param; }
        public void SetCantEnterMessageDeactive() { CantEnterMessage.SetActive(false); }
        public void DeactiveUI()
        {
            SectorUI.SetActive(false);
            isActivateUI = false;
            GetComponentInParent<SpriteRenderer>().material = DefaultMaterial;
        }
        
       

        
}

    

   

    

    
