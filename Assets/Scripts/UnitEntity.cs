using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEntity : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer { get; set; }

    public string m_sUnitName;
    public int m_iUnitHP;
    public int m_iUnitAtk;
    public int m_iUnitDef;
    public int m_iCurrentHP;
    public int m_iUnitLevel;


    //���� 3����
    IAttackBehavior m_AttackBehavior;
    IAttackBehavior m_AttackBehavior2;
    IAttackBehavior m_AttackBehavior3;

    //�ε����� �����ϴ°� ���ҰŰ��Ƽ� ��������ϴ�
    IAttackBehavior[] m_AttackBehaviors;

    public void Awake()
    {
        if (m_sUnitName != null)
        {
            SetUnit(m_sUnitName);
            //Attack();
        }
    }
    //GameManager�� �ִ� UnitTable SO�� �ִ� ������ ������� �ش� ���ӿ�����Ʈ �ʱ�ȭ
    public void SetUnit(string className)
    {
        
        var UnitData = GameManager.Instance.GetUnitData(className);

        m_sUnitName = UnitData.m_sUnitName;
        gameObject.name += "-" + m_sUnitName;

        m_iUnitHP = UnitData.m_iUnitHP;
        m_iUnitAtk = UnitData.m_iUnitAtk;
        m_iUnitDef = UnitData.m_iUnitDef;
        m_iUnitLevel = UnitData.m_iUnitLevel;
        m_iCurrentHP = m_iUnitHP;

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sprite = UnitData.m_UnitSprite;
        m_AttackBehaviors = new IAttackBehavior[3];

        //�ε����� �����ϴ°� ���ҰŰ��Ƽ� ��������ϴ�
        m_AttackBehaviors[0] = Instantiate(UnitData.m_AttackBehav_1);
        m_AttackBehaviors[1] = Instantiate(UnitData.m_AttackBehav_2);
        m_AttackBehaviors[2] = Instantiate(UnitData.m_AttackBehav_3);

        m_AttackBehavior = Instantiate(UnitData.m_AttackBehav_1);
        m_AttackBehavior2 = Instantiate(UnitData.m_AttackBehav_2);
        m_AttackBehavior3 = Instantiate(UnitData.m_AttackBehav_3);
    }


    public int Attack(UnitEntity Atker, UnitEntity Defender)
    {
        return m_AttackBehavior.ExecuteAttack(Atker,Defender);
    }

    //�ε����� �����ϴ°� ���ҰŰ��Ƽ� ��������ϴ�
    public int AttackByIndex(UnitEntity Atker, UnitEntity Defender,int index)
    {
        return m_AttackBehaviors[index].ExecuteAttack(Atker, Defender);
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
        if (m_iCurrentHP > m_iUnitHP)
            // ���� ü���� �ִ� ü������ ����
            m_iCurrentHP = m_iUnitHP;
    }

}