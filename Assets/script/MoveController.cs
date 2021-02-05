using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{

    private Rigidbody2D rb;
    public float speed = 5;

    private float startPosX;
    private float startPosY;
    private float pixeW;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosX = rb.position.x;
        startPosY = rb.position.y;

        pixeW = GetComponent<BoxCollider2D>().size.x;
    }

    // Update is called once per frame
    void Update()
    {
        MoveMent();
    }

    void MoveMent()
    {
     
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.velocity = new Vector2(rb.velocity.x, speed);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow)||
            Input.GetKeyUp(KeyCode.RightArrow) ||
            Input.GetKeyUp(KeyCode.UpArrow) ||
            Input.GetKeyUp(KeyCode.DownArrow))
        {
            rb.velocity = new Vector2(0, 0);
            Debug.Log("pos x=" +rb.position.x+" pos y="+rb.position.y+ "startPosX="+ startPosX + "pixeW="+ pixeW);
            int x = Convert.ToInt32((rb.position.x - startPosX)/ pixeW);
            int y = Convert.ToInt32((rb.position.y - startPosY)/ pixeW);
            Debug.Log("x=" + x+ "y=" + y + "  end:x=" + startPosX * x + " endY=" + startPosY * y);

            float endPosx = startPosX + x;
            float endPosy = startPosY + y;
            startPosX = endPosx;
            startPosY = endPosy;
            rb.position = new Vector2(endPosx, endPosy);
         
        }
    }
}
