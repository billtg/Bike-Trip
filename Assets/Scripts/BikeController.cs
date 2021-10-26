using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeController : MonoBehaviour
{
    public static BikeController instance;

    public float bikespeed;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //start biking

    }

    // Update is called once per frame
    void Update()
    {
        //coast
        //drink water
        //enjoy scenery
        //pedal

        //turn on light
        //turn off light
    }
}
