using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class KitchenBehavior : MonoBehaviour
{
    public List<int> ListeAttente;
    public Button BoutonAjoutRecette;

    class Recette
    {
        public string Name;
        public string TempsPreparation;
        public string Priorite;
        public string Etat;

        public Recette(string name, string tempsPreparation, string priorite, string etat)
        {
            Name = name;
            TempsPreparation = tempsPreparation;
            Priorite = priorite;
            Etat = etat;
        }
    }
    // Pour faire une nouvelle recette => Recette nomDeLaRecette = new Recette("...","...","...");

    public void AjouterListeAttente()
    {
        // Revoir, surment pas bonne méthode ==> lag à la fin de l'execution
        Recette Recette1 = new Recette("Name", "TempsPreparation", "Priorite","Etat");
        List<Recette> ListeAttente = new List<Recette>();
        ListeAttente.Add(Recette1);
        if(ListeAttente.Count > 0) 
            Debug.Log("Ajout à la liste d'attente");
    }

    public void Cuisine()
    {

    }

    void Update()
    {
        BoutonAjoutRecette.onClick.AddListener(AjouterListeAttente);
    }
}
