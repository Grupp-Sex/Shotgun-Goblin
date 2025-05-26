using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBone : MonoBehaviour
{
    public Transform targetAnimationBone;

    public ConfigurableJoint targetJoint { get; private set; }
    //public CharacterJoint boneJoint { get; private set; }
    

    [SerializeField] private Vector3 targetRotaion;
    [SerializeField] private Vector3 currentRotation;

    private void Awake()
    {
        //if(boneJoint == null)
        //{
        //    boneJoint = GetComponent<CharacterJoint>();
        //}

        if(targetJoint == null)
        {
            targetJoint = GetComponent<ConfigurableJoint>();
        }
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateBone();
    }

    public void SetTarget(Transform targetBone)
    {
        targetAnimationBone = targetBone;
    }

    protected void UpdateBone()
    {
        if (targetAnimationBone != null)
        {
            //targetJoint.targetPosition = targetAnimationBone.localPosition;
            targetJoint.targetRotation = GetTargetRotation();
            targetRotaion = targetJoint.targetRotation.eulerAngles;
            currentRotation = transform.localEulerAngles;
        }
    }

    protected Quaternion GetTargetRotation()
    {
        Quaternion targetLocalRotation = targetAnimationBone.localRotation; //Quaternion.Inverse(ParrentQuanterion()) * targetAnimationBone.rotation;
        return Quaternion.Inverse(targetLocalRotation);
    }

    protected Quaternion ParrentQuanterion()
    {
        return targetJoint.connectedBody.rotation;
        
    }
}
