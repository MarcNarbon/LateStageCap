using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Car/Settings", fileName = "CarData")]
public class CarSettings : ScriptableObject
{
    //funciones de easing para poder asignarlas y cambiarlas desde el inspector.
    [SerializeField] static private Easing .Ease easeAcelerar;
    [SerializeField] static private Easing .Ease easeFrenar;
    [SerializeField] static private Easing .Ease easeAccelerarToNormal;
    [SerializeField] static private Easing .Ease easeFrenarToNormal;
    [SerializeField] static private Easing .Ease easeGiro;
    [SerializeField] static private Easing .Ease easeDash;
    [SerializeField] static private Easing .Ease easeCastDash;

    //Asignacion de las funciones de EASING
    private Easing .Function fAcelerar = Easing .GetEasingFunction(easeAcelerar);
    private Easing .Function fFrenar = Easing .GetEasingFunction(easeFrenar);
    private Easing .Function fAccelerarToNormal = Easing .GetEasingFunction(easeAccelerarToNormal);
    private Easing .Function fFrenarToNormal = Easing .GetEasingFunction(easeFrenarToNormal);
    private Easing .Function fGiro = Easing .GetEasingFunction(easeGiro);
    private Easing .Function fDash = Easing .GetEasingFunction(easeDash);
    private Easing .Function fCastDash = Easing .GetEasingFunction(easeCastDash);

    [SerializeField] private bool useAi;

    [SerializeField] private float averageVerticalSpeed;
    [SerializeField] private float maxVerticalSpeed;
    [SerializeField] private float minVerticalSpeed;
    [SerializeField] private float maxHorizontalSpeed; 
    [SerializeField] private int maxHealth;

    [SerializeField] GameObject carMeshPrefab;

    public Easing .Function FAcelerar { get { return fAcelerar; } }
    public Easing .Function FFrenar { get { return fFrenar; } }
    public Easing .Function FAccelerarToNormal { get { return fAccelerarToNormal; } }
    public Easing .Function FFrenarToNormal { get { return fFrenarToNormal; } }
    public Easing .Function FGiro { get { return fGiro; } }
    public Easing .Function FDash { get { return fDash; } }
    public Easing .Function FCastDash { get { return fCastDash; } }
    public GameObject CarMeshPrefab { get { return carMeshPrefab; } }
    public bool UseAi { get { return useAi; } }
    public int MaxHealth { get { return maxHealth; } }
    public float AverageVerticalSpeed { get { return averageVerticalSpeed; } }
    public float MaxVerticalSpeed { get { return maxVerticalSpeed; } }
    public float MinVerticalSpeed { get { return minVerticalSpeed; } }
    public float MaxHorizontalSpeed { get { return maxHorizontalSpeed; } }


}
