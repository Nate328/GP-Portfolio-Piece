using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movingplat : MonoBehaviour
{
    public float fb;
    public float speed;
    public Vector3 velocity;
    private Vector3 previous;

    public Vector3 start;
    public Vector3 end;
    public bool multipoint;
    public Vector3[] stopoffs;

    public int stopnum;

    private Vector3 from;
    private Vector3 to;

    void Start()  //translates the positions it is given into world space for later movement
    {
        start += transform.parent.position;
        end += transform.parent.position;
        from = start;
        if (!multipoint) //sets destination for two point moving platforms
        {
            to = end;
        }
        else  //translates the rest of the positions other than the start and end
        {
            for (int i = stopoffs.GetLength(0)-1; i >= 0; i--)
            {
                stopoffs[i] += transform.parent.position;
            }
            to = stopoffs[0];  //sets first destination out of however many are given
            stopnum = 0;
        }
    }

    
    void FixedUpdate()  //moves the platform along its course- with different functions for two stop and multi stop platforms
    {
        if (!multipoint)
        {
            Twopoints();
        }
        else
        {
            Morepoints();
        }
        velocity = (transform.position - previous) / Time.deltaTime; //calculates velocity for the Sharementum script
        previous = transform.position;
    }

    void Twopoints()  //moving platform between two points
    {
        if (fb > 0) //if going one way
        {
            if (fb < 1)  //if not yet at its destination
            { transform.position = Vector3.Lerp(from, to, fb); }
            fb += speed;
        }
        else if (fb < 0)  //if going the other way
        {
            if (fb > -1)
            { transform.position = Vector3.Lerp(to, from, Mathf.Abs(fb)); }
            fb -= speed;
        }

        if (Mathf.Abs(fb) > 1.3) //Once the platform has reached its destination and waited for a moment, it swaps direction
        {
            if (fb > 0)
            { fb = -0.03f; }
            else if (fb < 0)
            { fb = 0.03f; }
        }
    }

    
    
    void Morepoints()  //moving platform between more than two points
    {
        if (fb > 0)  //similar to above, but always moves from -> to, as it reverses the list to reverse
        {
            if (fb < stopoffs.GetLength(0))
            { transform.position = Vector3.Lerp(from, to, fb); }
            fb += speed;
        }
        else if (fb < 0)
        {
            if (fb > -stopoffs.GetLength(0))
            { transform.position = Vector3.Lerp(from, to, Mathf.Abs(fb)); }
            fb -= speed;
        }

        if (Mathf.Abs(fb) > 1)  //when it reaches the next point
        {
            from = to; //set old destination as current location
            if (fb > 0)  //if moving forward through the list
            {
                stopnum += 1;
                fb = 0.03f;
                if (stopnum >= stopoffs.GetLength(0))  //if at the end of the list
                {
                    to = end;  //go to the end
                    stopnum = stopoffs.GetLength(0) - 1;  //set the last stop as the next stop so it can reverse
                    fb = -0.03f;
                }
                else
                { to = stopoffs[stopnum]; }  //else, just move onto the next stop on the list
            }
            else if (fb < 0)  //if moving back through the list
            {
                stopnum -= 1;
                fb = 0.03f;
                if (stopnum < 0)
                {
                    to = start;  //same as above, but with the first stop rather than the last
                    stopnum = 0;
                    fb = 0.03f;
                }
                else
                { to = stopoffs[stopnum]; }
            }
        }
    }
}
