using UnityEngine;

public class Hayvan : MonoBehaviour
{
    public int hiz=10;
    private Rigidbody hayvanRB;
    private GameObject karakter;
    void Start()
    {
        hayvanRB = GetComponent<Rigidbody>();
        karakter = GameObject.Find("Karakterimiz");
    }

    void Update()
    {
        hayvanRB.AddForce((karakter.transform.position-transform.position).normalized*hiz);
    }
}
