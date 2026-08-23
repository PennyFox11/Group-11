using UnityEngine;

public class GameRespawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float threshold;

    private void FixedUpdate()
    {
        if (transform.position.y < threshold)
        {
            transform.position = new Vector3(58.5f, 1f, 17f);
        }
    }
}

