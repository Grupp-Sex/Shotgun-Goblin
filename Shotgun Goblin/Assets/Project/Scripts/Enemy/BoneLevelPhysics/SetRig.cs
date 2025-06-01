using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class SetRig : MonoBehaviour
{
    public Transform AnimationRoot;
    [SerializeField] int AnimationBoneCount;
    [SerializeField] int AnimationBone_Succsess;
    public Transform[] AnimationBones { get; private set; }


    

    


    public SkinnedMeshRenderer SkinnedMeshRenderer;
 



    private void Awake()
    {
        if (SkinnedMeshRenderer == null)
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
        
        Transform[] OldAnimationBones = SkinnedMeshRenderer.bones;

        AnimationBones = FindBonesRecersively(OldAnimationBones, AnimationRoot, out AnimationBone_Succsess);
        
        SkinnedMeshRenderer.bones = AnimationBones;
        SkinnedMeshRenderer.rootBone = AnimationRoot;
        
    }

   

   


    
    private Transform[] FindBonesRecersively(Transform[] bones, Transform rootBone, out int succsess_count)
    {


        List<Transform> rootChildren = rootBone.GetComponentsInChildren<Transform>().ToList();

        bones = bones.ToArray();

        succsess_count = 0;

        for (int i = 0; i < bones.Length; i++)
        {
            bool succsess = false;

            for (int j = 0; j < rootChildren.Count; j++)
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



