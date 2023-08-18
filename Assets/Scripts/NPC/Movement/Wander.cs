using System.Collections;
using UnityEngine;

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

    private CharacterController controller;
    private float heading;
    private Vector3 targetRotation;

    private GameObject player;

    private bool toPlayer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        waitTime = (waitTime > 0 ? waitTime : Random.Range(3f, 6f)) + Random.Range(0f, 3f);
        waitCounter = waitTime;

        walkTime = (walkTime > 0 ? walkTime : Random.Range(1f, 4f)) + Random.Range(0f, 2f);
        walkCounter = walkTime;

        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(NewHeading());
    }

    private void Update()
    {
        if (isWalking)
        {
            animator.SetBool("isRunning", true);
            if (!toPlayer)
                transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
            else
            { 
                transform.LookAt(player.transform);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }

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

    private IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void NewHeadingRoutine()
    {
        if (!(Vector3.Distance(transform.position, player.transform.position) > 30f))
        {
            var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
            var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
            heading = Random.Range(floor, ceil);
            targetRotation = new Vector3(0, heading, 0);
            toPlayer = false;
        }
        else
            toPlayer = true;
    }
}