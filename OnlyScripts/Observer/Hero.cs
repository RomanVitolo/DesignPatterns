using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero : MonoBehaviour
{
    StartBTN startBTN;
    Move moveCpmnt;
    BattleController battleController;
    
    public CharAttributes heroData;
    public Stack stack;
    
    internal Turn turn;
    
    //public int velocity = 5;
    
    
    private void Awake()
    {
        heroData.SetCurrentAttributes();
        moveCpmnt = GetComponent<Move>();
        battleController = FindObjectOfType<BattleController>();
        turn = FindObjectOfType<Turn>();
    }
    private void Start()
    {
        StorageMNG.OnClickOnGrayIcon += DestroyMe; //Me subcribo al Destroy 
        startBTN = FindObjectOfType<StartBTN>();
        stack = GetComponentInChildren<Stack>();
        Turn.OnNewRound += heroData.SetDefaultVelocityAndInitiative;
    }
    public abstract void DealsDamage(BattleHex target);

    private void DestroyMe(CharAttributes SOHero)
    {
        if (SOHero == heroData)
        {
            BattleHex parentHex = GetComponentInParent<BattleHex>();
            parentHex.MakeMeDeploymentPosition();
            startBTN.ControlStartBTN();
            Destroy(gameObject);
        }
    }
    void OnDisable()
    {
        StorageMNG.OnClickOnGrayIcon -= DestroyMe;//Unsub a las notificaciones
    }
    public abstract IAdjacentFinder GetTypeOfHero();
    public abstract void DefineTargets();
    public virtual void HeroIsAtacking()
    {
        Vector3 targetPos = BattleController.currentTarget.transform.position;
        moveCpmnt.ControlDirection(targetPos);
    }
    public void PlayersTurn(IInitialHexes getInitialHexes)
    {
        IAdjacentFinder adjFinder = GetTypeOfHero();
        int stepsLimit = heroData.CurrentVelocity;
        GetComponent<AvailablePos>().GetAvailablePositions(stepsLimit, adjFinder, getInitialHexes);
        DefineTargets();
    }
    public void HeroIsKilled()
    {
        Turn.OnNewRound -= heroData.SetDefaultVelocityAndInitiative;
        battleController.RemoveHeroWhenItIsKilled(this);
    }
}
