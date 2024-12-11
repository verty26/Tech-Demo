using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomItemGenerator : MonoBehaviour
{
    [SerializeField] private List<SwordItem> swords = new List<SwordItem>();

    [Header("Settins")]
    public Vector2 damageRange;
    public Vector2 critRange;
    public Vector2 attackSpeedRange;
    public Vector2 luckRange;
    public Vector2 priceRange;

    [Header("UI")]
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private Image itemImage;

    public void GenerateItem()
    {
        int randomSword = Random.Range(0, swords.Count);

        statsText.text = swords[randomSword].itemName;
        itemImage.sprite = swords[randomSword].itemImage;

        int randomDamage = (int)Random.Range(damageRange.x, damageRange.y);
        int randomCrit = (int)Random.Range(critRange.x, critRange.y);
        int randomAttackSpeed = (int)Random.Range(attackSpeedRange.x, attackSpeedRange.y);
        int randomLuck = (int)Random.Range(luckRange.x, luckRange.y);
        int randomPrice = (int)Random.Range(priceRange.x, priceRange.y);

        statsText.text += "\nDamage: " + randomDamage;
        statsText.text += "\nCrit Chance: " + randomCrit;
        statsText.text += "\nAttack Speed: " + randomAttackSpeed;
        statsText.text += "\nLuck: " + randomLuck;
        statsText.text += "\nPrice: " + randomPrice;
    }

}

[System.Serializable]
public class SwordItem
{
    public string itemName;
    public Sprite itemImage;
}