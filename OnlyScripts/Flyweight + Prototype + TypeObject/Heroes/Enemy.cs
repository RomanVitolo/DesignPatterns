using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    BattleController battleController;
    AllPosForGroundAI tocheckTheField;
    BattleHex hexToOccupy;
    AvailablePos availablePos;
    Move move;
    Hero hero;

    public List<BattleHex> PosToOccupy = new List<BattleHex>();
   
    private List<BattleHex> allTargets = new List<BattleHex>();
    private List<BattleHex> closeTargets = new List<BattleHex>();
    
    
    private void Start()
    {
        battleController = FindObjectOfType<BattleController>();
        tocheckTheField = GetComponent<AllPosForGroundAI>();
        availablePos = GetComponent<AvailablePos>();
        hero = GetComponent<Hero>();
        move = GetComponent<Move>();
        move.lookingToTheRight = false;
    }
    public void Aisturnbegins(IInitialHexes getInitialHexes)
    {
        int stepsLimit = battleController.stepsToCheckWholeField;
        BattleHex startintHex = GetComponentInParent<BattleHex>();
        tocheckTheField.GetAvailablePositions(stepsLimit, getInitialHexes, startintHex);
        CollectAllPosToOccupy();
        AIMakesDecision();
    }
    List<BattleHex> CollectAllPosToOccupy()
    {
        PosToOccupy.Clear();
        foreach (BattleHex hex in FieldManager.activeHexList)
        {
            if (hex.distanceText.distanceFromStartingPoint <= hero.heroData.CurrentVelocity)
            {
                PosToOccupy.Add(hex);
            }
        }
        return PosToOccupy;
    }
    private List<BattleHex> CheckIfAttackIsAvailable()//checks if the player’s regiment is in the attack zone
    {
        int currentVelocity = BattleController.currentAtacker.heroData.CurrentVelocity;
        closeTargets.Clear();
        List<BattleHex> allTargets = battleController.IsLookingForPotentialTargets();
        foreach (BattleHex hex in allTargets)
        {
            if (hex.distanceText.distanceFromStartingPoint <= currentVelocity + 1)
            {
                closeTargets.Add(hex);
            }
        }
        return closeTargets;
    }
    public BattleHex AISelectsTargetToAttack()
    {
        allTargets.Clear();
        if (CheckIfAttackIsAvailable().Count > 0)
        {
            allTargets = CheckIfAttackIsAvailable().OrderBy(hero => hero.GetComponentInChildren<Hero>().heroData.HPCurrent).ToList();
        }
        else
        {
            allTargets = battleController.IsLookingForPotentialTargets().OrderBy(hero => hero.distanceText.distanceFromStartingPoint).
                        ThenBy(hero => hero.GetComponentInChildren<Hero>().heroData.HPCurrent).ToList();
        }
        BattleController.currentTarget = allTargets[0].GetComponentInChildren<Hero>();
        return allTargets[0];
    }
    void AIIStartsMoving(BattleHex targetToAttack)
    {
        battleController.CleanField();
        targetToAttack.DefineMeAsStartingHex();
        int stepsLimit = battleController.stepsToCheckWholeField;
        IInitialHexes getInitialHexes = new InitialPos();
        tocheckTheField.GetAvailablePositions(stepsLimit, getInitialHexes, targetToAttack);
        IAdjacentFinder adjFinder = BattleController.currentAtacker.GetTypeOfHero();
        AIDefinesPath(adjFinder);
    }
    private BattleHex AISelectsPosToOcuppy()
    {
        List<BattleHex> OrderedPos = PosToOccupy.OrderBy(s => s.distanceText.distanceFromStartingPoint).ToList();
        for (int i = 0; i < OrderedPos.Count; i++)
        {
            if (OrderedPos[i].GetComponentInChildren<Hero>() == null)
            {
                hexToOccupy = OrderedPos[i];
                break;
            }
        }
        return hexToOccupy;
    }
    void AIMakesDecision()
    {
        BattleHex targetToAttack = AISelectsTargetToAttack();
        if (targetToAttack.distanceText.distanceFromStartingPoint > 1)
        {
            AIIStartsMoving(targetToAttack);
        }
        else
        {
            hero.HeroIsAtacking();
        }
    }
    void AIDefinesPath(IAdjacentFinder adjFinder)
    {
        BattleController.targetToMove = AISelectsPosToOcuppy();
        battleController.CleanField();
        IInitialHexes getInitialHexes = new InitialPos();
        int stepsLimit = hero.heroData.CurrentVelocity;
        BattleHex startingHex = BattleController.currentAtacker.GetComponentInParent<BattleHex>();
        startingHex.DefineMeAsStartingHex();
        availablePos.GetAvailablePositions(stepsLimit, adjFinder, getInitialHexes);
        GetComponent<OptimalPath>().MatchPath();
        move.StartsMoving();
    }
}
