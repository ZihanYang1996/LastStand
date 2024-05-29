using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField]
    private TextMeshProUGUI countdownText;

    [SerializeField]
    private GameObject buttons;

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

    // Flag to indicate if the game is paused
    public static bool GamePaused = false;


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
        // Hide buttons at the start
        buttons.SetActive(false);
        
        // Set up and start the game
        SetupGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.R].wasPressedThisFrame)
        {
            Restart();
        }

        if (Keyboard.current[Key.Escape].wasPressedThisFrame)
        {
            EscPressed();
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
        
        // Unpause the game
        PauseGame(false);

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

    private void IncreaseDifficulty()
    {
        // Increase the force of the ball kick
        ball.IncreaseKickForce();
    }

    private IEnumerator StartGame()
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

    /// <summary>
    /// Handles the pressing of the Escape key.
    /// </summary>
    private void EscPressed()
    {
        // If the game is not paused, pause it
        if (!GamePaused)
        {
            PauseGame(true);
        }
        else
        {
            // If the result has not been received, unpause the game, otherwise do nothing
            if (!_resultReceived)
            {
                PauseGame(false);
            }
        }
    }

    private void PauseGame(bool pause)
    {
        if (pause && !GamePaused)
        {
            GamePaused = true;
            Time.timeScale = 0;
            // Show buttons
            buttons.SetActive(true);
        }
        else if (!pause && GamePaused)
        {
            GamePaused = false;
            Time.timeScale = 1;
            // Hide buttons
            buttons.SetActive(false);
        }
        else
        {
            Debug.Log("Unexpected pause state.");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Continue()
    {
        // If called after the result has been received, increase the difficulty then start new round
        if (_resultReceived)
        {
            Debug.Log("Continue, increasing difficulty and starting new round.");
            IncreaseDifficulty();
            SetupGame();
        }
        else
        {
            // Otherwise, just unpause the game first no matter what
            PauseGame(false);
        }
    }


    public void ProcessResult(bool isGoal)
    {
        // Skip if the result has already been received
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
        
        // Pause the game
        PauseGame(true);
    }
}