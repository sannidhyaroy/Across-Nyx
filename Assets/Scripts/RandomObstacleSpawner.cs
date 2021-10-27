using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainScript
{
    public class RandomObstacleSpawner : MonoBehaviour
    {
        public GameObject Obstacle;
        public int numberOfObstacles;
        private int ObstaclesCreated = 0;
        private float positionX;
        private float positionY;
        private float positionZ;
        public float FromPositionX;
        public float ToPositionX;
        private float FromPositionZ;
        private float ToPositionZ;
        private int LastPlayerPosition = 0;
        public float FromPositionZOffset;
        public float ToPositionZOffset;
        [SerializeField] private float MinDistance;
        private Vector3 SpawnPosition;
        private Vector3[] SpawnPoints;
        // Start is called before the first frame update
        void Start()
        {
            SpawnPoints = new Vector3[numberOfObstacles];
            //ObstaclesCreated = 0;
            //StartCoroutine("ObstacleSpawn");
        }
        void FixedUpdate()
        {
            if ((int)Mathf.Floor(FindObjectOfType<Score>().player.transform.position.z % (ToPositionZOffset-FromPositionZOffset)) == 0 && LastPlayerPosition != (int)Mathf.Floor(FindObjectOfType<Score>().player.transform.position.z))
            {
                LastPlayerPosition = (int)Mathf.Floor(FindObjectOfType<Score>().player.transform.position.z);
                FromPositionZ = FindObjectOfType<Score>().player.transform.position.z + FromPositionZOffset;
                ToPositionZ = FindObjectOfType<Score>().player.transform.position.z + ToPositionZOffset;
                ObstaclesCreated = 0;
                StartCoroutine("ObstacleSpawn");
                //ObstacleSpawn();
            }
        }
        IEnumerator ObstacleSpawn()
        {
            while (ObstaclesCreated < numberOfObstacles)
            {
                if (!FindObjectOfType<GameManager>().gameplay)
                {
                    break;
                }
                positionX = Random.Range(FromPositionX, ToPositionX);
                positionY = Obstacle.transform.position.y;
                positionZ = Random.Range(FromPositionZ, ToPositionZ);
                SpawnPosition = new Vector3(positionX, positionY, positionZ);

                for (int i = 0; i <= ObstaclesCreated - 1; i++)
                {
                    if (Vector2.Distance(new Vector2(SpawnPosition.x, SpawnPosition.z), new Vector2(SpawnPoints[i].x, SpawnPoints[i].z)) < MinDistance)
                    {
                        positionX = Random.Range(FromPositionX, ToPositionX);
                        positionZ = Random.Range(FromPositionZ, ToPositionZ);
                        SpawnPosition = new Vector3(positionX, positionY, positionZ);
                        Debug.Log("Cancel Spawn Position! Obstacles");
                        i = 0;
                    }
                }

                Instantiate(Obstacle, SpawnPosition, Quaternion.identity);
                ObstaclesCreated++;
                SpawnPoints[ObstaclesCreated - 1] = SpawnPosition;
                yield return null;
            }
        }
    }
}
