using System.Collections;
using UnityEngine;

namespace MainScript
{
    public class GroundSpawner : MonoBehaviour
    {
        public GameObject Ground;
        private GameObject SpawnedGround;
        public int numberOfGrounds;
        private int GroundsCreated;
        private float ScaleZ;
        //private float positionZ;
        //private int LastPlayerPosition = -1;
        private Vector3 SpawnPosition = Vector3.zero;
        // Start is called before the first frame update
        void Start()
        {
            ScaleZ = Ground.transform.lossyScale.z;
            //positionZ = -ScaleZ;
            GroundsCreated = 0;
            StartCoroutine("GroundSpawn");
        }
        void FixedUpdate()
        {
            /*if ((int)Mathf.Floor(FindObjectOfType<Score>().player.transform.position.z % (ScaleZ * (numberOfGrounds / 2))) == 0 && LastPlayerPosition != (int)Mathf.Floor(FindObjectOfType<Score>().player.transform.position.z))
            {
                LastPlayerPosition = (int)Mathf.Floor(FindObjectOfType<Score>().player.transform.position.z);
                GroundsCreated = 0;
                StartCoroutine("GroundSpawn");
                //Debug.Log("Ground Created variable resetted to 0");
                //GroundSpawn();
            }*/
            if (Vector3.Distance(FindObjectOfType<Score>().player.transform.position, SpawnedGround.transform.position) < 70)
            {
                GroundsCreated = 0;
                StartCoroutine("GroundSpawn");
            }
        }
        IEnumerator GroundSpawn()
        {
            while (GroundsCreated < numberOfGrounds)
            {
                //positionZ += ScaleZ;
                //SpawnPosition = new Vector3(0, 0, positionZ);
                SpawnedGround = Instantiate(Ground, SpawnPosition, Quaternion.identity);
                SpawnPosition.z += ScaleZ;
                //Debug.Log("Object spawned at " + positionZ);
                GroundsCreated++;
                yield return null;
            }
        }
    }
}
