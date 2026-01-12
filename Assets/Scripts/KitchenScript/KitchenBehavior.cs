using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenBehavior : MonoBehaviour
{
    [System.Serializable]
    public class RecipeMapping
    {
        public string recipeName;
        public GameObject foodPrefab;
    }

    [System.Serializable]
    public class Recette
    {
        public string Name;
        public float TempsPrep;
        public float TimeLeft;
        public GameObject foodPrefab;

        public Recette(string name, float temps, GameObject food)
        {
            Name = name;
            TempsPrep = temps;
            TimeLeft = temps;
            foodPrefab = food;
        }
    }

    public int maxQueue = 5;
    public Transform outputTable;
    public GameObject platePrefab;
    public RecipeMapping[] recipeMappings;

    public List<Recette> ListeAttente = new List<Recette>();
    bool cooking = false;

    public bool AjouterRecetteQueue(string nom, float temps)
    {
        if (ListeAttente.Count >= maxQueue) return false;

        if (OutputTableManager.Instance != null && !OutputTableManager.Instance.HasAvailableSlot())
            return false;

        GameObject food = GetFoodPrefab(nom);
        if (food == null) return false;

        Recette r = new Recette(nom, temps, food);
        ListeAttente.Add(r);

        if (!cooking)
            StartCoroutine(Cook());

        return true;
    }

    IEnumerator Cook()
    {
        cooking = true;

        while (ListeAttente.Count > 0)
        {
            Recette current = ListeAttente[0];

            while (current.TimeLeft > 0f)
            {
                current.TimeLeft -= Time.deltaTime;
                yield return null;
            }

            if (current.foodPrefab != null && platePrefab != null && outputTable != null)
            {
                Transform empSlot = null;
                if (OutputTableManager.Instance != null)
                {
                    empSlot = OutputTableManager.Instance.GetNextAvailableSlot();
                    if (empSlot == null)
                    {
                        ListeAttente.RemoveAt(0);
                        continue;
                    }
                }

                Vector3 pos = empSlot != null ? empSlot.position : outputTable.position;
                GameObject plate = Instantiate(platePrefab, pos, Quaternion.identity);
                
                PlateSlotManager slotMgr = plate.GetComponent<PlateSlotManager>();
                if (slotMgr != null && slotMgr.emplacements != null && slotMgr.emplacements.Length > 0)
                {
                    GameObject food = Instantiate(current.foodPrefab);
                    
                    PlateItem item = food.GetComponent<PlateItem>();
                    if (item == null)
                    {
                        item = food.AddComponent<PlateItem>();
                        item.weight = 1f;
                    }
                    
                    if (!food.CompareTag("Food"))
                        food.tag = "Food";

                    FoodCleanup cleanup = food.GetComponent<FoodCleanup>();
                    if (cleanup == null)
                        cleanup = food.AddComponent<FoodCleanup>();
                    
                    Transform emp = slotMgr.emplacements[0];
                    food.transform.SetParent(emp);
                    food.transform.localPosition = Vector3.zero;
                    food.transform.localRotation = Quaternion.identity;
                    food.transform.localScale = Vector3.one * 2f;
                    
                    Rigidbody foodRb = food.GetComponent<Rigidbody>();
                    if (foodRb != null)
                        foodRb.isKinematic = true;
                    
                    slotMgr.GetItems().Add(item);
                }
                
                Rigidbody plateRb = plate.GetComponent<Rigidbody>();
                if (plateRb != null)
                    plateRb.isKinematic = false;

                PlateSlotTracker tracker = plate.GetComponent<PlateSlotTracker>();
                if (tracker == null)
                    tracker = plate.AddComponent<PlateSlotTracker>();
                tracker.SetAssignedSlot(empSlot);

                if (empSlot != null && OutputTableManager.Instance != null)
                    OutputTableManager.Instance.OccupySlot(empSlot, plate);
            }

            ListeAttente.RemoveAt(0);
        }

        cooking = false;
    }

    public string GetQueueStatusString()
    {
        if (ListeAttente.Count == 0)
            return "File d'attente : (vide)";

        string result = "File d'attente :\n";
        for (int i = 0; i < ListeAttente.Count; i++)
        {
            var r = ListeAttente[i];
            result += (i + 1) + ". " + r.Name + " - " + r.TimeLeft.ToString("0.#") + "s\n";
        }
        return result;
    }

    GameObject GetFoodPrefab(string name)
    {
        if (recipeMappings == null || recipeMappings.Length == 0)
            return null;

        foreach (RecipeMapping m in recipeMappings)
        {
            if (m.recipeName.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                return m.foodPrefab;
        }
        return null;
    }
}
