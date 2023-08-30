using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;                                  // spine 애니메이션 사용

public class TestAnim : MonoBehaviour
{
    // -------------------- member variable -------------------- //
    // animation player //
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    private float Duration;                         // 재생 시간
    private bool IsPlay = false;

    // -------------------- inspector -------------------- //
    [SpineAnimation]
    public string AnimationName;                    // 재생할 애니메이션

    // -------------------- method -------------------- //
    void Start()
    {
        // animation player 세팅
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;

        // 재생 시간 backUp
        Duration = skeletonAnimation.timeScale;

        // 애니메이션 일시 정지
        spineAnimationState.TimeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsPlay)
        {
            // 애니메이션 시작
            StartCoroutine(PlayAnim());
            IsPlay = false;
        }
    }

    IEnumerator PlayAnim()
    {
        // 애니메이션의 재생 시간을 백업한 시간으로 되돌리기
        spineAnimationState.TimeScale = Duration;

        // 애니메이션 재생(코루틴 이용)
        spineAnimationState.SetAnimation(0, AnimationName, false);
        /*
         * 1 parameter : 트랙
         * 2 parameter : 재생할 애니메이션
         * 3 parameter : loop 여부
        **/
        yield return new WaitForSeconds(Duration);
    }

    // -------------------- interface -------------------- //
    public void PlayButton()
    {
        IsPlay = !IsPlay;
    }
}
