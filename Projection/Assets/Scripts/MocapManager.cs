using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MocapManager : MonoBehaviour
{
    [SerializeField] xsens.XsStreamReader actor;
    [SerializeField] GameObject moCapController;


    public void CheckForMoCap(GameObject target)
   {
        SetupMocap(target.transform, 0);
    }
    public void SetupMocap(Transform target, int currentRecursion)
    {
        int MaxRecurtion = 200;
        if (currentRecursion < MaxRecurtion)
        {
            foreach (Transform item in target)
            {
                
                SetupMocap(item, currentRecursion + 1);

                
            }
            Animator anim = target.GetComponent<Animator>();

            if (anim == null)
                return;

            Avatar avatar = anim.avatar;
            if (avatar == null)
                return;
            Debug.Log("Object " + target + " as " + avatar);
            if (avatar.isHuman)
            {
                GameObject mocap = Instantiate(moCapController, target.transform);
                Debug.Log("Target " + target + " is controlled by " + mocap);
                xsens.XsLiveAnimator animator = mocap.GetComponent<xsens.XsLiveAnimator>();
                Debug.Log(animator);
                animator.Setup(actor, target);
            }
        }
        else
        {
            Debug.LogWarning("Armagedon Recurtion level greater than " + MaxRecurtion);
        }
    }
}
