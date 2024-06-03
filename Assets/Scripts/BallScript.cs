using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BallScript : MonoBehaviour
{
    public float kickForce = 10f;

    private Rigidbody _ball;

    [Header("Target Positions")]
    // Height needs to be adjusted!!!!
    [SerializeField]
    private Transform leftTarget;

    [SerializeField]
    private Transform middleTarget;

    [SerializeField]
    private Transform rightTarget;

    private Vector3 _initialPosition;

    enum Direction
    {
        Left,
        Middle,
        Right
    }

    private void Awake()
    {
        _ball = gameObject.GetComponent<Rigidbody>();
        _initialPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.K].wasPressedThisFrame)
        {
            KickBall();
        }
    }

    private Direction RandomDirection()
    {
        return (Direction)Random.Range(0, 3);
    }

    public void KickBall()
    {
        Vector3 direction;
        switch (RandomDirection())
        {
            case Direction.Left:
                direction = (leftTarget.position
                             + new Vector3(0.0f, 7.0f, 0.0f)
                             + new Vector3(-GameManager.RandomPosition.x, GameManager.RandomPosition.y, GameManager.RandomPosition.z)
                             - transform.position).normalized;
                break;
            case Direction.Middle:
                direction = (middleTarget.position
                             + new Vector3(0.0f, 11f, 0.0f)
                             - transform.position).normalized;
                break;
            case Direction.Right:
                direction = (rightTarget.position
                             + new Vector3(0.0f, 7.0f, 0.0f)
                             + new Vector3(GameManager.RandomPosition.x, GameManager.RandomPosition.y, GameManager.RandomPosition.z)
                             - transform.position).normalized;
                break;
            default:
                direction = (middleTarget.position
                             + new Vector3(0.0f, 7.0f, 0.0f)
                             - transform.position).normalized;
                break;
        }

        _ball.AddForce(direction * kickForce, ForceMode.Impulse);
    }

    public void ResetBall()
    {
        // Reset the ball position and rotation
        transform.position = _initialPosition;

        // Reset the ball velocity and angular velocity
        _ball.velocity = Vector3.zero;
        _ball.angularVelocity = Vector3.zero;
    }

    public void IncreaseKickForce()
    {
        kickForce += 1f;
        Debug.Log("Kick force increased to " + kickForce);
    }
}