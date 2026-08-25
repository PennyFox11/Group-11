using UnityEngine;
//Akhona Khoali
// this script is to hold the point of the player when they are interacting with an object. This is to prevent the player from moving while interacting with an object. The player will be able to move again when they are done interacting with the object.
//this is connected to the player pickup script and the player movement script. The player pickup script will set the hold point to true when the player is interacting with an object and the player movement script will check if the hold point is true before allowing the player to move.

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
