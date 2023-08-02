using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SectorManager : MonoBehaviour
{
    [SerializeField] private int sectorProgress;
    [SerializeField] private GameObject sectorButton; //이게필요없을듯 버튼마다 호출하게해서 하면될꺼같은데
    [SerializeField] private GameObject sectorExplanation;
    [SerializeField] private GameObject sectorTouchDomain;
    
    public GameObject cantEnterMessage;


    public void clickSector()
    {
        judgeVitalization();
    }

    public void enterSector(int number)  //Enter버튼에 달아줘서 number를 숫자1로지정했음
    {
        SceneManager.LoadScene(number);

    }

    private void judgeVitalization()
    {
        if (sectorProgress >= 0)//일단 사용할일 없어서 0으로했음
        {
            sectorExplanation.SetActive(true);
        }
        else
        {
            cantEnterMessage.SetActive(true);
        }

    }

}
