using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playeract : MonoBehaviour
{
    public InputAction use;
    private Playermove body;
    private Uiscript inv;
    private Playerfight flesh;
    
    private ParticleSystem glow;
    private ParticleSystem[] auras = new ParticleSystem[4];
    
    private float[] timers = new float[3];
    private char[] timerass = {'X', 'X', 'X'};

    void Start()
    {
        use.performed += ctx => { Use(ctx); };
        use.Enable();
        inv = GameObject.Find("Canvas").GetComponent<Uiscript>();
        body = gameObject.GetComponent<Playermove>();
        glow = gameObject.GetComponent<ParticleSystem>();
        flesh = transform.GetChild(0).GetComponent<Playerfight>();
        var aura_part = GameObject.Find("auras");
        for (int i = auras.GetLength(0); i > 0; i--)
        {
            auras[i-1] = aura_part.transform.GetChild(i-1).GetComponent<ParticleSystem>();
        }
    }

    void Update()
    {
        int t = 0;
        while (timerass[t] != 'X' && t < 3)
        {
            if (timers[t] >= 0)
            {
                timers[t] -= Time.deltaTime;
            }
            if (timers[t] < 0)
            {
                Done(t);
                timerass[t] = 'X';
            }
            t++;
        }
    }

    void Use(InputAction.CallbackContext context)
    {
        int boon = inv.Loseitems();
        switch (boon)
        {
            case 0:
                Speed();
                auras[boon].Play();
                break;
            case 1:
                Jump();
                auras[boon].Play();
                break;
            case 2:
                Damage();
                auras[boon].Play();
                break;
            case 3:
                Heal('S');
                glow.Play();
                break;
            case 4:
                Heal('L');
                glow.Play();
                break;
            case 5:
                Sonic();
                auras[boon-2].Play();
                break;
        }
    }

    void Starttimer(char type, float time)
    {
        var begun = false;
        var t = 0;
        while (!begun)
        {
            if (timerass[t] == 'X')
            {
                timerass[t] = type;
                timers[t] = time;
                begun = true;
            }
            else
            {
                t++;
            }
        }
    }

    void Speed()
    {
        body.speed = body.speed * 2;
        Starttimer('S', 15f);
    }

    void Jump()
    {
        body.jstrength = body.jstrength * 2;
        Starttimer('J', 15f);
    }

    void Damage()
    {
        Debug.Log("missing prerequisite systems.");
    }

    void Heal(char size)
    {
        if (size == 'S')
        {
            flesh.health += 3;
        }
        else if (size == 'L')
        {
            flesh.health += 9;
        }
    }

    void Sonic()
    {
        body.speed = body.speed * 6;
        Starttimer('0', 1f);
    }

    void Done(int t)
    {
        timers[t] = 0;
        switch (timerass[t])
        {
            case 'S':
                body.speed = body.speed / 2;
                break;
            case 'J':
                body.jstrength = body.jstrength / 2;
                break;
            case 'O':
                body.speed = body.speed / 6;
                break;
        }
    }

}
