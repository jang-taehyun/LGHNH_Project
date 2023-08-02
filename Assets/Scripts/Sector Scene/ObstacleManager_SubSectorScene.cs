using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObstacleManager_SubSectorScene
{
    public class ObstacleManager_SubSectorScene : MonoBehaviour
    {
        // member variable //
        private bool isActive;

        // inspector setting //
        public Animation obsEliminationAnimation;
        public AudioSource ElimiationSoundl;
        public GameObject GlowEffect;

        // interface //
        public bool GetIsActive() { return isActive; }
        public void SetIsActive() { isActive = !isActive; }

        void Start()
        {
            isActive = true;
        }

        void Update()
        {
            if (!JudgeEliminationThisObs())
            {
                SetObsGlowEffect(false);
                obsEliminationAnimation.Play();
                ElimiationSoundl.Play();
                // write 'delay' code

                // ------------------
                Destroy(this);
            }
        }

        // method //
        bool JudgeEliminationThisObs()
        {
            return isActive;
        }

        void SetObsGlowEffect(bool _param)
        {
            GlowEffect.SetActive(_param);
        }
    }
}