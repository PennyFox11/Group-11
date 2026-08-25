using UnityEngine;

using UnityEngine.InputSystem;
//Akhona Khoali
//this script is added to all pickable objects in the scene. It allows the player to pick up and throw objects. It is used in together with the PlayerPickupController script. The player can pick up an object by pressing E when they are near it. The player can throw the object by pressing the Throw button (default is left mouse button). The player can drop the object by pressing E again. The object will be held at a HoldPoint, which is a child of the player. The HoldPoint is set in the PlayerPickupController script.

public class PickUp : MonoBehaviour
{
    
    bool IsHolding = false;
    [SerializeField]
    float throwForce = 600f;
    [SerializeField]
    float MaxDistance = 5f;
    float distance;
    HoldPoint holdPoint;
    Rigidbody rb;
    Vector3 objectpos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        holdPoint = HoldPoint.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsHolding)
            Hold();
    }

    // Called by PlayerPickupController when this is the nearest object and the player presses E
    public void Interact()
    {
        if (!IsHolding)
        {
            // pick up
            {
                if (holdPoint != null)
                {
                    IsHolding = true;
                    rb.useGravity = false;
                    rb.detectCollisions = true;

                    this.transform.SetParent(holdPoint.transform);
                }
                else
                {
                    Debug.Log("HoldPoint instance is null. To make sure there is a HoldPoint object in the scene.");
                }
            }
        }
        else
        {
            // drop
            IsHolding = false;
            rb.useGravity = true;
            this.transform.SetParent(null);
        }
    }

    private void Hold()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    // Called by PlayerPickupController when this object is currently held and the player presses Throw
    public void ThrowObject()
    {
        if (!IsHolding) return;

        //  throw
    }
}