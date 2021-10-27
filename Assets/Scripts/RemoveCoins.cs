using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCoins : MonoBehaviour
{
    public void OnCollisionExit(Collision collisioninfo)
    {
        if (collisioninfo.collider.tag == "Coins")
        {
            Destroy(this.gameObject);
        }
    }
}
