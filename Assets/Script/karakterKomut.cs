using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class karakterKomut : MonoBehaviour
{   
    private Rigidbody karakterRb;
    private Animator karakterAnim;
    public ParticleSystem kanama;
    public GameObject mermi;
    public GameObject et;
    public GameObject dusman;
    public GameObject Panel;
    

    public TextMeshProUGUI TilkiSayisiText;
    public TextMeshProUGUI skorTabelasý;

    public int TilkiSayisi = 0;
    public int hiz = 50;

    public float yerCekimi=2;
    public bool yerdeMi = true;
    public bool GameOver = false;

    void Start()
    {
        TilkiSayisiText.text = "OLDURULEN TILKI SAYISI: "+TilkiSayisi.ToString();
        skorTabelasý.text= "OLDURULEN TILKI SAYISI: " + TilkiSayisi.ToString();


        Panel.SetActive(false);
        karakterRb =GetComponent<Rigidbody>();
        karakterAnim=GetComponent<Animator>();
        Physics.gravity *= yerCekimi;
        InvokeRepeating("Spawn", 2, 1);
    }

    public void TilkiOldurme()
    {
        TilkiSayisi++;
        TilkiSayisiText.text = "OLDURULEN TILKI SAYISI: " + TilkiSayisi.ToString();
        skorTabelasý.text = "OLDURULEN TILKI SAYISI: " + TilkiSayisi.ToString();

    }




    void Update()
    {
        if (transform.position.x < 0 ){transform.position = new(0, transform.position.y, transform.position.z);}//x kordinatýnda karakterin gidebileceði sýnýrlarý belirler
        if (transform.position.x > 43){transform.position = new(43, transform.position.y, transform.position.z);}
        if (GameOver == false)
        {
            Vector3 hareket = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            transform.Translate(hareket * hiz* Time.deltaTime);
        }
        

        if (Input.GetKeyDown(KeyCode.E)) {mermiAt();}
        if (Input.GetKeyDown(KeyCode.Q)) {etAt();}
        if (Input.GetKeyDown(KeyCode.Space) && yerdeMi && !GameOver) {karakterRb.AddForce(Vector3.up * 10, ForceMode.Impulse); yerdeMi = false; karakterAnim.SetTrigger("Jump_t"); }
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
        Vector3 spawnPosition = new(Random.Range(1f, 42f), 0, 46);
        Quaternion spawnRotation = Quaternion.Euler(0, 180, 0); // 180 derece döndürmek için
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
            GameOver=true;
            CancelInvoke("Spawn");
            karakterAnim.SetBool("Death_b", true);
            karakterAnim.SetInteger("DeathType_int",2);
            kanama.Play();
            Panel.SetActive(true);
        }
    }
    
}
