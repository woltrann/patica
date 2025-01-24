using UnityEngine;

public class mermiHareketi : MonoBehaviour
{
    private Vector3 startPosition;
    private float speed = 100f;


    void Start()
    {
        startPosition = transform.position; // Objeyi oluþturduðun pozisyonu kaydet
    }
    
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        float distanceTravelled = Vector3.Distance(startPosition, transform.position);
        if (distanceTravelled >= 100f)
        {
            Destroy(gameObject); // 20 birimden fazla hareket ettiyse objeyi yok et
        }

    }
}
