using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenBehavior : MonoBehaviour
{
    [System.Serializable]
    public class Recette
    {
        public string Name;
        public float TempsPreparation; // en secondes (durée totale)
        public float RemainingTime; // temps restant (se met à jour lors de la préparation)

        public Recette(string name, float tempsPreparation)
        {
            Name = name;
            TempsPreparation = tempsPreparation;
            RemainingTime = tempsPreparation;
        }
    }

    // File d'attente visible dans l'inspector
    public List<Recette> ListeAttente = new List<Recette>();

    // flag pour savoir si la cuisine est en train de traiter
    private bool isProcessing = false;

    // Ajoute une recette à la file d'attente et lance le traitement si nécessaire
    public void AjouterRecetteQueue(string nom, float tempsEnSecondes)
    {
        var r = new Recette(nom, tempsEnSecondes);
        ListeAttente.Add(r);
        UpdateQueueUI();

        if (!isProcessing)
            StartCoroutine(CuisineCoroutine());
    }

    // Coroutine qui traite la file d'attente séquentiellement
    private IEnumerator CuisineCoroutine()
    {
        isProcessing = true;

        while (ListeAttente.Count > 0)
        {
            Recette current = ListeAttente[0];

            // décrémente RemainingTime pour la recette en cours
            while (current.RemainingTime > 0f)
            {
                current.RemainingTime -= Time.deltaTime;
                if (current.RemainingTime < 0f) current.RemainingTime = 0f;
                UpdateQueueUI();
                yield return null;
            }

            ListeAttente.RemoveAt(0);
            UpdateQueueUI();

            yield return null;
        }

        isProcessing = false;
    }

    // Méthode publique pour obtenir l'état formaté de la file (ordre + temps restant)
    public string GetQueueStatusString()
    {
        if (ListeAttente.Count == 0)
            return "File d'attente : (vide)";

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("File d'attente :");
        for (int i = 0; i < ListeAttente.Count; i++)
        {
            var r = ListeAttente[i];
            sb.AppendLine($"{i + 1}. {r.Name} - {r.RemainingTime:0.#}s restantes");
        }

        return sb.ToString();
    }

    // Méthode interne : prévue pour mise à jour UI si besoin.
    // Actuellement vide car l'affichage est géré par AfficherUI_CarnetRecette.
    private void UpdateQueueUI()
    {
        // Intentionnellement vide — pas de log ni affichage direct ici.
    }
}
