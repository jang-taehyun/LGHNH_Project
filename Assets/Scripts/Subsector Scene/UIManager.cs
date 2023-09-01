using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject questUI;              //����Ʈ UI
    [SerializeField] private GameObject collectionImage;      //ä���ؾ� �ϴ� ä�� ������Ʈ�� �̹���
    [SerializeField] private GameObject clearUI;
    [SerializeField] private GameObject requireNumUI;
    [SerializeField] private TextMeshProUGUI requireNumUIText;
    [SerializeField] private ProcessManager processManager;
    [SerializeField] private GameObject cam;
    public SoundManager soundManager;

    [Header("설정")]
    public GameObject settings;     private bool settingsOn;
    public GameObject[] bgButtons;  private bool bgmOn;
    public GameObject[] seButtons;  private bool seOn;


    public GameObject[] collectionIllust;


    public AudioSource clickSoundPlayer;
    public AudioClip clickSound;


    void Start()
    {
        questUI.SetActive(false);
        clearUI.SetActive(false);
        settings.SetActive(false); settingsOn = false;

        bgmOn = GameManager.GameManager.Inst.bgmonoff;
        seOn = GameManager.GameManager.Inst.seonoff;
        

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
        clickSoundPlayer.PlayOneShot(clickSound);
        SceneManager.LoadScene("일리야마운틴");
    }

    public void ClickSetting()
    {
        if (settingsOn == false)
        {
            settings.SetActive(true);
            settingsOn = true;
            if (bgmOn == true)
            {
                bgButtons[0].SetActive(true);
                bgButtons[1].SetActive(false);
            }
            else
            {
                bgButtons[0].SetActive(false);
                bgButtons[1].SetActive(true);
            }
            if (seOn == true)
            {
                seButtons[0].SetActive(true);
                seButtons[1].SetActive(false);
            }
            else
            {
                seButtons[0].SetActive(false);
                seButtons[1].SetActive(true);
            }
        }
        else
        {
            settings.SetActive(false);
            settingsOn = false;
        }
    }


    public void BGButton()
    {
        GameManager.GameManager.Inst.bgmonoff = !GameManager.GameManager.Inst.bgmonoff;
        bgmOn = GameManager.GameManager.Inst.bgmonoff;

        if (bgmOn == true)
        {
            bgButtons[0].SetActive(true);
            bgButtons[1].SetActive(false);
        }
        else
        {
            bgButtons[0].SetActive(false);
            bgButtons[1].SetActive(true);
        }
        soundManager.SettingChanged();
    }

    public void SEButton()
    {
        GameManager.GameManager.Inst.seonoff = !GameManager.GameManager.Inst.seonoff;
        seOn = GameManager.GameManager.Inst.seonoff;

        if (seOn == true)
        {
            seButtons[0].SetActive(true);
            seButtons[1].SetActive(false);
        }
        else
        {
            seButtons[0].SetActive(false);
            seButtons[1].SetActive(true);
        }
        soundManager.SettingChanged();

    }
}
