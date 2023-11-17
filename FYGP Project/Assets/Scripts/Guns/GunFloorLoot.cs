using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GunFloorLoot : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI gunTitleText;
    public TextMeshProUGUI bulletTypeText;
    public TextMeshProUGUI rarityText;
    public Image gunIconImage;
    private GunData gunData;
    public Image newBackground;
    private string  rarityName;
    /*private Image[] raritiesList;*/
    void Start()


    {

        gunData = GetComponentInParent<ItemCollection>().item as GunData;

        gunTitleText.text = ("Name: " + gunData.name);
        bulletTypeText.text = ("Bullet type: " + gunData.type.ToString()); 
        rarityText.text = ("Rarity: " + gunData.rarity.ToString());
        gunIconImage.sprite = gunData.icon;
        rarityName = gunData.rarity.ToString();

        switch (rarityName)
        {
            case "Common":
                newBackground.sprite = Resources.Load<Sprite>("Common");
                break;
            case "Uncommon":
                newBackground.sprite = Resources.Load<Sprite>("Uncommon");
                break;
            case "Rare":
                newBackground.sprite = Resources.Load<Sprite>("Rare");
                break;
            case "Epic":
                newBackground.sprite = Resources.Load<Sprite>("Epic");
                break;
            case "Legendary":
                newBackground.sprite = Resources.Load<Sprite>("Legendary");
                break;
        }

    }

    // Update is called once per framea
    void Update()
    {
        
    }

    
         

}
