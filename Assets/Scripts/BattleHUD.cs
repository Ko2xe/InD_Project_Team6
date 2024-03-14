using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    // UI ��ҵ�
    public Text nameText;   // �̸��� ǥ���ϴ� �ؽ�Ʈ
    public Text levelText;  // ������ ǥ���ϴ� �ؽ�Ʈ
    public Slider hpSlider; // ü���� ǥ���ϴ� �����̴�

    // HUD�� �����ϴ� �޼���
    public void SetHUD(Unit unit)
    {
        // ������ �̸��� �ؽ�Ʈ�� ����
        nameText.text = unit.unitName;
        // ������ ������ �ؽ�Ʈ�� ����
        levelText.text = "Lvl " + unit.unitLevel;
        // �����̴��� �ִ밪�� ������ �ִ� ü������ ����
        hpSlider.maxValue = unit.maxHP;
        // �����̴��� ��(ü��)�� ������ ���� ü������ ����
        hpSlider.value = unit.currentHP;
    }

    // ü���� ������Ʈ�ϴ� �޼���
    public void SetHP(int hp)
    {
        // �����̴��� ��(ü��)�� �־��� ������ ����
        hpSlider.value = hp;
    }
}
