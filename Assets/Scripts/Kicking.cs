using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Kicking : MonoBehaviour
{
    public float kickForce = 10f;

    private Rigidbody _ball;

    enum Direction
    {
        Left,
        Middle,
        Right
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        _ball = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.K].wasPressedThisFrame)
        {
            KickBall();
        }
        if (Keyboard.current[Key.R].wasPressedThisFrame)
        {
            Restart();
        }
    }
    
    public void KickBall()
    {
        Vector3 direction;
        switch (Random.Range(0, 3))
        {
            case (int)Direction.Left:
                direction = Vector3.forward + Vector3.left;
                break;
            case (int)Direction.Middle:
                direction = Vector3.forward;
                break;
            case (int)Direction.Right:
                direction = Vector3.forward + Vector3.right;
                break;
            default:
                direction = Vector3.forward;
                break;
        }
        _ball.AddForce(-direction * kickForce, ForceMode.Impulse);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
