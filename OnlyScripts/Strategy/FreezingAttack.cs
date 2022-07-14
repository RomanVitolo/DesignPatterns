using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezingAttack : MonoBehaviour, IAttacking
{
    DamageCounter damageController = new DamageCounter();
    int targetStack;
    public void HeroIsDealingDamage(Hero atacker, Hero Target)
    {        
        targetStack = damageController.CountTargetStack(atacker, Target);
        int currentInt = Target.heroData.StackCurrent;
        Freeze(Target);        
        Target.heroData.StackCurrent = targetStack;
        Target.stack.StartCoroutine(Target.stack.CountDownToTargetStack(currentInt, targetStack));
    }

    void Freeze(Hero Target)
    {
        Target.heroData.InitiativeCurrent = 0;
        Target.GetComponent<SpriteRenderer>().color = new Color32(135, 255, 255, 255);
    }
}
