using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : Hero
{
    IAttacking dealsDamage = new FreezingAttack();

    [SerializeField] DamagingFlyingObject mageBall;
    [SerializeField] internal Vector3 initialPosCorrection;

    public override void DealsDamage(BattleHex target)
    {

    }
    public override IAdjacentFinder GetTypeOfHero()
    {
        IAdjacentFinder adjFinder = new PositionsForFlying();
        return adjFinder;
    }
    public override void DefineTargets()
    {
        IDefineTarget wayToLookForTargets = new TargetPlayerRange();
        wayToLookForTargets.DefineTargets(this);
    }
    public override void HeroIsAtacking()
    {
        base.HeroIsAtacking();
        GetComponent<Animator>().SetTrigger("isAttacking");
        InstantiateBall();
    }
    private void InstantiateBall()
    {
        Vector3 positionForArrow = new Vector3(transform.position.x, transform.position.y + initialPosCorrection.y, transform.position.z);
        Hero currentTarget = BattleController.currentTarget.GetComponentInChildren<Hero>();
        Quaternion rotation = CalcRotation.CalculateRotation(currentTarget); 
        DamagingFlyingObject ball = Instantiate(mageBall, positionForArrow, rotation, transform);
        ball.FireArrow(dealsDamage);
    }
}
