using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private Material dissolveMaterial;
    private Material material;
    private Renderer myRenderer;
    public bool selected = false;

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
        material = new Material(myRenderer.material);
        myRenderer.material = material;
    }

    public void SetDefaultOutline()
    {
        myRenderer.material.SetFloat("_OutlineWidth", 0.0f);
        selected = false;
    }

    public void StartDissolve(Vector3 newPosition)
    {
        IEnumerator coroutine = Dissolve(newPosition);
        StartCoroutine(coroutine);
    }

    private IEnumerator Dissolve(Vector3 newPosition)
    {
        SetDissolveMaterial();
        float ft = 0.4f;

        SetDefaultOutline();
        Renderer renderer = GetComponent<Renderer>();
        while (ft > 0.0f)
        {
            //Debug.Log("Time " + Time.deltaTime);
            //Debug.Log("Ft " + ft);
            ft -= Time.deltaTime*0.5f;
            renderer.material.SetFloat("_AlphaThreshold", ft);
            yield return null;
        }

        //renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        transform.position = newPosition;

        while (ft < 0.4f)
        {
            ft += Time.deltaTime * 0.5f;
            renderer.material.SetFloat("_AlphaThreshold", ft);

            //if (!shadowsBackOn && ft > 0.3f)
            //{
            //    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            //}
            yield return null;
        }

        renderer.material = material;
        yield return null;
    }

    private void SetDissolveMaterial()
    {
        myRenderer.material = dissolveMaterial;
    }
    public void ChangeOutline()
    {
        if (selected)
        {
            myRenderer.material.SetFloat("_OutlineWidth", 0.0f);
            selected = false;
        }
        else
        {
            myRenderer.material.SetFloat("_OutlineWidth", 0.07f);
            selected = true;
        }
        
    }

}
