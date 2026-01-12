using UnityEngine;

public class PlateSlotTracker : MonoBehaviour
{
    private Transform empSlot = null;
    private bool onTable = true;

    public void SetAssignedSlot(Transform slot)
    {
        empSlot = slot;
        onTable = true;
    }

    void Update()
    {
        if (empSlot != null && onTable)
        {
            if (!IsStillOnTable())
            {
                if (OutputTableManager.Instance != null)
                {
                    OutputTableManager.Instance.FreeSlot(empSlot);
                }
                onTable = false;
                empSlot = null;
            }
        }
    }

    bool IsStillOnTable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && rb.isKinematic) return true;

        if (empSlot != null)
        {
            float dist = Vector3.Distance(transform.position, empSlot.position);
            return dist < 0.5f;
        }

        return false;
    }

    void OnDestroy()
    {
        if (empSlot != null && OutputTableManager.Instance != null)
        {
            OutputTableManager.Instance.FreeSlot(empSlot);
        }
    }
}
