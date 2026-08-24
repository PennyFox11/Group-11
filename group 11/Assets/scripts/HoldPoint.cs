using UnityEngine;

public class HoldPoint : MonoBehaviour
{
    public static HoldPoint instance { get; private set; }    
    private void Awake()
    {
     if (instance == null)
        {
            instance = this;
        }
     else
        {
            Destroy(this);
        }


    }
}
