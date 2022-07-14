using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stack : MonoBehaviour
{
    Turn turn;
    Hero parentHero;
   
    public TextMeshProUGUI stackText;
    
    [SerializeField] float iterationCntrl;
    private int stack;  
    private int iterationVal;  
    
    public int IterationVal
    {
        get { return iterationVal; }
        set
        {
            if (value < 1) { iterationVal = 1; }
            else { iterationVal = value; }
        }
    }
    void Start()
    {
        parentHero = GetComponentInParent<Hero>();
        stackText = GetComponent<TextMeshProUGUI>();
        DisplayCurrentStack(parentHero.heroData.StackCurrent);
        turn = FindObjectOfType<Turn>();
    }

    public void DisplayCurrentStack(int currentStack)
    {
        parentHero.heroData.StackCurrent = currentStack;
        stackText.text = currentStack.ToString();
    }
 
    public IEnumerator CountDownToTargetStack(int currentValue, int targetValue)
    {
        int diff = currentValue - targetValue;
        IterationVal = Mathf.FloorToInt(diff * Time.deltaTime / iterationCntrl);
        WaitForSeconds wait = new WaitForSeconds(0.01f);
        while (currentValue >= targetValue + IterationVal)
        {
            currentValue -= IterationVal;
            DisplayCurrentStack(currentValue);
            yield return wait;
        }
        DisplayCurrentStack(targetValue);
        CheckIfHeroIsKilled();
    }
    void CheckIfHeroIsKilled()
    {
        if (parentHero.heroData.StackCurrent == 0)
        {
            parentHero.GetComponent<Animator>().SetTrigger("IsDead");
        }
        else
        {
            turn.TurnIsCompleted();
        }
    }
}
