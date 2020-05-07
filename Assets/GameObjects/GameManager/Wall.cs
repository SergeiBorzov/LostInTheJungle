using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private Renderer rndr;

    private float ft = 1.0f;
    private void Start()
    {
        rndr = GetComponent<Renderer>();
    }

    IEnumerator Fade()
    {
        while (ft > 0)
        {
            ft -= Time.deltaTime;
            if (ft < 0)
            {
                rndr.material.SetFloat("_Alpha", 0);
            }
            rndr.material.SetFloat("_Alpha", ft);
            yield return null;
        }
    }

    IEnumerator Opaque()
    {
        while (ft < 1.0)
        {
            ft += Time.deltaTime;
            if (ft > 1)
            {
                rndr.material.SetFloat("_Alpha", 1);
            }
            rndr.material.SetFloat("_Alpha", ft);
            yield return null;
        }
    }

    public void Action()
    {
        StartCoroutine("Fade");
    }

    public void Deaction()
    {
        StartCoroutine("Opaque");
    }


}
