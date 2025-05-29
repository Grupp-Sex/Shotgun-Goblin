using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BonePhysicsManager : MonoBehaviour
{
    public Transform AnimationRoot;
    [SerializeField] int AnimationBoneCount;
    [SerializeField] int AnimationBone_Succsess;
    public Transform[] AnimationBones {  get; private set; }


    public Transform PhysicsRoot;
    [SerializeField] int PhysicsBoneCount;
    [SerializeField] int PhysicsBone_Sucsess;

    public Transform[] PhysicsBones_Transforms { get; private set; }
    public PhysicsBone[] PhysicsBones_PysBone { get; private set; }


    public SkinnedMeshRenderer SkinnedMeshRenderer;
    [SerializeField] private bool RenderPhysicsBones = true;

    

    private void Awake()
    {
        if(SkinnedMeshRenderer == null)
        {
            SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        }
    }

    private void Start()
    {
        Initialize();
    }

    protected void Initialize()
    {
        AnimationRoot = SkinnedMeshRenderer.rootBone;
        AnimationBones = SkinnedMeshRenderer.bones;

        PhysicsBones_Transforms = FindBonesRecersively(AnimationBones, PhysicsRoot, out AnimationBone_Succsess);
        SetRenderBones(RenderPhysicsBones);

        SetPhysicsBoneTargets();

        AnimationBoneCount = AnimationBones.Length;
        PhysicsBoneCount = PhysicsBones_Transforms.Length;
    }

    protected void SetRenderBones(bool isPhysicsBones)
    {
        if (isPhysicsBones)
        {
            SkinnedMeshRenderer.rootBone = PhysicsRoot;
            SkinnedMeshRenderer.bones = PhysicsBones_Transforms;
        }
        else
        {
            SkinnedMeshRenderer.rootBone = AnimationRoot;
            SkinnedMeshRenderer.bones = AnimationBones;
        }
        
    }

    protected void OnValidate()
    {
        if(Application.isPlaying && isActiveAndEnabled)
        {
            SetRenderBones(RenderPhysicsBones);
        }
    }


    protected void SetPhysicsBoneTargets()
    {
        PhysicsBone_Sucsess = 0;

        PhysicsBones_PysBone = new PhysicsBone[PhysicsBones_Transforms.Length];

        for(int i = 0; i < PhysicsBones_Transforms.Length; i++)
        {
            PhysicsBones_PysBone[i] = PhysicsBones_Transforms[i].GetComponent<PhysicsBone>();

            if(PhysicsBones_PysBone[i] != null && AnimationBones[i] != null)
            {
                PhysicsBones_PysBone[i].SetTarget(AnimationBones[i]);
                PhysicsBone_Sucsess++;
            }
            
        }
    }
    private Transform[] FindBonesRecersively(Transform[] bones, Transform rootBone, out int succsess_count)
    {
        

        List<Transform> rootChildren = rootBone.GetComponentsInChildren<Transform>().ToList();

        bones = bones.ToArray();

        succsess_count = 0;

        for(int i = 0; i < bones.Length; i++)
        {
            bool succsess = false;

            for(int j = 0; j< rootChildren.Count; j++)
            {
                if (bones[i].name == rootChildren[j].name)
                {
                    bones[i] = rootChildren[j];
                    //rootChildren.RemoveAt(j);
                    //j--;
                    succsess = true;
                    break;
                }
            }

            if (succsess)
            {
                succsess_count++;
            }


        }

        return bones;

        
    }



    
}
