using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private Camera eyes;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float smooth;
    float _smoothValx, _smoothValZ; // used in input smoothing
    [SerializeField]
    private float jumpHeight = 10f;
    [SerializeField]
    private float gravity = -9.81f;
    private Vector3 playerVelocity;
    private bool grounded;
    float mRotationY = 0f;
    [SerializeField]
    private float lookSensitivity;

    public AudioSource ilkSeslendirme;
    //private bool isPlaying = false;

    public int Yellowscore = 0;
    public int Redscore = 0;
    public int Bluescore = 0;
    public int Greenscore = 0;

    public GameObject room2door1;
    public GameObject room2door2;
    public GameObject room1door1;
    public GameObject room1door2;
    // Start is called before the first frame update
    void Start()
    {
        room2door1.SetActive(false);
        room2door2.SetActive(false);
        room1door1.SetActive(false);
        room1door2.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        grounded = true;
        StartCoroutine(PlaySoundAndWait());
    }

    // Update is called once per frame
    void Update()
    {
        if (Yellowscore >= 3 && Redscore >= 3 && Bluescore >= 3 && Greenscore >= 3)
        {
            room2door1.SetActive(true);
            room2door2.SetActive(true);
        }

        Movement();
        Camera();
    }

    public void Movement()
    {
        //walking
        grounded = controller.isGrounded;
        Vector2 smoothedInput = SmoothedInput();
        float horizontal = smoothedInput.x;
        float vertical = smoothedInput.y;
        Vector3 move = transform.forward * vertical * speed + transform.right * horizontal * speed;

        move.y = 0f;
        controller.Move(move * Time.deltaTime);

        if (grounded)
        {
            playerVelocity.y = -1f; // keeps player tied to ground
        }

        // Jump
        if (Input.GetButton("Jump") && grounded)
        {
            playerVelocity.y = 0f;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -1f * gravity);
        }

        var flags = controller.Move(playerVelocity * Time.deltaTime); // jump and save collision flags
        bool collidedUp = (flags & CollisionFlags.CollidedAbove) != 0; // check if hit head
        if (collidedUp)
        {
            playerVelocity.y = 0f;
        }
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime); //gravity

    }

    public void Camera()
    {
        float mouse_x = Input.GetAxis("Mouse X");
        float mouse_y = Input.GetAxis("Mouse Y");
        transform.Rotate(0f, mouse_x * lookSensitivity, 0f);
        mRotationY += mouse_y * lookSensitivity;
        mRotationY = Mathf.Clamp(mRotationY, -90f, 90f);
        eyes.transform.localEulerAngles = new Vector3(-mRotationY, eyes.transform.localEulerAngles.y, eyes.transform.localEulerAngles.z);

    }


    public Vector2 SmoothedInput()
    {
        float dead = 0.001f;
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        float rX, ry;

        //Horizontal
        float targetx = input.x;
        _smoothValx = Mathf.MoveTowards(_smoothValx, targetx, smooth * Time.unscaledDeltaTime);
        rX = (Mathf.Abs(_smoothValx) < dead) ? 0f : _smoothValx;

        //Vertical
        float targety = input.y;
        _smoothValZ = Mathf.MoveTowards(_smoothValZ, targety, smooth * Time.unscaledDeltaTime);
        ry = (Mathf.Abs(_smoothValZ) < dead) ? 0f : _smoothValZ;

        return new Vector2(rX, ry);
    }
    private IEnumerator PlaySoundAndWait()
    {
        speed = 0; // Ses çalarken hýz sýfýrlanýr
        jumpHeight = 0;
        //isPlaying = true;

        ilkSeslendirme.Play(); // Ses dosyasýný çal
        yield return new WaitForSeconds(ilkSeslendirme.clip.length); // Sesin uzunluðu kadar bekle

        speed = 10; // Ses bittikten sonra hýzý 10 yap
        jumpHeight = 2;
        //isPlaying = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SonKapi"))
        {
            SceneManager.LoadScene(0);
        }
        if (other.gameObject.CompareTag("DeathWall"))
        {
            
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "DoorArea")
        {
            Debug.Log("Alandasýn");

            if (Input.GetKeyUp(KeyCode.F))
            {
                Animator anim = other.GetComponentInParent<Animator>();
                anim.SetTrigger("OpenDoor");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "DoorArea")
        {
            Animator anim = other.GetComponentInParent<Animator>();
            anim.SetTrigger("CloseDoor");
        }
    }

    

    public void YellowScore()
    {
        if (Yellowscore < 3)
        {
            Yellowscore++;
            Debug.Log("Sarý skor +1");
        }
        
        if (Yellowscore == 3)
        {
            Debug.Log("Sarý Kazan Tamamlandý");
        }
    }
    public void RedScore()
    {
        if (Redscore < 3)
        {
            Redscore++;
            Debug.Log("Kýrmýzý skor +1");
        }
        
        if (Redscore == 3)
        {
            Debug.Log("Kýrmýzý Kazan Tamamlandý");
        }
    }
    public void BlueScore()
    {
        if (Bluescore < 3)
        {
            Bluescore++;
            Debug.Log("Mavi skor +1");
        }
        
        if (Bluescore == 3)
        {
            Debug.Log("mavi Kazan Tamamlandý");
        }
    }
    public void GreenScore()
    {
        if (Greenscore < 3)
        {
            Greenscore++;
            Debug.Log("Yeþil skor +1");
        }

        if (Greenscore == 3)
        {
            Debug.Log("Yeþil Kazan Tamamlandý");
        }
    }

    public void Room1DoorOpen()
    {
        room1door1.SetActive(true);
        room1door2.SetActive(true);
    }
}
