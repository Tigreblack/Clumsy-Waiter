using System.Collections.Generic;
using UnityEngine;

public class OutputTableManager : MonoBehaviour
{
    public static OutputTableManager Instance { get; private set; }

    public Transform[] empSlots;
    public int maxPlates = 5;

    private Dictionary<Transform, GameObject> slots = new Dictionary<Transform, GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (empSlots == null || empSlots.Length == 0)
        {
            Debug.LogError("OutputTableManager: No emp slots assigned!");
            return;
        }

        foreach (Transform emp in empSlots)
        {
            if (emp != null)
            {
                slots[emp] = null;
            }
        }
    }

    public bool HasAvailableSlot()
    {
        foreach (var slot in slots)
        {
            if (slot.Value == null) return true;
        }
        return false;
    }

    public int GetAvailableSlotCount()
    {
        int count = 0;
        foreach (var slot in slots)
        {
            if (slot.Value == null) count++;
        }
        return count;
    }

    public int GetOccupiedSlotCount()
    {
        return maxPlates - GetAvailableSlotCount();
    }

    public Transform GetNextAvailableSlot()
    {
        foreach (var slot in slots)
        {
            if (slot.Value == null) return slot.Key;
        }
        return null;
    }

    public bool OccupySlot(Transform slot, GameObject plate)
    {
        if (slots.ContainsKey(slot) && slots[slot] == null)
        {
            slots[slot] = plate;
            return true;
        }
        return false;
    }

    public void FreeSlot(Transform slot)
    {
        if (slots.ContainsKey(slot))
        {
            slots[slot] = null;
        }
    }

    public void FreePlate(GameObject plate)
    {
        foreach (var slot in slots)
        {
            if (slot.Value == plate)
            {
                slots[slot.Key] = null;
                return;
            }
        }
    }

    public bool IsPlateOnOutputTable(GameObject plate)
    {
        foreach (var slot in slots)
        {
            if (slot.Value == plate) return true;
        }
        return false;
    }

    void Update()
    {
        foreach (var slot in slots)
        {
            if (slot.Value != null && slot.Value == null)
            {
                slots[slot.Key] = null;
            }
        }
    }
}
