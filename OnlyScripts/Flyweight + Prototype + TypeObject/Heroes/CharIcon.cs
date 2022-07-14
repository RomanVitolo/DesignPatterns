using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharIcon : MonoBehaviour
{
    StorageMNG storage;
    
    [SerializeField] internal Image heroImage;
    [SerializeField] internal Image backGround;
    [SerializeField] internal TMPro.TextMeshProUGUI stackText;
    [SerializeField] internal CharAttributes charAttributes;
    string losses = "0";
    private void Start()
    {
        storage = GetComponentInParent<StorageMNG>();
        StorageMNG.OnClickOnGrayIcon += ReturnDefaultState; 
    }
    internal void FillIcon()
    {
        heroImage.sprite = charAttributes.heroSprite;
        stackText.text = charAttributes.stack.ToString();
        charAttributes.isDeployed = false;
    }
    
    internal void FillIconWhenGameIsOver(CharAttributes Attributes)
    {
        heroImage.sprite = Attributes.heroSprite;
        if (Attributes.Calculatelosses() != 0)
        {
            losses = "- " + Attributes.Calculatelosses();
        }

        stackText.text = losses;
    }
    public void IconClicked()
    {
        StorageMNG storage = GetComponentInParent<StorageMNG>();
        if (!charAttributes.isDeployed)
        {
            storage.TintIcon(this);
        }
        else
        {
            storage.RemoveRegiment(charAttributes);
        }
    }
    public void HeroIsDeployed()
    {
        backGround.sprite = storage.deployedRegiment;
        charAttributes.isDeployed = true;
        
    }
    public void ReturnDefaultState(CharAttributes selectedCharAttributes)
    {
        if (selectedCharAttributes == charAttributes)
        {
            backGround.sprite = GetComponentInParent<StorageMNG>().defaultIcon; 
            charAttributes.isDeployed = false;
        }
    }
}