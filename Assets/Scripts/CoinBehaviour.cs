using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainScript
{
    public class CoinBehaviour : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            transform.Rotate(0, 0, 90 * Time.deltaTime);
        }
    }
}
