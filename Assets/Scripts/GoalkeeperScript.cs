using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GoalkeeperScript : MonoBehaviour
{
    [Header("Target Positions")] [SerializeField]
    private Transform leftTarget;

    [SerializeField] private Transform middleTarget;
    [SerializeField] private Transform rightTarget;

    [Header("Tuning Parameters")] [SerializeField]
    private float jumpSpeed = 5f;
    private float jumpTime = 1f;

    private bool isJumping = false;
    enum Direction
    {
        Left,
        Middle,
        Right
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[Key.LeftArrow].wasPressedThisFrame)
        {
            Jump(Direction.Left);
        }
        if (Keyboard.current[Key.DownArrow].wasPressedThisFrame)
        {
            Jump(Direction.Middle);
        }
        if (Keyboard.current[Key.RightArrow].wasPressedThisFrame)
        {
            Jump(Direction.Right);
        }
    }

    // private IEnumerator JumpCoroutine(Vector3 targetPosition)
    // {
    //     // Try Animation Curve in the future !!!!!!!!
    //     // try use time instead of distance, so it's easier to control
    //     Vector3 currentPosition = transform.position;
    //     while (Vector3.Distance(currentPosition, targetPosition) > Mathf.Epsilon)
    //     {
    //         // Lerp (Linear Interpolation
    //         Vector3 newPosition = Vector3.Lerp(currentPosition, targetPosition, jumpSpeed * Time.deltaTime);
    //         transform.position = newPosition;
    //         currentPosition = newPosition;
    //         yield return new WaitForEndOfFrame();
    //     }
    // }
    
    private IEnumerator JumpCoroutine(Vector3 targetPosition)
    {
        // Try Animation Curve in the future !!!!!!!!
        Vector3 velocity = Vector3.zero;
        float elapsedTime = 0f;
        
        while (elapsedTime < jumpTime)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / jumpTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        isJumping = false;
        yield return null;
    }

    void Jump(Direction direction)
    {
        if (isJumping) return;
        
        isJumping = true;
        switch (direction)
        {
            case Direction.Left:
                StartCoroutine(JumpCoroutine(leftTarget.position));
                break;
            case Direction.Middle:
                StartCoroutine(JumpCoroutine(middleTarget.position));
                break;
            case Direction.Right:
                StartCoroutine(JumpCoroutine(rightTarget.position));
                break;
        }
    }
}