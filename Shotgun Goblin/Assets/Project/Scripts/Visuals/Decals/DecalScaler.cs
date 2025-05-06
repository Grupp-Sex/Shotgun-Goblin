using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalScaler : MonoBehaviour
{
    public Vector3 Scale = new Vector3(1,1,1);

    public Vector3 OriginalSize;

    public bool SaveOnStart;

    public bool Buton_Save = true;

    public DecalProjector DecalComponent;

    private void Start()
    {
        if (SaveOnStart)
        {
            SaveScale();
        }
        ScaleDecal(Scale, OriginalSize);
    }

    private void OnValidate()
    {
        if (Buton_Save)
        {
            SaveScale();
        }
        else
        {
            ScaleDecal(Scale, OriginalSize);
        }
    }

    


    protected bool TryGetDecal(out DecalProjector outdecal)
    {
        if(DecalComponent == null)
        {
            DecalComponent = GetComponent<DecalProjector>();       
        }

        outdecal = DecalComponent;
        
        return DecalComponent != null;
    }

    protected void SaveScale()
    {
        if (TryGetDecal(out DecalProjector decal))
        {
            OriginalSize = decal.size;
        }
    }

    protected void ScaleDecal(Vector3 scale, Vector3 originalSize)
    {
        if (TryGetDecal(out DecalProjector decal))
        {
            decal.size = ScaleVector(originalSize, scale);

        }
        
    }

    protected Vector3 ScaleVector(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
    }

}
