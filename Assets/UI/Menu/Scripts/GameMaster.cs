using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    public Vector3 levelStart = new Vector3(46.22f, 29.79f, 0);
    public Vector3 lastCheckPoint = new Vector3(46.22f, 29.79f, 0);
    void Awake()
    {
        if (instance != null && instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
