using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // ������ �̸�
    public string g_sUnitName;

    // ������ ����
    public int g_iUnitLevel;

    // ������ ������ ������
    public int g_iDamage;

    // ������ �ִ� ü��
    public int g_iMaxHP;

    // ������ ���� ü��
    public int g_iCurrentHP;

    IAttackBehavior m_AttackBehavior;


    // ������� �Դ� �޼���
    public bool TakeDamage(int dmg)
    {
        // ���� ü�¿��� ������� ���ҽ�Ŵ
        g_iCurrentHP -= dmg;

        // ���� ���� ü���� 0 ���϶��
        if (g_iCurrentHP <= 0)
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
        g_iCurrentHP += amount;

        // ���� ���� ü���� �ִ� ü���� �ʰ��ϸ�
        if (g_iCurrentHP > g_iMaxHP)
            // ���� ü���� �ִ� ü������ ����
            g_iCurrentHP = g_iMaxHP;
    }
}
