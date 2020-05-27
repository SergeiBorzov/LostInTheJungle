using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenTree : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private BoxCollider actionCollider;

    [SerializeField]
    private BoxCollider bridgeCollider;

    [SerializeField]
    private BoxCollider safeCollider;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    void Start()
    {
        
        actionCollider.enabled = true;
        bridgeCollider.enabled = false;
        safeCollider.enabled = true;
    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        var swordScript = other.gameObject.GetComponent<Sword>();

        if (swordScript != null)
        {
            
            actionCollider.enabled = false;
            safeCollider.enabled = false;
            bridgeCollider.enabled = true;
            animator.enabled = true;
            StartCoroutine(soundCorutine());
        }
    }

     IEnumerator soundCorutine()
    {
        this.GetComponents<AudioSource>()[1].Play();

        yield return new WaitForSeconds(0.85f);

        this.GetComponents<AudioSource>()[0].Play();
    }
}
