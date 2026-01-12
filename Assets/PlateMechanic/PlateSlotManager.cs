using System.Collections.Generic;
using UnityEngine;

public class PlateSlotManager : MonoBehaviour
{
    public Transform[] emplacements;
    List<PlateItem> items = new List<PlateItem>();

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Food"))
            return;

        PlateItem item = col.gameObject.GetComponent<PlateItem>();
        if (item == null)
        {
            item = col.gameObject.AddComponent<PlateItem>();
        }

        TryPlace(item);
    }

    public bool TryPlace(PlateItem item)
    {
        foreach (Transform emp in emplacements)
        {
            if (emp.childCount == 0)
            {
                item.transform.SetParent(emp);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;

                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb) rb.isKinematic = true;

                if (!items.Contains(item))
                {
                    items.Add(item);
                }
                return true;
            }
        }

        Debug.Log("Plateau plein !");
        return false;
    }

    public List<PlateItem> GetItems() => items;
}

