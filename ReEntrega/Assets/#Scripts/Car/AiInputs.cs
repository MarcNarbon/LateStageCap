using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiInputs : ICarInputs {

    public void ReadInputs() {
        //escribir modificacion imputs ia aqui.
    }

    public bool UP { get; private set; }
    public bool DOWN { get; private set; }
    public bool LEFT { get; private set; }
    public bool RIGHT { get; private set; }
    public bool DoubleTapRIGHT { get; private set; }
    public bool DoubleTapLEFT { get; private set; }
    public bool JumpLEFT { get; private set; }
    public bool JumpRIGHT { get; private set; }
}
