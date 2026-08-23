using UnityEngine;

public class GameSpawn2 : MonoBehaviour
{
    public float threshold;

    void FixedUpdate()

    {
    if(transform.position.y < threshold)
        {
            transform.position = new Vector3(50f,0.25f,20.5f);
        }
    }
}
