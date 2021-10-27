using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSphere : MonoBehaviour
{

    int layerId = 9;
    int layerMask;
    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << layerId;
    }

    // Update is called once per frame
    /*void FixedUpdate()
    {
        Collider[] hitcoins;
        hitcoins = Physics.OverlapSphere(transform.position, 80, layerMask); // Should probably add layermask and a triggerquery

        for (int i = hitcoins.Length - 1; i > -1; i--)
        {
            ObjectComponentState(hitcoins[i], true);
        }
    }*/
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Request Coins appear");
        ObjectComponentState(collider, true);
        Debug.Log("Coins appeared!");
    }

    private void OnTriggerExit(Collider collider)
    {
        Debug.Log("Request Coins disappear");
        ObjectComponentState(collider, false);
        Debug.Log("Coins disappered!");
    }

    private void ObjectComponentState(Collider collisioninfo, bool state)
    {
        //collider.GetComponentInChildren<MonoBehaviour>(true).enabled = state;
        //collider.GetComponent<MonoBehaviour>(true).enabled = state;
        Debug.Log(collisioninfo.name + " is SetActive to " + state);
        collisioninfo.gameObject.SetActive(state);
    }
}
