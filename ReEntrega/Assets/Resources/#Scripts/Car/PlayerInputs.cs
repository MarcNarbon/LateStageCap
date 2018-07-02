using System .Collections;
using System .Collections .Generic;
using UnityEngine;

public class PlayerInputs : ICarInputs
{

    //Control del DOUBLE TAP para el DASH
    private float lastTapTimeLeft;//Tiempo desde el ultimo TAP hacia la izquierda
    private float lastTapTimeRight;//Tiempo desde el ultimo TAP hacia la derecha
    private float tapSpeed = 0.2f;//Tiempo maximo entre dos pulsaciones para ser considerado DOUBLE TAP
    private bool doubleTappedLeft = false;//Mirar si se ha tapeado hacia la izquierda 
    private bool doubleTappedRight = false;//Mirar si se ha tapeado hacia la derecha 


    public void ReadInputs()
    {
        if ( Input .GetKeyDown(KeyCode .A) )
        {
            if ( ( Time .time - lastTapTimeLeft ) < tapSpeed )//Control del DOUBLE TAP, mira si se esta haciendo tap(girar) o Doble tap(casteo de DASH)
            {
                DoubleTapLEFT = true;
                DoubleTapRIGHT = false;
            }
            lastTapTimeLeft = Time .time;
        }
        if ( Input .GetKeyDown(KeyCode .D) )
        {
            if ( ( Time .time - lastTapTimeRight ) < tapSpeed )//Control del DOUBLE TAP, mira si se esta haciendo tap(girar) o Doble tap(casteo de DASH)
            {
                DoubleTapRIGHT = true;
                DoubleTapLEFT = false;
            }
            lastTapTimeRight = Time .time;
        }
        if ( Input .GetKeyUp(KeyCode .A) )
        {
            DoubleTapLEFT = false;
        }
        if ( Input .GetKeyUp(KeyCode .D) )
        {
            DoubleTapRIGHT = false;
        }


        LEFT = Input .GetKey(KeyCode .A);
        RIGHT = Input .GetKey(KeyCode .D);

        UP = Input .GetKey(KeyCode .W);
        DOWN = Input .GetKey(KeyCode .S);
        JumpLEFT = Input .GetKeyUp(KeyCode .Q);
        JumpRIGHT = Input .GetKeyUp(KeyCode .E);
    }

    public bool UP { get; private set; }
    public bool DOWN { get; private set; }
    public bool LEFT { get; private set; }
    public bool RIGHT { get; private set; }
    public bool DoubleTapRIGHT { get; private set; }
    public bool DoubleTapLEFT { get; private set; }
    public bool JumpRIGHT { get; private set; }
    public bool JumpLEFT { get; private set; }
}
