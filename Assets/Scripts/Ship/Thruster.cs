using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public enum ThrusterPlacement
    {
        FrontBack,
        Side
    }
    [SerializeField] private Rigidbody rb;
    [SerializeField] private bool isActiveByDefault;
    [SerializeField] private bool requiresRaycast = true;
    [SerializeField] private bool affectsTorque = false;
    [SerializeField] private float strength = 2f;
    [SerializeField] private float torqueMultiplier = 1f;
    [SerializeField] private float detectionDistance = 10f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private ThrusterPlacement placement;
    [SerializeField] private bool renderGizmo = true;
    [HideInInspector] public bool isActive;

    public Vector3 Normal {get; private set;}
    public float HitDistance {get; private set;}

    private void Start() {
        isActive = isActiveByDefault;
        Normal = Vector3.up;
        HitDistance = 0;
    }
    void FixedUpdate()
    {
        if (isActive)
        {
            Vector3 pos = transform.position;
            Vector3 dir = transform.forward;
            Vector3 force = Vector3.zero;
            float torque = 0f;
            RaycastHit hit;
            float relPosOnAxis = placement == ThrusterPlacement.FrontBack ? transform.localPosition.z : transform.localPosition.x;

            Debug.Log($"{name}\t{dir}");
            if (requiresRaycast)
            {
                bool isHit = Physics.Raycast(pos, dir, out hit, detectionDistance, layerMask);
                if (isHit)
                {
                    HitDistance = hit.distance;
                    force = -dir * (strength / HitDistance);
                    torque = (strength / HitDistance) * relPosOnAxis * torqueMultiplier;
                    Normal = hit.normal;
                }
            }
            else{
                force = -dir * strength;
                torque = strength * relPosOnAxis * torqueMultiplier;
            }
            rb.AddRelativeForce(force, ForceMode.Force);
            if (affectsTorque)
            {
                if (placement == ThrusterPlacement.FrontBack)
                    rb.AddRelativeTorque(new Vector3(-1f, 0f, 0f) * torque, ForceMode.Force);
                else rb.AddRelativeTorque(new Vector3(0f, 0f, 1f) * torque, ForceMode.Force);
            }
        }
    }
    void OnDrawGizmos() {
        if (renderGizmo)
        {
            Gizmos.color = Color.Lerp(new Color(1f, .1f, .4f), new Color(.1f, 1f, .4f), HitDistance / detectionDistance);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * detectionDistance);
        }
    }
}
