using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Slider hpSlider;

    public void SetHUD(Character character)
    {
        nameText.text = character.charName;
        hpSlider.maxValue = character.maxHP;
        hpSlider.value = character.currentHP;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
