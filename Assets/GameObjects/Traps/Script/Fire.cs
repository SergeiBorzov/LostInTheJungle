using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private ParticleSystem main;
    [SerializeField]
    private ParticleSystem glow;

    [SerializeField]
    private Collider fireCollider;


    private void Start()
    {
        fireCollider.enabled = false;
        main.Stop();
        glow.Stop();
    }

    public void Action()
    {
        fireCollider.enabled = true;
        main.Play();
        glow.Play();
    }

    public void Stop()
    {
        fireCollider.enabled = false;
        main.Stop();
        glow.Stop();
    }

    private void OnTriggerStay(Collider other)
    {
        var characterScript = other.GetComponent<Character>();

        if (characterScript != null)
        {
            Debug.Log("Pain in Fire!");
        }
    }


}
