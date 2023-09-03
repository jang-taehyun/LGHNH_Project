using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ObstacleAnimation : MonoBehaviour
{
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    private float Duration;                         // ��� �ð�
    private bool IsPlay = false;

    [SpineAnimation]
    public string AnimationName;

    // Start is called before the first frame update
    void Start()
    {
        // animation player ����
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;

                       // ����� �ִϸ��̼�

        // ��� �ð� backUp
        //Duration = skeletonAnimation.timeScale;

        // �ִϸ��̼� �Ͻ� ����
        //spineAnimationState.TimeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayDisappear()
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

    public void PlayAnimation_Dis()
    {
        StartCoroutine(PlayDisappear());
    }
}
