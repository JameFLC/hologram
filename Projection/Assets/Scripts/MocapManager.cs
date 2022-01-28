using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MocapManager : MonoBehaviour
{
    [SerializeField] xsens.XsStreamReader actor;
    [SerializeField] GameObject moCapController;


    public void SetupMocap(GameObject target)
   {
        Avatar avatar = target.GetComponent<Animator>().avatar;

        if (avatar == null)
            return;

        if (avatar.isHuman)
        {
            GameObject mocap = Instantiate(moCapController, transform);
            mocap.GetComponent<xsens.XsLiveAnimator>().Setup(actor, transform);
        }
    }
}
