using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="HeroAttributes", menuName ="Fighter")]
public class CharAttributes : ScriptableObject
{
    [Header("Default Attributes")]
    [SerializeField] private int velocity;
    [SerializeField] private float initiative;
    [SerializeField] private int hp;
    [SerializeField] private int atack;
    [SerializeField] private int resistance;
    [SerializeField] int atackdistanse;
    
    public  int stack;

    [Header("General Attributes")] 
    [SerializeField] private bool isRanger;
    [SerializeField] private bool isFlying;
    
    public bool isDeployed;
    public Sprite heroSprite;
    public Hero heroSO;

    
    [Header("Current Attributes")]
    float initiativeCurrent;
    public float InitiativeCurrent
    {
        get { return initiativeCurrent; }
        set { initiativeCurrent = value; }
    }
    int hpCurrent;
    public int HPCurrent
    {
        get { return hpCurrent; }
        set { hpCurrent = value; }
    }
    int atackCurrent;
    public int AtackCurrent
    {
        get { return atackCurrent; }
        set { atackCurrent = value; }
    }
    int resistanceCurrent;
    public int ResistanceCurrent
    {
        get { return resistanceCurrent; }
        set { resistanceCurrent = value; }
    }
    int stackCurrent;
    public int StackCurrent
    {
        get { return stackCurrent; }
        set
        {
            if (value > 0) { stackCurrent = value; }
            else { stackCurrent = 0; }
        }
    }
    public int Atackdistanse
    {
        get
        {
            if (!isRanger) { return 1; }
            else { return atackdistanse; }
        }
    }
    int velocityCurrent;
    public int CurrentVelocity
    {
        get { return velocityCurrent; }
        set { velocityCurrent = value; }
    }
    public void SetCurrentAttributes()
    {
        hpCurrent = hp;
        atackCurrent = atack;
        resistanceCurrent = resistance;
        stackCurrent = stack;
        initiativeCurrent = initiative;
        velocityCurrent = velocity;
    }
    public void SetDefaultVelocityAndInitiative()
    {
        velocityCurrent = velocity;
        initiativeCurrent = initiative;
    }
    public int Calculatelosses()
    {
        return stack - stackCurrent;
    }
}

