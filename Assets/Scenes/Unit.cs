using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // ������ �̸�
    public string unitName;

    // ������ ����
    public int unitLevel;

    // ������ ������ ������
    public int damage;

    // ������ �ִ� ü��
    public int maxHP;

    // ������ ���� ü��
    public int currentHP;

    // ������� �Դ� �޼���
    public bool TakeDamage(int dmg)
    {
        // ���� ü�¿��� ������� ���ҽ�Ŵ
        currentHP -= dmg;

        // ���� ���� ü���� 0 ���϶��
        if (currentHP <= 0)
            // ������ �׾����� ��Ÿ���� true ��ȯ
            return true;
        else
            // �ƴϸ� ��������� ��Ÿ���� false ��ȯ
            return false;
    }

    // ü���� ȸ���ϴ� �޼���
    public void Heal(int amount)
    {
        // ȸ������ ���� ü�¿� ����
        currentHP += amount;

        // ���� ���� ü���� �ִ� ü���� �ʰ��ϸ�
        if (currentHP > maxHP)
            // ���� ü���� �ִ� ü������ ����
            currentHP = maxHP;
    }
}
