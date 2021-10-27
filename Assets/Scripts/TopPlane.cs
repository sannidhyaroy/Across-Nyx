using UnityEngine;

namespace MainScript
{
    public class TopPlane : MonoBehaviour
    {
        public bool UpdateYPosition = true;
        [SerializeField] private Animator animator;
        private void OnCollisionEnter(Collision collisioninfo)
        {
            if (collisioninfo.collider.tag == "Player")
            {
                FindObjectOfType<GameManager>().animator.SetFloat("Vertical Position", 1);
                animator.SetBool("Top Plane Mode", true);
                animator.SetTrigger("Top Plane");
                UpdateYPosition = false;
            }
        }
        private void OnCollisionExit(Collision collisioninfo)
        {
            if (collisioninfo.collider.tag == "Player")
            {
                FindObjectOfType<GameManager>().animator.SetFloat("Vertical Position", FindObjectOfType<GameScript>().rb.transform.position.y);
                animator.SetBool("Top Plane Mode", false);
                animator.ResetTrigger("Top Plane");
                UpdateYPosition = true;
            }
        }
    }
}
