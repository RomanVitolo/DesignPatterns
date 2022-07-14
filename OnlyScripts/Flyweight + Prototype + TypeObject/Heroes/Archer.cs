using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Hero
{
    IAttacking dealsDamage = new SimpleMeleeAttack();
    
    [SerializeField] DamagingFlyingObject arrow;
    [SerializeField] internal Vector3 initialPosCorrection;

    public override void DealsDamage(BattleHex target)
    {

    }

    public override IAdjacentFinder GetTypeOfHero()
    {
        IAdjacentFinder adjFinder = new PositionsForGround();
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
        InstantiateArrow();
    }
    private void InstantiateArrow()
    {
        Vector3 positionForArrow = new Vector3(transform.position.x,transform.position.y + initialPosCorrection.y, transform.position.z);
        Hero currentTarget = BattleController.currentTarget.GetComponentInChildren<Hero>();
        Quaternion rotation = CalcRotation.CalculateRotation(currentTarget); 
        DamagingFlyingObject Arrow = Instantiate(arrow, positionForArrow, rotation, transform);
        Arrow.FireArrow(dealsDamage);
    }
}