using UnityEngine;
using UnityEngine.UI;

namespace MainScript
{
    public class Score : MonoBehaviour
    {
        public Transform player;
        public Text scoretext;
        public int score;
        public int highestscore;
        public bool cliffFell = false; // check if player fell off the cliff
        public bool keepupdate = true; // keep updating score if true, if false, score will check if level pass or fail
        public int collisionrandomtext;

        public void Start()
        {
            collisionrandomtext = Random.Range(1, 5);
        }

        // Update is called once per frame
        void Update()
        {
            if (keepupdate)
            {
                if (FindObjectOfType<GameManager>().gameplay)
                {
                    score = (int)player.position.z;
                    score *= 15;
                    scoretext.text = score.ToString();
                    highestscore = score;
                }

                else
                {
                    if (FindObjectOfType<GameManager>().gamerunning && !FindObjectOfType<CollisionScript>().collided)
                    {
                        scoretext.text = "Level Passed!";
                    }

                    if (FindObjectOfType<CollisionScript>().collided)
                    {
                        if (collisionrandomtext == 1)
                        {
                            scoretext.text = "BOOOOOMM!!";
                        }

                        else if (collisionrandomtext == 2)
                        {
                            scoretext.text = "Uh Ohh! My head!!";
                        }

                        else if (collisionrandomtext == 3)
                        {
                            scoretext.text = "BANGGG!!";
                        }

                        else if (collisionrandomtext == 4)
                        {
                            scoretext.text = "ehehehe, where am I?";
                        }

                        else if (collisionrandomtext == 5)
                        {
                            scoretext.text = "Ouch!!";
                        }
                    }
                }
            }
        }

        public void CliffFall()
        {
            if (!cliffFell)
            {
                scoretext.text = "You fell off the cliff!";
                cliffFell = true;
                keepupdate = false;
                FindObjectOfType<GameManager>().gameplay = true;
                FindObjectOfType<GameManager>().GameOver();
            }
        }
    }
}
