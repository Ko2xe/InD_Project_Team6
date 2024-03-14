using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntity : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer { get; set; }

    public string m_sUnitName;
    public int m_iHealthPoint;
    public float m_fAttackDamage;
    public int m_iAttackDamage;
    public int m_iCurrentHP;
    public int m_iUnitLevel;

    IAttackBehavior m_AttackBehavior;

    public void Awake()
    {
        if (m_sUnitName != null)
        {
            SetUnit(m_sUnitName);
            //Attack();
        }
    }
    //UnitTable SO�� �ִ� ������ ������� �ش� ���ӿ�����Ʈ �ʱ�ȭ
    public void SetUnit(string className)
    {
        
        var UnitData = GameManager.Instance.GetUnitData(className);

        m_sUnitName = UnitData.m_sUnitName;


        gameObject.name += "-" + m_sUnitName;

        m_iHealthPoint = UnitData.m_iHealthPoint;
        m_iAttackDamage = UnitData.m_iAttackDamage;
        m_iUnitLevel = UnitData.m_iUnitLevel;
        m_iCurrentHP = m_iHealthPoint;

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sprite = UnitData.m_UnitSprite;

        m_AttackBehavior = Instantiate(UnitData.m_AttackBehav_1);
    }


    public int Attack()
    {
        return m_AttackBehavior.ExecuteAttack(m_iAttackDamage);
    }

    // ������� �Դ� �޼���
    public bool TakeDamage(int dmg)
    {
        // ���� ü�¿��� ������� ���ҽ�Ŵ
        m_iCurrentHP -= dmg;

        // ���� ���� ü���� 0 ���϶��
        if (m_iCurrentHP <= 0)
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
        m_iCurrentHP += amount;

        // ���� ���� ü���� �ִ� ü���� �ʰ��ϸ�
        if (m_iCurrentHP > m_iHealthPoint)
            // ���� ü���� �ִ� ü������ ����
            m_iCurrentHP = m_iHealthPoint;
    }

}
