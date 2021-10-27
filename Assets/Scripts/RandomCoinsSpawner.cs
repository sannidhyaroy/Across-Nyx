using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainScript
{
    public class RandomCoinsSpawner : MonoBehaviour
    {
        public GameObject Coin;
        public int numberOfCoins;
        private int CoinsCreated = 0;
        private float positionX;
        private float positionZ;
        public float FromPositionX;
        public float ToPositionX;
        private float FromPositionZ;
        private float ToPositionZ;
        private int LastPlayerPosition = 0;
        public float FromPositionZOffset;
        public float ToPositionZOffset;
        private Vector3 SpawnPosition;
        private Vector3[] SpawnPoints;
        // Start is called before the first frame update
        void Start()
        {
            SpawnPoints = new Vector3[numberOfCoins];
        }
        void FixedUpdate()
        {
            if ((int)Mathf.Floor(FindObjectOfType<Score>().player.transform.position.z % (ToPositionZOffset - FromPositionZOffset)) == 0 && LastPlayerPosition != (int)Mathf.Floor(FindObjectOfType<Score>().player.transform.position.z))
            {
                LastPlayerPosition = (int)Mathf.Floor(FindObjectOfType<Score>().player.transform.position.z);
                FromPositionZ = FindObjectOfType<Score>().player.transform.position.z + FromPositionZOffset;
                ToPositionZ = FindObjectOfType<Score>().player.transform.position.z + ToPositionZOffset;
                CoinsCreated = 0;
                //StartCoroutine("CoinSpawn");
                CoinSpawn();
            }
        }
        void CoinSpawn()
        {
            while (CoinsCreated < numberOfCoins)
            {
                if (!FindObjectOfType<GameManager>().gameplay)
                {
                    break;
                }
                positionX = Random.Range(FromPositionX, ToPositionX);
                positionZ = Random.Range(FromPositionZ, ToPositionZ);
                SpawnPosition = new Vector3(positionX, FindObjectOfType<Score>().player.position.y + 0.5f, positionZ);

                for (int i = 0; i <= CoinsCreated - 1; i++)
                {
                    if (Vector3.Distance(SpawnPosition, SpawnPoints[i]) < 1)
                    {
                        positionX = Random.Range(FromPositionX, ToPositionX);
                        positionZ = Random.Range(FromPositionZ, ToPositionZ);
                        SpawnPosition = new Vector3(positionX, FindObjectOfType<Score>().player.position.y + 0.5f, positionZ);
                        //Debug.Log("Cancel Spawn Position");
                        i = 0;
                    }
                }

                Instantiate(Coin, SpawnPosition, Quaternion.Euler(90, 0, 0));
                CoinsCreated++;
                SpawnPoints[CoinsCreated - 1] = SpawnPosition;
                //yield return null;
            }
        }
    }
}
