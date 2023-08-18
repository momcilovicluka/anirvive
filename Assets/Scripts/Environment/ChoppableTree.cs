using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : MonoBehaviour
{
    public bool playerInRange;
    public bool canBeChopped;

    public float treeMaxHealth;
    public float treeHealth;

    public Animator animator;

    public float caloriesSpentChoppingWood = 20;

    public void Start()
    {
        treeHealth = treeMaxHealth;
        animator = transform.parent.transform.parent.GetComponent<Animator>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            canBeChopped = true;
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            canBeChopped = false;
        }
    }

    public void GetHit()
    {
        animator.SetTrigger("shake");

        treeHealth--;

        PlayerState.Instance.currentCalories -= caloriesSpentChoppingWood;

        if (treeHealth <= 0)
            TreeIsDead();

    }

    public void TreeIsDead()
    {
        Vector3 treePosition = transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            Vector3 stickPosition = new Vector3(treePosition.x + Random.Range(-0.5f, 0.5f), treePosition.y, treePosition.z + Random.Range(-0.5f, 0.5f));
            GameObject brokenTree = Instantiate(Resources.Load<GameObject>("Stick"), stickPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
    }

    private void Update()
    {
        if(canBeChopped)
        {
            GlobalState.Instance.resourceHealth = treeHealth;
            GlobalState.Instance.resourceMaxHealth = treeMaxHealth;
        }
    }
    
}
