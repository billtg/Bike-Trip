using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public bool useParallax = false;
    public float speed;
    public float minScreen = -20;

    public Material material;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        CheckForOffScreen();
    }

    private void Move()
    {
        transform.position = new Vector3(this.transform.position.x - speed*ObjectController.instance.simSpeed, this.transform.position.y, this.transform.position.z);
    }

    private void CheckForOffScreen()
    {
        if (transform.position.x< minScreen)
        {
            Destroy(this.gameObject);
        }
    }
}
