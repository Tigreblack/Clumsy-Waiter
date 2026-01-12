using UnityEngine;

public class PlatePickup : MonoBehaviour
{
    public Transform carryPoint;
    public float pickupRange = 2f;
    
    GameObject plate;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (plate == null)
                TryPickup();
            else
                Drop();
        }
    }

    void TryPickup()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, pickupRange);
        
        GameObject nearest = null;
        float nearestDist = float.MaxValue;
        
        foreach (Collider col in cols)
        {
            if (col.CompareTag("Plate"))
            {
                float dist = Vector3.Distance(transform.position, col.transform.position);
                if (dist < nearestDist)
                {
                    nearestDist = dist;
                    nearest = col.gameObject;
                }
            }
        }
        
        if (nearest != null)
        {
            plate = nearest;
            Rigidbody rb = plate.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            Collider plateCol = plate.GetComponent<Collider>();
            if (plateCol != null)
                plateCol.enabled = false;

            plate.transform.SetParent(carryPoint);
            plate.transform.localPosition = Vector3.zero;
            plate.transform.localRotation = Quaternion.identity;
        }
    }

    void Drop()
    {
        plate.transform.SetParent(null);
        
        Rigidbody rb = plate.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        Collider plateCol = plate.GetComponent<Collider>();
        if (plateCol != null)
            plateCol.enabled = true;
        
        plate = null;
    }
}
