using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageMNG : MonoBehaviour
{
    [SerializeField] internal Sprite selectedIcon;
    [SerializeField] internal Sprite defaultIcon;
    [SerializeField] internal Sprite deployedRegiment;
    [SerializeField] internal CurrentProgress currentProgress;
    [SerializeField] private CharIcon iconPrefab;
    
    ScrollRect scrollRect;
        
    List<CharAttributes> regimentIcons = new List<CharAttributes>();

    public static event Action<CharAttributes> OnRemoveHero;   
    public delegate void DeleteHero(CharAttributes SOofHero);
    public static event DeleteHero OnClickOnGrayIcon;
    public CharIcon[] charIcons;
 
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        CallHeroIcons();
        StartBTN.OnStartingBattle += DisableMe;
        charIcons = GetComponentsInChildren<CharIcon>();
    }
    private void CallHeroIcons()
    {
        regimentIcons = currentProgress.heroesOfPlayer;
        Transform parentOfIcons = scrollRect.content.transform;
        for (int i = 0; i < regimentIcons.Count; i++)
        {
            CharIcon fighterIcon = Instantiate(iconPrefab, parentOfIcons);
            fighterIcon.charAttributes = regimentIcons[i];
            fighterIcon.FillIcon();
        }
    }
    internal void TintIcon(CharIcon clickedIcon)
    {
        CharIcon[] charIcons = GetComponentsInChildren<CharIcon>();
        foreach (CharIcon icon in charIcons)
        {
            if (!icon.charAttributes.isDeployed)
                icon.backGround.sprite = defaultIcon;
        }
        clickedIcon.backGround.sprite = selectedIcon;
        Deployer.readyForDeploymentIcon = clickedIcon;
    }

    private void RemoveHero(Hero hero)
    {
        BattleHex parentHex = hero.GetComponentInParent<BattleHex>();
        parentHex.MakeMeDeploymentPosition();
        Destroy(hero.gameObject);
    }
    public void RemoveRegiment(CharAttributes SOHero)
    {
       OnClickOnGrayIcon(SOHero);
    }
    private void DisableMe()
    {
        gameObject.SetActive(false);
    }

}