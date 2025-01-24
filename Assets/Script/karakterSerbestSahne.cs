using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private GameObject OdakNoktasi;


    public int hiz = 50;
    public int wiz = 50;

    public float yerCekimi = 2;
    public float yukseklik = 40;
    public bool yerdeMi = true;
    public bool canliMi = true;
    public bool GameOver = false;
    public bool PowerStart = false;

    void Start()
    {
        Panel.SetActive(false);
        karakterRb = GetComponent<Rigidbody>();
        karakterAnim = GetComponent<Animator>();
        Physics.gravity *= yerCekimi;
        InvokeRepeating("Spawn", 2, 1);

        OdakNoktasi=GameObject.Find("KameraObjesi");
    }

    void FixedUpdate()
    {
        float yatayHareket = Input.GetAxis("SSHorizontal");
        transform.Rotate(Vector3.up, Time.deltaTime*wiz*yatayHareket);
        
        if (canliMi == true)
        {
            Vector3 hareket=new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))*Time.deltaTime*hiz;
            karakterRb.linearVelocity=transform.TransformDirection(hareket)*hiz;
        }
        
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) { mermiAt(); }
        if (Input.GetKeyDown(KeyCode.L)) { etAt(); }
        if (Input.GetKeyDown(KeyCode.Space) && yerdeMi && !GameOver) {karakterRb.AddForce(Vector3.up * yukseklik, ForceMode.Impulse); yerdeMi = false; karakterAnim.SetTrigger("Jump_t"); }
    }


    private void OnMouseDown()
    {
        if (gameObject.CompareTag("Dusman"))
        {
            Destroy(gameObject); // Bu nesneyi yok et
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
            canliMi = false;
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
