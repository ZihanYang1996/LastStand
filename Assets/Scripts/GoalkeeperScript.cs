using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GoalkeeperScript : MonoBehaviour
{
    [Header("Target Positions")]
    [Tooltip("The position the goalkeeper dives to when the player presses the left arrow key")]
    [SerializeField]
    private Transform leftTarget;

    [Tooltip("The position the goalkeeper dives to when the player presses the down arrow key")] [SerializeField]
    private Transform middleTarget;

    [Tooltip("The position the goalkeeper dives to when the player presses the right arrow key")] [SerializeField]
    private Transform rightTarget;

    [Header("Tuning Parameters")]
    [Tooltip("The duration of the dive in seconds")]
    [SerializeField]
    private float diveDuration = 1f;

    [Tooltip("The curve that defines the dive behavior over time")]
    [SerializeField] private AnimationCurve diveCurve;

    private bool _isDiving = false;

    /// <summary>
    /// Enum representing the possible directions for the goalkeeper to dive.
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Represents the left direction.
        /// </summary>
        Left,

        /// <summary>
        /// Represents the middle direction.
        /// </summary>
        Middle,

        /// <summary>
        /// Represents the right direction.
        /// </summary>
        Right
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        TakeInput();
    }


    /// <summary>
    /// Handles the input from the user.
    /// </summary>
    private void TakeInput()
    {
        // Checks if the left arrow key was pressed during this frame
        // If so, it initiates a dive to the left
        if (Keyboard.current[Key.LeftArrow].wasPressedThisFrame)
        {
            Dive(Direction.Left);
        }

        // Checks if the down arrow key was pressed during this frame
        // If so, it initiates a dive to the middle
        if (Keyboard.current[Key.DownArrow].wasPressedThisFrame)
        {
            Dive(Direction.Middle);
        }

        // Checks if the right arrow key was pressed during this frame
        // If so, it initiates a dive to the right
        if (Keyboard.current[Key.RightArrow].wasPressedThisFrame)
        {
            Dive(Direction.Right);
        }
    }

    /// <summary>
    /// Coroutine that animates the dive of the goalkeeper towards the target position.
    /// </summary>
    /// <param name="targetPosition">The target position to which the goalkeeper should dive.</param>
    /// <returns>An IEnumerator that can be used to control the coroutine.</returns>
    private IEnumerator DiveCoroutine(Vector3 targetPosition)
    {
        // Set the diving state to true
        _isDiving = true;

        // Store the starting position and initialize the elapsed time
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        // Continue the dive as long as the elapsed time is less than the dive duration
        while (elapsedTime < diveDuration)
        {
            // Calculate the normalized time and evaluate the dive curve at this time
            float t = elapsedTime / diveDuration;
            float curveValue = diveCurve.Evaluate(t);

            // Calculate the new position as a lerp between the start and target positions, using the curve value
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, curveValue);
            // Update the position of the goalkeeper
            transform.position = newPosition;

            // Increment the elapsed time by the time since the last frame
            elapsedTime += Time.deltaTime;
            // Yield control back to the Unity engine until the next frame
            yield return null;
        }

        // At the end of the dive, set the position of the goalkeeper to the target position
        transform.position = targetPosition;
        // Set the diving state to false
        _isDiving = false;
    }

    /// <summary>
    /// Initiates the dive of the goalkeeper in the specified direction.
    /// </summary>
    /// <param name="direction">The direction in which the goalkeeper should dive.</param>
    void Dive(Direction direction)
    {
        // If the goalkeeper is already diving, we return to prevent another dive from starting
        if (_isDiving) return;

        // Depending on the direction, we start a coroutine to animate the dive towards the target position
        switch (direction)
        {
            // If the direction is left, we start a coroutine to animate the dive towards the left target position
            case Direction.Left:
                // Adjust the target position slightly to the left for a more natural dive
                Vector3 targetPosition = leftTarget.position + new Vector3(-3.0f, 0.0f, 0.0f);
                StartCoroutine(DiveCoroutine(targetPosition));
                break;
            // If the direction is middle, we start a coroutine to animate the dive towards the middle target position
            case Direction.Middle:
                // Adjust the target position slightly above the middle for a more dynamic dive
                Vector3 middleTargetPosition = middleTarget.position + new Vector3(0.0f, 0.0f, 3.0f);
                StartCoroutine(DiveCoroutine(middleTargetPosition));
                break;
            // If the direction is right, we start a coroutine to animate the dive towards the right target position
            case Direction.Right:
                // Adjust the target position slightly to the right for a more natural dive
                Vector3 rightTargetPosition = rightTarget.position + new Vector3(3.0f, 0.0f, 0.0f);
                StartCoroutine(DiveCoroutine(rightTargetPosition));
                break;
        }
    }
}