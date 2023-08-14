using UnityEngine;
using System.Collections;

/// <summary>
/// Creates wandering behaviour for a CharacterController.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Wander : MonoBehaviour
{
    public float speed = 5;
    public float directionChangeInterval = 1;
    public float maxHeadingChange = 30;

    public float walkTime;
    public float walkCounter;
    public float waitTime;
    public float waitCounter;
    private bool isWalking;

    private Animator animator;

    CharacterController controller;
    float heading;
    Vector3 targetRotation;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        waitTime = (waitTime > 0 ? waitTime : Random.Range(3f, 6f)) + Random.Range(0f, 3f);
        waitCounter = waitTime;

        walkTime = (walkTime > 0 ? walkTime : Random.Range(1f, 4f)) + Random.Range(0f, 2f);
        walkCounter = walkTime;

        StartCoroutine(NewHeading());
    }

    void Update()
    {
        if(isWalking)
        {
            animator.SetBool("isRunning", true);
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
            var forward = transform.TransformDirection(Vector3.forward);
            controller.SimpleMove(forward * speed);
            
            walkCounter -= Time.deltaTime;
            
            if (walkCounter < 0)
            {
                isWalking = false;
                animator.SetBool("isRunning", false);
                waitCounter = waitTime;
            }

        }
        else
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter < 0)
            {
                isWalking = true;
                walkCounter = walkTime;
            }
        }
    }

    /// <summary>
    /// Repeatedly calculates a new direction to move towards.
    /// Use this instead of MonoBehaviour.InvokeRepeating so that the interval can be changed at runtime.
    /// </summary>
    IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    /// <summary>
    /// Calculates a new direction to move towards.
    /// </summary>
    void NewHeadingRoutine()
    {
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = Random.Range(floor, ceil);
        targetRotation = new Vector3(0, heading, 0);

    }
}