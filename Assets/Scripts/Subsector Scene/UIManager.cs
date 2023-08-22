using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject questUI;              //퀘스트 UI
    [SerializeField] private GameObject collectionImage;      //채집해야 하는 채집 오브젝트의 이미지
    [SerializeField] private GameObject clearUI;
    [SerializeField] private GameObject requireNumUI;
    [SerializeField] private TextMeshProUGUI requireNumUIText;
    [SerializeField] private ProcessManager processManager;
    [SerializeField] private GameObject cam;

    public GameObject[] collectionIllust;


    void Start()
    {
        questUI.SetActive(false);
        clearUI.SetActive(false);

        requireNumUI.SetActive(false);
        collectionImage.SetActive(false);

        for (int i = 0; i < collectionIllust.Length; i++)
        {
            collectionIllust[i].SetActive(false);
        }        
        
    }

    void Update()
    {
        
    }

    public void OffQuestUI()
    {
        questUI.SetActive(false);
        requireNumUI.SetActive(false);
        collectionIllust[processManager.ReadPhase()].SetActive(false);
        collectionImage.SetActive(false);
    }

    public void OnQuestUI()
    {
        questUI.SetActive(true);
        requireNumUI.SetActive(true);
        collectionIllust[processManager.ReadPhase()].SetActive(true);
        collectionImage.SetActive(true);
    }

    public void OnClearUI()
    {
        clearUI.SetActive(true);
        cam.GetComponent<CameraMover>().FocusCamera();
    }
    public void OffClearUI()
    {
        clearUI.SetActive(false);
        cam.GetComponent<CameraMover>().FreeCamera();
    }

    public void SetRequireNumText(int num)
    {
        if (num == 5) { requireNumUIText.text = "x5"; }
        else if (num == 4) { requireNumUIText.text = "x4"; }
        else if (num == 3) { requireNumUIText.text = "x3"; }
        else if (num == 2) { requireNumUIText.text = "x2"; }
        else if (num == 1) { requireNumUIText.text = "x1"; }
        else { requireNumUI.SetActive(false); }
    }

    public void GoSectorScene()
    {
        SceneManager.LoadScene("일리야마운틴");
    }
}
