using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalNetScript : MonoBehaviour
{
    [SerializeField]
    private float backForce = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        // Get the ball's rigidbody
        Rigidbody ball = other.gameObject.GetComponent<Rigidbody>();
        
        // Store the ball's direction
        Vector3 direction = ball.velocity.normalized;
        
        // Stop the ball
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
        
        // Add a little force backwards
        ball.AddForce(-direction * backForce, ForceMode.Impulse);
        
        Debug.Log("Goal!");

    }
}
