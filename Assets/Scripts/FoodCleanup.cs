using UnityEngine;

public class FoodCleanup : MonoBehaviour
{
    public float cleanupDelay = 30f;

    private float dropTimer = 0f;
    private bool onFloor = false;
    private bool onOutputTable = false;

    void Start()
    {
        UpdateOutputTableStatus();
    }

    void Update()
    {
        UpdateOutputTableStatus();

        if (!onOutputTable && IsOnFloor())
        {
            if (!onFloor)
            {
                onFloor = true;
                dropTimer = 0f;
            }

            dropTimer += Time.deltaTime;

            if (dropTimer >= cleanupDelay)
            {
                GameObject rootObj = transform.root.gameObject;
                if (rootObj.CompareTag("Plate"))
                {
                    Destroy(rootObj);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            onFloor = false;
            dropTimer = 0f;
        }
    }

    bool IsOnFloor()
    {
        GameObject rootObj = transform.root.gameObject;
        
        if (rootObj.CompareTag("Plate"))
        {
            Rigidbody rb = rootObj.GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic)
            {
                return rb.linearVelocity.magnitude < 0.1f;
            }
        }
        else
        {
            if (transform.parent != null) return false;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic)
            {
                return rb.linearVelocity.magnitude < 0.1f;
            }
            return true;
        }

        return false;
    }

    void UpdateOutputTableStatus()
    {
        if (transform.parent == null)
        {
            onOutputTable = false;
            return;
        }

        GameObject rootObj = transform.root.gameObject;
        
        if (OutputTableManager.Instance != null)
        {
            onOutputTable = OutputTableManager.Instance.IsPlateOnOutputTable(rootObj);
        }
        else
        {
            onOutputTable = rootObj.name.Contains("Plate") && transform.parent.name.StartsWith("emp_");
        }
    }

    void OnDestroy()
    {
        if (transform.parent != null)
        {
            Transform empSlot = GetEmpSlot(transform);
            if (empSlot != null && OutputTableManager.Instance != null)
            {
                OutputTableManager.Instance.FreeSlot(empSlot);
            }
        }
    }

    Transform GetEmpSlot(Transform t)
    {
        while (t != null)
        {
            if (t.name.StartsWith("emp_")) return t;
            t = t.parent;
        }
        return null;
    }
}
