using UnityEngine;


public class Win_Defeat : MonoBehaviour
{
    public int Victory_status;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Win_Defeat(int victory_status)
    {
        Victory_status = victory_status;
    }

    void Start()
    {
        // 0 Correspond à neutral , 1 à Win , 2 Perdu , 3 awaiting status.
        Victory_status = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Defaite
        if (Victory_status == 2)
        {
            // 1 time action.

            Debug.Log("Defaite , vous venez de perdre la partie");

            // On mais le awaiting status pour éviter d'avoir plusieurs fois le message.
            Victory_status = 3;

        } else if (Victory_status == 1) // Victoire
        {

            // Idem
            Victory_status = 3;
        } 

        else // Neutral

        {

        }
        
    }

    /// <summary>
    /// Un setter basic qui permet d'initaliser une nouvelle valeur.
    /// </summary>
    /// <param name="vic">La nouvelle valeur.</param>
    public void Set_victory_status(int vic)
    {
        Victory_status = vic;
    }

    /// <summary>
    /// Un getter pour récup la valeur
    /// </summary>
    /// <returns>la valeur</returns>
    public int Get_victory_status() 
    { 
        return Victory_status; 
    }
}
