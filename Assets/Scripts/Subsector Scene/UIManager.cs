using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private GameObject questUI;              //퀘스트 UI
    private GameObject collectionImage;      //채집해야 하는 채집 오브젝트의 이미지
    private GameObject clearUI;
    private GameObject requireNumUI;
    private TextMeshProUGUI requireNumUIText;
    void Start()
    {
        questUI = GameObject.Find("Quest UI");    questUI.SetActive(false);
        clearUI = GameObject.Find("Clear UI");    clearUI.SetActive(false);
        requireNumUIText = GameObject.Find("RequireNum UI Text").GetComponent<TextMeshProUGUI>();
        requireNumUI = GameObject.Find("RequireNum UI"); requireNumUI.SetActive(false);
        collectionImage = GameObject.Find("Collection Image");  collectionImage.SetActive(false);
        
        
    }

    void Update()
    {
        
    }

    public void OffQuestUI()
    {
        questUI.SetActive(false);
        requireNumUI.SetActive(false);
        collectionImage.SetActive(false);
    }

    public void OnQuestUI()
    {
        questUI.SetActive(true);
        requireNumUI.SetActive(true);
        collectionImage.SetActive(true);
    }

    public void OnClearUI()
    {
        clearUI.SetActive(true);
    }
    public void OffClearUI()
    {
        clearUI.SetActive(false);
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
}
