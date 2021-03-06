﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowColumn : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private Transform lowSpawn;

    [SerializeField]
    private Transform middleSpawn;

    [SerializeField]
    private Transform highSpawn;

    [SerializeField]
    private float waitTime = 1.5f;
    private float timeToShoot;

    [SerializeField]
    private float shootTime = 3.0f;
    private float timeToStop;

    private bool shooting;

    [SerializeField]
    private Fire low;
    [SerializeField]
    private Fire middle;
    [SerializeField]
    private Fire high;
    //suond
    AudioSource flame_sound;
    //sound
    private void Fire()
    {
        low.Action();
        middle.Action();
        high.Action();
        flame_sound.Play();
    }

    private void StopFire()
    {
        low.Stop();
        middle.Stop();
        high.Stop();
        flame_sound.Stop();
    }

    /*private void Shoot()
    {
        var lower = Instantiate(arrow, lowSpawn);
        var middle = Instantiate(arrow, middleSpawn);
        var high = Instantiate(arrow, highSpawn);
    }*/

    private void Start()
    {
        flame_sound = GetComponent<AudioSource>();
        low.Stop();
        middle.Stop();
        high.Stop();

        shooting = false;
        timeToShoot = 1.5f;
    }


    void Update()
    {

        if (!shooting)
        {
            timeToShoot -= Time.deltaTime;
            if (timeToShoot < 0)
            {
                shooting = true;
                timeToStop = shootTime;
                Fire();
            }
        }
        else
        {
            timeToStop -= Time.deltaTime;
            if (timeToStop < 0)
            {
                shooting = false;
                timeToShoot = waitTime;
                StopFire();
            }
        }
        /*if (!shooting)
        {
            timeToShoot -= Time.deltaTime;
            if (timeToShoot < 0)
            {
                shooting = true;
                timeToStop = shootTime;
                InvokeRepeating("Shoot", 0.0f, 0.3f);
            }
        }
        else
        {
            timeToStop -= Time.deltaTime;
            if (timeToStop < 0)
            {
                shooting = false;
                timeToShoot = waitTime;
                CancelInvoke("Shoot");
            }
        }*/
    }
}
