using UnityEngine;

public class Win_Defeat : MonoBehaviour
{
    public int Victory_status;

    public Win_Defeat(int status)
    {
        Victory_status = status;
    }

    void Start()
    {
        Victory_status = 0;
    }

    void Update()
    {
        if (Victory_status == 2)
        {
            Debug.Log("Defaite , vous venez de perdre la partie");
            Victory_status = 3;
        } 
        else if (Victory_status == 1)
        {
            Victory_status = 3;
        }
    }

    public void Set_victory_status(int vic)
    {
        Victory_status = vic;
    }

    public int Get_victory_status() 
    { 
        return Victory_status; 
    }
}
