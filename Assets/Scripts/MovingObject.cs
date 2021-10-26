using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public bool useParallax = false;
    public float speed;
    public float minScreen = -20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckForOffScreen();
    }

    private void Move()
    {
        if (useParallax)
        {
            //Debug.Log("Use Parallax");
            transform.position = new Vector3(this.transform.position.x - speed*ObjectController.instance.simSpeed, this.transform.position.y, this.transform.position.z);
        } else
        {
            transform.position = new Vector3(this.transform.position.x - speed * ObjectController.instance.simSpeed, this.transform.position.y, this.transform.position.z);
        }
    }

    private void CheckForOffScreen()
    {
        if (transform.position.x< minScreen)
        {
            Destroy(this.gameObject);
        }
    }
}
