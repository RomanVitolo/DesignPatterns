using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFairy : Hero
{
    IAttacking dealsDamage = new SimpleMeleeAttack();
    public override void DealsDamage(BattleHex target)
    {
        dealsDamage.HeroIsDealingDamage(this, BattleController.currentTarget);
    }
    public override IAdjacentFinder GetTypeOfHero()
    {
        IAdjacentFinder adjFinder = new PositionsForFlying();
        return adjFinder;
    }
    public override void DefineTargets()
    {
        IDefineTarget wayToLookForTargets = new TargetPlayerMelee();
        wayToLookForTargets.DefineTargets(this);
    }
    public override void HeroIsAtacking()
    {
        base.HeroIsAtacking();
        GetComponent<Animator>().SetTrigger("IsAttacking");
    }
}