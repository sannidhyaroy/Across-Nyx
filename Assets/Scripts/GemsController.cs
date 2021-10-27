using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MainScript
{
    public class GemsController : MonoBehaviour
    {
        [SerializeField]
        public int GemCollected;
        public float radius;
        public float PickupSpeed;

        [SerializeField]
        private float PickupDistance;

        [SerializeField]
        private string GemsTag;

        public TextMeshProUGUI GemsCollected;
        int layerId = 9;
        int layerMask;

        private List<Transform> Gems;

        // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.        void Start()
        private void Start()
        {
            layerMask = 1 << layerId;
            GemsCollected.text = "0";
            Gems = new List<Transform>();
        }

        //Update is called every frame, if the MonoBehaviour is enabled.
        void FixedUpdate()
        {
            if (FindObjectOfType<GameManager>().gameplay)
            {
                /*GameObject[] Gems = GameObject.FindGameObjectsWithTag("Gems");
                Vector3 GemPos = transform.InverseTransformPoint(Gems[i].transform.position);
                if (GemPos.y < 0)
                {
                    Destroy(Gems[i].gameObject);
                    Gems.Remove(Gems[i]);
                }*/
                
                var colliders = Physics.OverlapSphere(transform.position, radius,layerMask);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (!Gems.Contains(colliders[i].transform) && colliders[i].CompareTag(GemsTag))
                    {
                        Gems.Add(colliders[i].transform);
                    }
                }

                for (int i = 0; i < Gems.Count; i++)
                {
                    if (Vector3.Distance(transform.position, Gems[i].position) <= PickupDistance)
                    {
                        GemCollected++;
                        GemsCollected.text = GemCollected.ToString();
                        FindObjectOfType<AudioManager>().Play("Gem Collect");
                        Destroy(Gems[i].gameObject);
                        Gems.Remove(Gems[i]);
                    }
                    else
                    {
                        float step = (Time.deltaTime * PickupSpeed);
                        if (transform.position.y > 2)
                        {
                            Gems[i].position = Vector3.MoveTowards(Gems[i].position, transform.position, step);
                        }
                        else
                        {
                            Vector3 PlayerPosition = new Vector3(transform.position.x, 1, transform.position.z);
                            Gems[i].position = Vector3.MoveTowards(Gems[i].position, PlayerPosition, step);
                        }
                    }
                }
            }
        }
    }
}
