using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MainScript
{
    public class CoinsController : MonoBehaviour
    {
        [SerializeField]
        public int CoinCollected;
        public float radius;
        public float PickupSpeed;

        [SerializeField]
        private float PickupDistance;

        [SerializeField]
        private string CoinsTag;

        public TextMeshProUGUI CoinsCollected;
        int layerId = 9;
        int layerMask;

        private List<Transform> coins;

        // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.        void Start()
        private void Start()
        {
            layerMask = 1 << layerId;
            CoinsCollected.text = "0";
            coins = new List<Transform>();
        }

        //Update is called every frame, if the MonoBehaviour is enabled.
        void FixedUpdate()
        {
            if (FindObjectOfType<GameManager>().gameplay)
            {
                /*GameObject[] Coins = GameObject.FindGameObjectsWithTag("Coins");
                Vector3 coinPos = transform.InverseTransformPoint(coins[i].transform.position);
                if (coinPos.y < 0)
                {
                    Destroy(coins[i].gameObject);
                    coins.Remove(coins[i]);
                }*/
                
                var colliders = Physics.OverlapSphere(transform.position, radius, layerMask);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (!coins.Contains(colliders[i].transform) && colliders[i].CompareTag(CoinsTag))
                    {
                        coins.Add(colliders[i].transform);
                    }
                }

                for (int i = 0; i < coins.Count; i++)
                {
                    if (Vector3.Distance(transform.position, coins[i].position) <= PickupDistance)
                    {
                        CoinCollected++;
                        CoinsCollected.text = CoinCollected.ToString();
                        FindObjectOfType<AudioManager>().Play("Coin Collect");
                        Destroy(coins[i].gameObject);
                        coins.Remove(coins[i]);
                    }
                    else
                    {
                        float step = (Time.deltaTime * PickupSpeed);
                        if (transform.position.y > 2)
                        {
                            coins[i].position = Vector3.MoveTowards(coins[i].position, transform.position, step);
                        }
                        else
                        {
                            Vector3 PlayerPosition = new Vector3(transform.position.x, 1, transform.position.z);
                            coins[i].position = Vector3.MoveTowards(coins[i].position, PlayerPosition, step);
                        }
                    }
                }
            }
        }
    }
}
