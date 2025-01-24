using UnityEngine;

public class RotateKamera : MonoBehaviour
{
    public float speed = 25.1f;
    public GameObject targetObject;
    void Start()
    {
        
    }

    void Update()
    {
        

    }

    public void donus()
    {
        targetObject.transform.rotation = transform.rotation;
        float horizontaInput=Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontaInput * speed*Time.deltaTime);
    }
}
