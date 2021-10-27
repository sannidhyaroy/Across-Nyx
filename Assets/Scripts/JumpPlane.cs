using UnityEngine;

namespace MainScript
{
    public class JumpPlane : MonoBehaviour
    {

        [SerializeField] private float JumpForce;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "Player")
            {
                Debug.Log("Player has collided with the Jump Plane");
                FindObjectOfType<GameScript>().rb.AddForce(0, JumpForce * Time.deltaTime, 0, ForceMode.VelocityChange);
                FindObjectOfType<AudioManager>().Play("yaa");
            }
        }
    }
}
