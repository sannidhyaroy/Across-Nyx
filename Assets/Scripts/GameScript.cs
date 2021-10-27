using UnityEngine;

namespace MainScript
{
    public class GameScript : MonoBehaviour
    {
        public Rigidbody rb;
        public float force = 500f;
        public float playerspeed = 7f;
        public Joystick joystick;
        [SerializeField] private float joystickspeed = 10f;
        public float gyroinput;
        public float gyromovespeed = 30f;
        public float dirX;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            rb.transform.position += new Vector3(0, 0, force) * Time.deltaTime;
            //rb.AddForce(0, 0, force * Time.deltaTime);
            //transform.Translate(force * Time.deltaTime, 0, 0);

            if (FindObjectOfType<GameManager>().joystickUI.activeInHierarchy)
            {
                //Debug.Log("Joystick is active!");
                //rb.transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * joystickspeed * Time.deltaTime;
                //rb.AddForce(joystickspeed * joystick.Horizontal * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
                JoystickMove(joystick.Horizontal);
            }

            if (FindObjectOfType<GameManager>().gyroactive == 1)
            {
                gyroinput = Input.acceleration.x;
                dirX = gyroinput * gyromovespeed;
                //Debug.Log("Gyroinput: "+gyroinput);
                //Debug.Log("DirX: "+dirX);
                //rb.AddForce(directionalforce * dirX, 0, 0, ForceMode.VelocityChange);
                transform.Translate(gyroinput * gyromovespeed * Time.deltaTime, 0, 0);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                KeyPress();
                //rb.AddForce(-directionalforce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            }

            else if (Input.GetKey(KeyCode.RightArrow))
            {
                KeyPress();
                //rb.AddForce(directionalforce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            }

            else if (Input.GetKey(KeyCode.A))
            {
                KeyPress();
                //rb.AddForce(-directionalforce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            }

            else if (Input.GetKey(KeyCode.D))
            {
                KeyPress();
                //b.AddForce(directionalforce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
            }

            if (FindObjectOfType<Score>().score == 200)
            {
                FindObjectOfType<AudioManager>().Play("Laugh 1");
            }
        }

        private void KeyPress()
        {
            rb.transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * playerspeed * Time.deltaTime;
        }

        private void JoystickMove(float joystickinput)
        {
            transform.Translate(joystickinput * joystickspeed * Time.deltaTime, 0, 0);
        }
    }
}
