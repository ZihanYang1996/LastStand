using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    

    [Header("Game Settings")]
    [SerializeField]
    private TextMeshProUGUI countdownText;
    [SerializeField]
    private BallScript ball;
    [SerializeField]
    private float countdown = 5f;
    
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
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString("F0");
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = "Go!";
        ball.KickBall();
    }
}