using UnityEngine;

namespace MainScript
{
    public class CollisionScript : MonoBehaviour
    {
        public GameScript movement;
        public bool collided = false;

        public void OnCollisionEnter(Collision collisioninfo)
        {
            if (collisioninfo.collider.tag == "Obstacles")
            {
                movement.enabled = false;
                //Invoke("Collided", 0.4f);
                collided = true;
                FindObjectOfType<GameManager>().GameOver();
            }
        }

        private void Collided()
        {
            collided = true;
        }

    }
}
