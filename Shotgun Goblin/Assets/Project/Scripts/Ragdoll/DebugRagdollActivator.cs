using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRagdollActivator : MonoBehaviour
{
    public RagdollManager RagdollManager;
    [SerializeField] private float Duration = 5;
    [SerializeField] private Vector3 ApliedAngularVelocity;

    [Header("Activate Ragdoll")]
    [SerializeField] private bool ActivateButton;

    private void Awake()
    {
        if(RagdollManager == null)
        {
            RagdollManager = GetComponent<RagdollManager>();
        }
    }

    private void Update()
    {
        RagdollButton();
    }

    protected void RagdollButton()
    {
        if (ActivateButton)
        {
            ActivateRagdoll();
            ActivateButton = false;
        }
    }

    protected void ActivateRagdoll()
    {
        RagdollManager.EnterRagdoll(Duration, ApliedAngularVelocity);
    }

}
