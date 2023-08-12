using UnityEngine;

public class AIMovement : MonoBehaviour
{
    private Animator animator;

    public float moveSpeed = 0.2f;

    private Vector3 stopPosition;

    private float walkTime;
    public float walkCounter;
    private float waitTime;
    public float waitCounter;

    private int WalkDirection;

    public bool isWalking;

    private void Start()
    {
        animator = GetComponent<Animator>();

        walkTime = Random.Range(3, 6);
        waitTime = Random.Range(5, 7);

        waitCounter = waitTime;
        walkCounter = walkTime;

        ChooseDirection();
    }

    private void Update()
    {
        if (isWalking)
        {
            animator.SetBool("isRunning", true);

            walkCounter -= Time.deltaTime;

            transform.localRotation = Quaternion.Euler(0f, WalkDirection == 2 ? -90 : WalkDirection * 90, 0f);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            // not floaty
            Ray
                ray = new Ray(transform.position + new Vector3(0f, 0.25f, 0f), Vector3.down),
                ray2 = new Ray(transform.position + new Vector3(0f, 0.25f, 0f) + transform.right * 0.2f, Vector3.down),
                ray3 = new Ray(transform.position + new Vector3(0f, 0.25f, 0f) + transform.right * -0.2f, Vector3.down);
            RaycastHit
                hit,
                hit2,
                hit3;
            if (!Physics.Raycast(ray, out hit, 0.3f) && !Physics.Raycast(ray2, out hit2, 0.3f) && !Physics.Raycast(ray3, out hit3, 0.3f))
            {
                stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                isWalking = false;
                transform.position = stopPosition;
                animator.SetBool("isRunning", false);
                waitCounter = waitTime;
            }

            if (walkCounter <= 0)
            {
                stopPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                isWalking = false;

                transform.position = stopPosition;
                animator.SetBool("isRunning", false);

                waitCounter = waitTime;
            }
        }
        else
        {
            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                ChooseDirection();
            }
        }
    }

    public void ChooseDirection()
    {
        WalkDirection = Random.Range(0, 4);

        isWalking = true;
        walkCounter = walkTime;
    }
}