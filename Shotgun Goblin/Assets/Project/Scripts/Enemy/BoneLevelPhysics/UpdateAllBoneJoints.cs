using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAllBoneJoints : MonoBehaviour
{
    protected ConfigurableJoint joint;
    public BonePhysicsManager boneManager;
    public bool updateBones;
    // Start is called before the first frame update
    void Awake()
    {
        joint = GetComponent<ConfigurableJoint>();
        if(boneManager == null)
        {
            boneManager = GetComponent<BonePhysicsManager>();
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying && updateBones)
        {
            UpdateBones();
            updateBones = false;
        }
    }

    protected void UpdateBones()
    {
        foreach(var bone in boneManager.PhysicsBones_PysBone)
        {
            ConfigurableJoint tJoint = bone.targetJoint;
            tJoint.angularXDrive = joint.angularXDrive;
            tJoint.angularXLimitSpring = joint.angularXLimitSpring;
            tJoint.angularXMotion = joint.angularXMotion;
            tJoint.angularYLimit = joint.angularYLimit;
            tJoint.angularYMotion = joint.angularYMotion;
            tJoint.angularYZDrive = joint.angularYZDrive;
            tJoint.angularYZLimitSpring = joint.angularYZLimitSpring;
            tJoint.angularZLimit = joint.angularZLimit;
            tJoint.angularZMotion = joint.angularZMotion;
            tJoint.breakForce = joint.breakForce;
            tJoint.breakTorque = joint.breakTorque;
        }
    }
}
