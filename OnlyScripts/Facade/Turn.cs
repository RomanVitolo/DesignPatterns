using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn : MonoBehaviour
{
    BattleController battleController;
    IInitialHexes getInitialHexes = new InitialPos();
    FieldManager parent;
    
    public delegate void StartNewRound();
    public static event StartNewRound OnNewRound;
    
    [SerializeField] GameOver gameOverPanel;
    private void Start()
    {
        battleController = GetComponent<BattleController>();
        StartBTN.OnStartingBattle += InitializeNewTurn;
        parent = FindObjectOfType<FieldManager>();
    }
    public void InitializeNewTurn()
    {
        battleController.CleanField();
        battleController.DefineNewAtacker();
        Hero currentAtacker = BattleController.currentAtacker;
        GetStartingHex();
        if (currentAtacker.GetComponent<Enemy>() == null)
        {
            IInitialHexes getInitialHexes = new InitialPos();
            currentAtacker.PlayersTurn(getInitialHexes);

        }
        else
        {
            IInitialHexes getInitialHexes = new InitialPosAI();
            currentAtacker.GetComponent<Enemy>().Aisturnbegins(getInitialHexes);
        }
    }
    
    private void GetStartingHex()
    {
        BattleHex startingHex = BattleController.currentAtacker.GetComponentInParent<BattleHex>();
        startingHex.DefineMeAsStartingHex();
    }
    public void TurnIsCompleted()
    {
        StartCoroutine(NextTurnOrGameOver());
    }
    public IEnumerator NextTurnOrGameOver()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        yield return wait;
        battleController.events.gameObject.SetActive(true);
        List<Hero> allFighters = battleController.DefineAllFighters();
        if (IfThereIsAIRegiment(allFighters) && IfThereIsPlayerRegiment(allFighters))
        {
            NextTurnOrNextRound(allFighters);
        }
        else
        {
            battleController.CleanField();
            GameOver GameOver = Instantiate(gameOverPanel, parent.transform);
            GameOver.DefeatOrVictory(IfThereIsPlayerRegiment(allFighters)); RemoveAllHeroes(allFighters);
        }
    }
    private void RemoveAllHeroes(List<Hero> allFighters)
    {
        foreach (Hero hero in allFighters)
        {
            Destroy(hero.gameObject);
        }
    }
    bool IfThereIsAIRegiment(List<Hero> allFighters)
    {
        return allFighters.Exists(x => x.gameObject.GetComponent<Enemy>());
    }
     bool IfThereIsPlayerRegiment(List<Hero> allFighters)
    {
        return allFighters.Exists(x => !x.gameObject.GetComponent<Enemy>());
    }
    private void NextTurnOrNextRound(List<Hero> allFighters)
    {
        if (allFighters.Exists(x => x.heroData.InitiativeCurrent > 0))
        {
            InitializeNewTurn();
        }
        else
        {
            OnNewRound();
            InitializeNewTurn();
        }
    }
}
