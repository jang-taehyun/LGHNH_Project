using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager inst = null;
        private int ClearNum = 0;
        private const uint ItemVariableNum = 30;
        private bool[] CurrentHaveItemNum;

        // interface //
        public int ReadClearNum() { return ClearNum; }
        public void IncreaseClearNum() { ClearNum++; }
        public void SetClearNumZero() { ClearNum = 0; }
        public static GameManager Inst
        {
            get
            {
                if (inst == null)
                    return null;
                return inst;
            }
        }
        public bool isHaveItem(uint index) { return CurrentHaveItemNum[index]; }
        public void AddItem(uint index) { CurrentHaveItemNum[index] = true; }

        // method //
        private void Awake()
        {
            if (null == inst)
            {
                inst = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
                Destroy(this.gameObject);
        }

        private void Start()
        {
            CurrentHaveItemNum = new bool[ItemVariableNum];
            for (int index = 0; index < ItemVariableNum; index++)
                CurrentHaveItemNum[index] = false;
        }
    }
}