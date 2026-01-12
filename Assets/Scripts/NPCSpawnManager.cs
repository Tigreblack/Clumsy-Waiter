using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCSpawnManager : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform[] spawnPoints;
    public GameObject[] characterModels;
    public string[] availableFoods = { "Pizza", "Hotdog", "Curry", "Udon", "Sushi", "Doughnut" };
    public int minQuestPoints = 1;
    public int maxQuestPoints = 7;
    public int maxActiveNPCs = 2;
    public float respawnCooldown = 3f;
    
    List<NPCData> activeNPCs = new List<NPCData>();
    List<int> freeSpawns = new List<int>();
    
    class NPCData
    {
        public GameObject npc;
        public int spawnIdx;
    }
    
    void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
            freeSpawns.Add(i);
        
        StartCoroutine(SpawnInitialNPCs());
    }
    
    IEnumerator SpawnInitialNPCs()
    {
        yield return new WaitForSeconds(0.5f);
        
        for (int i = 0; i < maxActiveNPCs; i++)
        {
            SpawnRandom();
            yield return new WaitForSeconds(0.2f);
        }
    }
    
    void SpawnAt(int idx)
    {
        if (idx < 0 || idx >= spawnPoints.Length || npcPrefab == null) return;
        
        Transform spawn = spawnPoints[idx];
        GameObject npc = Instantiate(npcPrefab, spawn.position, spawn.rotation);
        npc.name = "NPC_" + spawn.name;
        
        if (characterModels != null && characterModels.Length > 0)
        {
            GameObject model = characterModels[Random.Range(0, characterModels.Length)];
            if (model != null)
            {
                GameObject character = Instantiate(model, npc.transform);
                character.name = "CharacterModel";
                character.transform.localPosition = Vector3.zero;
                character.transform.localRotation = Quaternion.identity;
                character.transform.localScale = Vector3.one * 3f;
            }
        }
        
        DeliveryZone zone = npc.GetComponentInChildren<DeliveryZone>();
        if (zone != null)
        {
            string food = availableFoods[Random.Range(0, availableFoods.Length)];
            int pts = Random.Range(minQuestPoints, maxQuestPoints + 1);
            zone.SetRequestedFood(food, pts);
            zone.OnDeliveryComplete += () => HandleComplete(npc, idx);
            zone.OnQuestFailed += () => HandleFailed(npc, idx);
        }
        
        NPCData data = new NPCData { npc = npc, spawnIdx = idx };
        activeNPCs.Add(data);
        freeSpawns.Remove(idx);
    }
    
    public void SpawnRandom()
    {
        if (freeSpawns.Count == 0) return;
        
        int idx = freeSpawns[Random.Range(0, freeSpawns.Count)];
        SpawnAt(idx);
    }
    
    void HandleComplete(GameObject npc, int idx)
    {
        NPCData data = activeNPCs.Find(n => n.npc == npc);
        if (data != null) activeNPCs.Remove(data);
        
        Destroy(npc);
        freeSpawns.Add(idx);
        StartCoroutine(Respawn());
    }
    
    void HandleFailed(GameObject npc, int idx)
    {
        NPCData data = activeNPCs.Find(n => n.npc == npc);
        if (data != null) activeNPCs.Remove(data);
        
        Destroy(npc);
        freeSpawns.Add(idx);
        StartCoroutine(Respawn());
    }
    
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnCooldown);
        
        if (activeNPCs.Count < maxActiveNPCs)
            SpawnRandom();
    }
    
    public void ClearAll()
    {
        foreach (NPCData data in activeNPCs)
        {
            if (data.npc != null) Destroy(data.npc);
        }
        activeNPCs.Clear();
        
        freeSpawns.Clear();
        for (int i = 0; i < spawnPoints.Length; i++)
            freeSpawns.Add(i);
    }
}
