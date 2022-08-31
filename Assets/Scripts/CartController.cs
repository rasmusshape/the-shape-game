using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{
    [SerializeField] private float currentSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accelerationSpeed = 0.2f;
    [SerializeField] private float turnSpeed = 200f;

    private int steerValue;

    private void Start()
    {

    }

    void Update()
    {
        // If speeder down, add speed pr update (acceler)
        currentSpeed += accelerationSpeed * Time.deltaTime;

        transform.Rotate(0f, 0f, steerValue * turnSpeed * Time.deltaTime);

        transform.Translate(Vector2.up * currentSpeed * Time.deltaTime);
    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle"))
        {
            //SceneManager.LoadScene(0);
        }
    }

    public void Accelerate() 
    {
        //transform.
        
    }

    public void Steer(int value)
    {
        steerValue = value;
    }

}
