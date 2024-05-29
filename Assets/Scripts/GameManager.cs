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
    private GoalkeeperScript goalkeeper;

    [SerializeField]
    private float kickingCountdown = 5f;

    private float _currentCountdown;
    private float _goalDetectionDelay = 2f;
    private bool _resultReceived = false;
    private Coroutine _gameCoroutine;
    
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
        SetupGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.R].wasPressedThisFrame)
        {
            Restart();
        }
    }
    
    // Prepare the game for a new round
    private void SetupGame()
    {
        // Stop coroutine
        if (_gameCoroutine != null)
        {
            StopCoroutine(_gameCoroutine);
            _gameCoroutine = null;
        }
        
        // Rest the flag that indicates if the result has been received
        _resultReceived = false;
        // Reset the countdown
        _currentCountdown = kickingCountdown;
        // Reset the ball position
        ball.ResetBall();
        // Reset the goalkeeper position
        goalkeeper.ResetGoalkeeper();
        
        // Start the game
        _gameCoroutine = StartCoroutine(StartGame());
    }

    private void Restart()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetupGame();
    }

    public IEnumerator StartGame()
    {
        // Enable the countdown text
        countdownText.gameObject.SetActive(true);
        
        while (_currentCountdown > 0)
        {
            countdownText.text = _currentCountdown.ToString("F0");
            yield return new WaitForSeconds(1f);
            _currentCountdown--;
        }
        countdownText.text = "Go!";
        
        // Wait for 0.5 second before hiding the countdown text and kicking the ball
        yield return new WaitForSeconds(0.5f);
        countdownText.gameObject.SetActive(false);
        ball.KickBall();

        yield return new WaitForSeconds(_goalDetectionDelay);
        ProcessResult(false);

        // Set the game coroutine to null when finished
        _gameCoroutine = null;
    }

    public void ProcessResult(bool isGoal)
    {
        if (_resultReceived)
        {
            return;
        }
        
        _resultReceived = true;
        // Re-enable the countdown text
        countdownText.gameObject.SetActive(true);
        if (isGoal)
        {
            countdownText.text = "Noooooooooo!";
        }
        else
        {
            countdownText.text = "Ooooooooooh!";
        }
    }
}