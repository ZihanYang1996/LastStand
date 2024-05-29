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
    private bool _increaseDifficulty = false;
    private Coroutine _gameCoroutine;
    private TextMeshProUGUI _continueButtonText;

    // Singleton instance
    public static GameManager Instance { get; private set; }

    // Flag to indicate if the game is paused
    public static bool GamePaused = false;

    enum ContinueButtonState
    {
        Continue,
        Retry,
        IncreaseDifficulty
    }

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
        
        // Find the continue button text
        if (_continueButtonText = buttons.transform.Find("ContinueButton").GetComponentInChildren<TextMeshProUGUI>())
        {
            Debug.Log("Continue button text found.");
        }
        else
        {
            Debug.LogError("Continue button text not found.");
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
        Debug.Log("Starting the game.");
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
        // Reset the flag that indicates if the difficulty should be increased
        _increaseDifficulty = false;
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

    private void ConfigureContinueButton(ContinueButtonState state)
    {
        switch (state)
        {
            case ContinueButtonState.Continue:
                _continueButtonText.text = "Continue";
                break;
            case ContinueButtonState.Retry:
                _continueButtonText.text = "Retry";
                break;
            case ContinueButtonState.IncreaseDifficulty:
                _continueButtonText.text = "Increase Difficulty";
                break;
        }
    }

    /// <summary>
    /// Handles the pressing of the Escape key.
    /// </summary>
    private void EscPressed()
    {
        // If the game is not paused, pause it
        if (!GamePaused)
        {
            ConfigureContinueButton(ContinueButtonState.Continue);
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
            // If player succeeded, increase the difficulty
            if (_increaseDifficulty)
            {
                IncreaseDifficulty();
            }
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
            // Do not increase the difficulty if the player failed
            _increaseDifficulty = false;
            ConfigureContinueButton(ContinueButtonState.Retry);
        }
        else
        {
            countdownText.text = "Ooooooooooh!";
            // If the player succeeded, increase the difficulty
            _increaseDifficulty = true;
            ConfigureContinueButton(ContinueButtonState.IncreaseDifficulty);
        }

        // Pause the game
        PauseGame(true);
    }
}