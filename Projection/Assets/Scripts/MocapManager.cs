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
                GameObject mocap = Instantiate(moCapController, target.transform.position, transform.rotation);
                mocap.transform.SetParent(target);
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

    public void ScaleMocap(Transform target, int currentRecursion, float newScale)
    {

        int MaxRecurtion = 200;
        // For some reason the mocap animator need to have a scale of one worldwise so you need to scale localy by the inverse of the hologram game object scale;
        float invProportion = 1 / Mathf.Max(newScale, 0.001f);
        Vector3 invScale = new Vector3(invProportion, invProportion, invProportion);

        if (currentRecursion < MaxRecurtion)
        {

            foreach (Transform item in target)
            {


                ScaleMocap(item, currentRecursion + 1, newScale);
            }
           

            xsens.XsLiveAnimator mocap = target.GetComponent<xsens.XsLiveAnimator>();

            

            if (mocap == null)
                return;

            mocap.obj.transform.localScale = invScale;
            Debug.LogWarning(target + " Scaled to " + invScale);
        }
        else
        {
            Debug.LogWarning("Armagedon Recurtion level greater than " + MaxRecurtion);
        }
    }
}