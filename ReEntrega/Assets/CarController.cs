using System .Collections;
using System .Collections .Generic;
using UnityEngine;

public class CarController2 : MonoBehaviour
{
    #region EASING FUNCIONES
    public Easing .Ease easeAcelerar;
    public Easing .Ease easeFrenar;
    public Easing .Ease easeGiro;

    public Easing .Function fAcelerar;
    public Easing .Function fGiro;
    public Easing .Function fFrenar;
    #endregion

    #region Velocidades y Counters
    private float normalVspeed = 10;
    private float maxVspeed = 20;
    private float minVspeed = 5f;
    private float turnSpeed = 30;

    private float vSpeed = 0;
    private float hSpeed = 0;

    private float counterUp = 0;
    private float counterDown = 0;
    private float counterLeft = 0;
    private float counterRight = 0;

    #endregion

    [SerializeField] public CarSettings carSettings;
    private ICarInputs carInputs;
    private Rigidbody rb;

    private bool useAi;
    public bool canAccelerate = true;//Si el coche puede acelerar

    public bool canSideMove = true; //Si el coche puede moverse horizontalmente o no
    public bool sideMoving;//Si el coche SE ESTA moviendo horizontalmente
    public bool accelerating;//Si el coche esta ACELERANDO
    public bool braking;//Si el coche esta ACELERANDOs
    public bool dashing = false;//Si el coche esta haciendo un DASH
    public bool casting = false;//Si el coche esta CASTEANDO 


    private void Awake()
    {
        carInputs = carSettings .UseAi ? new AiInputs() as ICarInputs : new PlayerInputs();
        useAi = carSettings .UseAi;

        fAcelerar = Easing .GetEasingFunction(easeAcelerar);
        fGiro = Easing .GetEasingFunction(easeGiro);
        fFrenar = Easing .GetEasingFunction(easeFrenar);

        vSpeed = normalVspeed;
    }

    void Update()
    {
        carInputs .ReadInputs();
    }
}
