using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float speed = 1.0f;

    [SerializeField]
    float lifeTime = 2.0f;

    bool alive = true;
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            alive = false; 
            DestroyImmediate(gameObject);
        }
        if (alive)
        {
            transform.position = transform.position + new Vector3(0.0f, 0.0f, -1.0f) * speed * Time.deltaTime;
        }
    }
}
