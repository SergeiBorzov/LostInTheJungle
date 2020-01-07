using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private Material selectMaterial = null;
    [SerializeField]
    private Material originalMaterial = null;
    // Start is called before the first frame update

    public void SetDefaultMaterial()
    {
        GetComponent<Renderer>().sharedMaterial = originalMaterial;
    }

    public void StartDissolve(Vector3 newPosition)
    {
        IEnumerator coroutine = Dissolve(newPosition);
        StartCoroutine(coroutine);
    }

    private IEnumerator Dissolve(Vector3 newPosition)
    {
        float ft = 0.4f;


        Renderer renderer = GetComponent<Renderer>();
        while (ft > 0.0f)
        {
            //Debug.Log("Time " + Time.deltaTime);
            //Debug.Log("Ft " + ft);
            ft = ft - Time.deltaTime*0.5f;
            renderer.material.SetFloat("_AlphaThreshold", ft);
            yield return null;
        }

        //renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        transform.position = newPosition;

        bool shadowsBackOn = false;
        while (ft < 0.4f)
        {
            ft = ft + Time.deltaTime * 0.5f;
            renderer.material.SetFloat("_AlphaThreshold", ft);

            //if (!shadowsBackOn && ft > 0.3f)
            //{
            //    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            //}
            yield return null;
        }
        SetDefaultMaterial();

        yield return null;
    }

    public void ChangeMaterial()
    {
        if (GetComponent<Renderer>().sharedMaterial == selectMaterial)
        {
            GetComponent<Renderer>().sharedMaterial = originalMaterial;
        }
        else
        {
            GetComponent<Renderer>().sharedMaterial = selectMaterial; 
        }
    }

}
