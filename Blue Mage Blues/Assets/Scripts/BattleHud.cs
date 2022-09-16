using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHud : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Slider hpBar;

    public void SetHud(Unit unit){
        nameText.text = unit.unitName;
        levelText.text = "Lvl " + unit.unitLevel;
        hpBar.maxValue = unit.maxHP;
        hpBar.value = unit.currentHP;
    }

    public void SetHP(int hp){
        hpBar.value = hp;
    }
}
