using UnityEngine;

namespace MainScript
{
    public class Followplayer : MonoBehaviour
    {

        public Transform player;
        public Vector3 cameraoffset;

        // Update is called once per frame
        void Update()
        {
            transform.position = player.position + cameraoffset;
        }

        public void CameraRotation()
        {
            float speed = 10F;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * speed);
        }
    }
}
