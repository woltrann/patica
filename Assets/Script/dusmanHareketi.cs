using System.Net.Mime;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class dusmanHareketi : MonoBehaviour
{
   
    public ParticleSystem kanama2;
    private karakterKomut nesne;
    void Start()
    {
        nesne=GameObject.Find("Karakterimiz").GetComponent<karakterKomut>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime*20);
        if (transform.position.z < -55)
        {
            Destroy(gameObject);
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mermi"))
        {
            Destroy(gameObject);
            Instantiate(kanama2, transform.position, kanama2.transform.rotation);
            //nesne.TilkiOldurme();
        }
        else if (other.CompareTag("Et"))
        {
            Debug.Log("YANLIS");
            Destroy(other.gameObject); // Tetiklenen nesneyi yok et 
        }
    }
}
