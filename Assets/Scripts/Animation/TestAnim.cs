using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;                                  // spine �ִϸ��̼� ���

public class TestAnim : MonoBehaviour
{
    // -------------------- member variable -------------------- //
    // animation player //
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    private float Duration;                         // ��� �ð�
    private bool IsPlay = false;

    // -------------------- inspector -------------------- //
    [SpineAnimation]
    public string AnimationName;                    // ����� �ִϸ��̼�

    // -------------------- method -------------------- //
    void Start()
    {
        // animation player ����
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;

        // ��� �ð� backUp
        Duration = skeletonAnimation.timeScale;

        // �ִϸ��̼� �Ͻ� ����
        spineAnimationState.TimeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsPlay)
        {
            // �ִϸ��̼� ����
            StartCoroutine(PlayAnim());
            IsPlay = false;
        }
    }

    IEnumerator PlayAnim()
    {
        // �ִϸ��̼��� ��� �ð��� ����� �ð����� �ǵ�����
        spineAnimationState.TimeScale = Duration;

        // �ִϸ��̼� ���(�ڷ�ƾ �̿�)
        spineAnimationState.SetAnimation(0, AnimationName, false);
        /*
         * 1 parameter : Ʈ��
         * 2 parameter : ����� �ִϸ��̼�
         * 3 parameter : loop ����
        **/
        yield return new WaitForSeconds(Duration);
    }

    // -------------------- interface -------------------- //
    public void PlayButton()
    {
        IsPlay = !IsPlay;
    }
}
