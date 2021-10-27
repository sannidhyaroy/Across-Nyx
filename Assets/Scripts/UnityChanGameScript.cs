using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainScript
{
    public class UnityChanGameScript : MonoBehaviour
    {

        public Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator.Play("Run Forward");
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                animator.CrossFade("Run Left", 1, -1);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                animator.CrossFade("Run Right", 1, -1);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                animator.CrossFade("Run Left", 1, -1);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                animator.CrossFade("Run Right", 1, -1);
            }

            // Key Animations

            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                RunForward();
            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                RunForward();
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                RunForward();
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                RunForward();
            }
        }

        public void RunForward()
        {
            animator.CrossFade("Run Forward", 1, -1);
        }

        public void RunLeft()
        {
            animator.CrossFade("Run Left", 1, -1);
        }

        public void RunRight()
        {
            animator.CrossFade("Run Right", 1, -1);
        }

        public void RunBackward()
        {
            animator.CrossFade("Run Back", 1, -1);
        }
    }
}
