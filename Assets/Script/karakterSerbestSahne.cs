using System.Collections;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

//using UnityEngine.Windows;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class karakterSerbestSahne : MonoBehaviour
{
    private Rigidbody karakterRb;
    private Animator karakterAnim;
    public ParticleSystem kanama;
    public GameObject mermi;
    public GameObject et;
    public GameObject dusman;
    public GameObject Panel;
    public bool GameOver = false;
    public bool PowerStart = false;

    public Transform karakter; // Karakterin transformu
    public Transform inekTutmaNoktasi; // İneğin taşınacağı nokta (karakterin eline yakın bir yer)
    private GameObject seciliInek = null; // Şu anda taşınan inek
    private bool inekAlindi = false; // İnek alındı mı?
    
    //Hareket
    public Camera kamera;
    private Vector3 playerVelocity;
    public bool yerdeMi = true;
    private float mRotationY = 0f;
    public float mouseSensitivity;
    public float yerCekimi = -9.81f;
    public float yukseklik = 10;
    public int hiz = 50;

    public float horsePower = 0f;

    
    void Start()
    {
        Panel.SetActive(false);
        karakterRb = GetComponent<Rigidbody>();
        karakterAnim = GetComponent<Animator>();
        Physics.gravity *= yerCekimi;
        InvokeRepeating("Spawn", 2, 1);
        Cursor.lockState = CursorLockMode.Locked; // Mouse imlecini ekranın ortasında kilitler
    }
    void FixedUpdate()
    {
       Hareket();
    }
    void Update()
    {
        Kamera();    
        if (Input.GetKeyDown(KeyCode.K)) { mermiAt(); }
        if (Input.GetKeyDown(KeyCode.L)) { etAt(); }
        if (Input.GetKeyDown(KeyCode.Space) && yerdeMi && !GameOver)
        { 
            karakterRb.AddForce(Vector3.up * yukseklik* Time.deltaTime, ForceMode.Impulse); 
            yerdeMi = false;
            karakterAnim.SetTrigger("Jump_t");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inekAlindi)
            {
                Birak(); // Eğer ineği aldıysak bırak
            }
            else
            {
                Al(); // Eğer ineği almadıysak al
            }
        }    
    }
    void Kamera()
    {
        float mouse_x = Input.GetAxis("Mouse X");
        float mouse_y = Input.GetAxis("Mouse Y");
        transform.Rotate(0f, mouse_x * mouseSensitivity, 0f);
        mRotationY += mouse_y * mouseSensitivity;
        mRotationY = Mathf.Clamp(mRotationY, -15f, 10f);
        kamera.transform.localEulerAngles = new Vector3(-mRotationY, kamera.transform.localEulerAngles.y, kamera.transform.localEulerAngles.z);
    }
    void Hareket()
    {
        if (GameOver == false)
        {
            //Vector3 hareket = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * hiz;
            //karakterRb.linearVelocity = transform.TransformDirection(hareket) * hiz;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            karakterRb.AddRelativeForce(Vector3.forward * verticalInput * horsePower);
            transform.Rotate(Vector3.up * horizontalInput * hiz * Time.deltaTime);
        }
    }
    void Al()
    {
        Collider[] yakinNesneler = Physics.OverlapSphere(karakter.position, 2f); // Karakterin etrafındaki nesneleri kontrol et (yarıçap: 2 birim)
        foreach (Collider nesne in yakinNesneler)
        {
            if (nesne.CompareTag("Inek")) // Eğer nesne bir inekse
            {
                seciliInek = nesne.gameObject; // İneği seç
                seciliInek.GetComponent<Rigidbody>().isKinematic = true; // Fiziği kapat, hareket etmeyecek
                seciliInek.transform.position = inekTutmaNoktasi.position; // İneği taşıma noktasına yerleştir
                seciliInek.transform.SetParent(inekTutmaNoktasi); // Karaktere bağla
                inekAlindi = true; // Artık bir inek taşıyoruz
                break;
            }
        }
    }
    void Birak()
    {
        if (seciliInek != null)
        {
            seciliInek.GetComponent<Rigidbody>().isKinematic = false; // Fiziği geri aç
            seciliInek.transform.SetParent(null); // Karakterden ayır
            seciliInek = null; // İnek artık taşınmıyor
            inekAlindi = false;
        }
    }
    public void mermiAt()
    {
        Vector3 spawnPosition = transform.position + transform.forward * 1.5f;
        Instantiate(mermi, spawnPosition, transform.rotation);
    }
    public void etAt()
    {
        Vector3 spawnPosition = transform.position + transform.forward * 1.5f;
        Instantiate(et, spawnPosition, transform.rotation);
    }
    public void Spawn()
    {
        Vector3 spawnPosition = new (Random.Range(1f, 42f), 0, 46);
        Quaternion spawnRotation = Quaternion.Euler(0, 180, 0); // 180 derece d�nd�rmek i�in
        Instantiate(dusman, spawnPosition, spawnRotation);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            yerdeMi = true;
        }
        else if (collision.gameObject.CompareTag("Dusman"))
        {
            GameOver = true;
            CancelInvoke("Spawn");
            karakterAnim.SetBool("Death_b", true);
            karakterAnim.SetInteger("DeathType_int", 2);
            kanama.Play();
            Panel.SetActive(true);
        }
        else if (collision.gameObject.CompareTag("Hayvan")&&PowerStart)
        {
            Debug.Log("Collided with " + collision.gameObject.name);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Power"))
        {
            PowerStart = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpSuresi(5));
        }
    }

    IEnumerator PowerUpSuresi(int x)
    {
        yield return new WaitForSeconds(x); 
        PowerStart = false;
    } 

}
