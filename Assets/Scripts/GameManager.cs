using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField]
    private TextMeshProUGUI countdownText;

    [SerializeField]
    private BallScript ball;

    [SerializeField]
    private float kickingCountdown = 5f;

    private float _goalDetectionDelay = 2f;
    private bool _resultReceived = false;
    
    // Singleton instance
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.R].wasPressedThisFrame)
        {
            Restart();
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator StartGame()
    {
        while (kickingCountdown > 0)
        {
            countdownText.text = kickingCountdown.ToString("F0");
            yield return new WaitForSeconds(1f);
            kickingCountdown--;
        }

        countdownText.text = "Go!";
        ball.KickBall();

        yield return new WaitForSeconds(_goalDetectionDelay);
        GetResult(false);

    }

    public void GetResult(bool isGoal)
    {
        if (_resultReceived)
        {
            return;
        }
        
        _resultReceived = true;
        if (isGoal)
        {
            Debug.Log("Goal! You lose!");
        }
        else
        {
            Debug.Log("No goal! You win!");
        }
    }
}