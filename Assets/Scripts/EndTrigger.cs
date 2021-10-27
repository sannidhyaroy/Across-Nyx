using UnityEngine;

namespace MainScript
{
    public class EndTrigger : MonoBehaviour
    {

        public GameManager gameManager;
        public GameScript movement;

        public void OnTriggerEnter(Collider collider)
        {
            if (collider.tag == "Player")
            {
                movement.enabled = false;
                //gameManager.WinAnimation();
                FindObjectOfType<AudioManager>().Play("Yohoo!");
                FindObjectOfType<AudioManager>().PlayDelayed("Excellent!", 1);
                gameManager.LevelCompleteWait();
            }
        }
    }
}
