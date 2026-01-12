using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlatePhysMechanic : MonoBehaviour
{
    public float torqueMultiplier = 10f;
    public float maxAngle = 30f;

    Rigidbody rb;
    PlateSlotManager slotManager;

    public float CurrentAngle { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        slotManager = GetComponent<PlateSlotManager>();

        HingeJoint hinge = GetComponent<HingeJoint>();
        hinge.axis = Vector3.forward;

        JointLimits limits = hinge.limits;
        limits.min = -maxAngle;
        limits.max = maxAngle;
        hinge.useLimits = true;
        hinge.limits = limits;
    }

    void FixedUpdate()
    {
        float totalTorque = 0f;

        foreach (PlateItem item in slotManager.GetItems())
        {
            totalTorque += item.weight * item.transform.localPosition.x;
        }

        rb.AddTorque(Vector3.forward * totalTorque * torqueMultiplier);

        float z = transform.localEulerAngles.z;
        if (z > 180f) z -= 360f;
        CurrentAngle = z;
    }
}

