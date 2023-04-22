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

    void Start()
    {
        start += transform.parent.position;
        end += transform.parent.position;
        from = start;
        if (!multipoint)
        {
            to = end;
        }
        else
        {
            for (int i = stopoffs.GetLength(0)-1; i >= 0; i--)
            {
                stopoffs[i] += transform.parent.position;
            }
            to = stopoffs[0];
            stopnum = 0;
        }
    }

    
    void FixedUpdate()
    {
        if (!multipoint)
        {
            Twopoints();
        }
        else
        {
            Morepoints();
        }
        velocity = (transform.position - previous) / Time.deltaTime;
        previous = transform.position;
    }

    void Twopoints()
    {
        if (fb > 0)
        {
            if (fb < 1)
            { transform.position = Vector3.Lerp(from, to, fb); }
            fb += speed;
        }
        else if (fb < 0)
        {
            if (fb > -1)
            { transform.position = Vector3.Lerp(to, from, Mathf.Abs(fb)); }
            fb -= speed;
        }

        if (Mathf.Abs(fb) > 1.3)
        {
            if (fb > 0)
            { fb = -0.03f; }
            else if (fb < 0)
            { fb = 0.03f; }
        }
    }

    
    
    void Morepoints()
    {
        if (fb > 0)
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

        if (Mathf.Abs(fb) > 1)
        {
            from = to;
            if (fb > 0)
            {
                stopnum += 1;
                fb = 0.03f;
                if (stopnum >= stopoffs.GetLength(0))
                {
                    to = end;
                    stopnum = stopoffs.GetLength(0) - 1;
                    fb = -0.03f;
                }
                else
                { to = stopoffs[stopnum]; }
            }
            else if (fb < 0)
            {
                stopnum -= 1;
                fb = 0.03f;
                if (stopnum < 0)
                {
                    to = start;
                    stopnum = 0;
                    fb = 0.03f;
                }
                else
                { to = stopoffs[stopnum]; }
            }
        }
    }
}
