using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshCutOnThaw : MonoBehaviour, IFrozenOnFractionFreeze
{

    public NavMeshObstacle NavMeshObstacle;
    public NavmeshCutoutBounds Bounds;
    [SerializeField] NavmeshCutOnThawType type;

    public NavMeshObjstcleData ThawObsticleData;
    protected NavMeshObjstcleData FreezeObsticleData;
    public bool IsFrozen {  get; set; }

    private void Start()
    {
        if(NavMeshObstacle == null)
        {
            NavMeshObstacle = GetComponent<NavMeshObstacle>();
        }

        if(Bounds == null)
        {
            Bounds = GetComponent<NavmeshCutoutBounds>();
        }

        FreezeObsticleData = FreezeObsticleData.Save(NavMeshObstacle, Bounds);

    }

    public void Freze()
    {
        if (NavMeshObstacle != null)
        {
            switch (type)
            {
                case NavmeshCutOnThawType.LoadTemplate:

                    FreezeObsticleData.Load(NavMeshObstacle, Bounds);

                    break;

                case NavmeshCutOnThawType.Disable:

                    NavMeshObstacle.enabled = true;

                    break;

                case NavmeshCutOnThawType.DisableCarve:

                    NavMeshObstacle.carving = FreezeObsticleData.Carving;

                    break;

                default:


                    break;
            }
        }
    }

    public void Thaw()
    {
        switch (type)
        {
            case NavmeshCutOnThawType.LoadTemplate:

                FreezeObsticleData = FreezeObsticleData.Save(NavMeshObstacle, Bounds);

                ThawObsticleData.Load(NavMeshObstacle, Bounds);

                break;

            case NavmeshCutOnThawType.Disable:

                NavMeshObstacle.enabled = false;


                break;

            case NavmeshCutOnThawType.Remove:

                Destroy(NavMeshObstacle);

                break;

            case NavmeshCutOnThawType.DisableCarve:

                NavMeshObstacle.carving = false;

                break;

            default:


                break;

        }

    }

    public enum NavmeshCutOnThawType
    {
        Nothing = 0,
        Disable,
        Remove,
        DisableCarve,
        LoadTemplate,
        
    }


}

[System.Serializable]
public struct NavMeshObjstcleData
{
    public Vector3 SizeMult;

    public NavMeshObstacleShape Shape;

    public bool Carving;
    public float MoveThreshold;
    public float TimeToStationary;
    public bool CarveOnlyStationary;

    public NavMeshObjstcleData Save(NavMeshObstacle obsticle, NavmeshCutoutBounds bounds)
    {
        if(bounds != null)
        {
            SizeMult = bounds.sizeMult;
        }

        if(obsticle != null)
        {
            Carving = obsticle.carving;
            MoveThreshold = obsticle.carvingMoveThreshold;
            TimeToStationary = obsticle.carvingTimeToStationary;
            CarveOnlyStationary = obsticle.carveOnlyStationary;
        }

        return this;

    }

    public void Load(NavMeshObstacle obsticle, NavmeshCutoutBounds bounds)
    {
        if (bounds != null)
        {
            bounds.sizeMult = SizeMult;   
        }

        if (obsticle != null)
        {
            obsticle.carveOnlyStationary = CarveOnlyStationary;
            obsticle.carving = Carving;
            obsticle.carvingMoveThreshold = MoveThreshold;
            obsticle.carvingTimeToStationary = TimeToStationary;

        }

    }

}