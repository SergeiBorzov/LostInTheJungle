using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private ParticleSystem main;

    [SerializeField]
    private Collider fireCollider;


    private void Start()
    {
        fireCollider.enabled = false;
        main.Stop();
    }

    public void Action()
    {
        fireCollider.enabled = true;
        main.Play();
    }

    public void Stop()
    {
        fireCollider.enabled = false;
        main.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        var characterScript = other.GetComponent<Character>();

        if (characterScript != null)
        {
            characterScript.TakeDamage(1.0f);
            characterScript.OnFire();
        }
    }


}
