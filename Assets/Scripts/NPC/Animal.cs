using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;

    [Header("Sounds")]
    [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip hitAndScreamSound;
    [SerializeField] AudioClip dieSound;

    private Animator animator;
    public bool isDead;

    [SerializeField] ParticleSystem bloodSplashParticles;
    [SerializeField] GameObject bloodPuddle;

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        bloodPuddle.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            currentHealth -= damage;

            bloodSplashParticles.Play();

            if (currentHealth <= 0)
            {
                PlayDyingSound();

                animator.SetTrigger("Die");

                if(GetComponent<Wander>() != null)
                    GetComponent<Wander>().enabled = false;

                StartCoroutine(PuddleDelay());

                isDead = true;
            }
            else
                PlayHitSound(); 
        }
    }

    IEnumerator PuddleDelay()
    {
        yield return new WaitForSeconds(1f);
        if(bloodPuddle != null)
            bloodPuddle.SetActive(true);
    }

    private void PlayDyingSound()
    {
        soundChannel.PlayOneShot(dieSound);
    }

    private void PlayHitSound()
    {
        soundChannel.PlayOneShot(hitAndScreamSound);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
