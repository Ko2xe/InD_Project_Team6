using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewBaseAttack", menuName = "AttackBehavior/Base")]
public class SOBaseAttack : SOAttackBase
{
    //���ֵ��� �������̰ų� �������� �����Դϴ�.
    //SO�� ����� SOInstance�� �����ϰ� ����մϴ�.

    //�� �κп��� �ش� ������ ���� ����� ������ �� �ֽ��ϴ�.
    float CriticalChance = 10.0f;
    float AttackMag = 1.2f;
    
    public override int ExecuteAttack(UnitEntity Atker, UnitEntity Defender)
    {
        
        int AttackDamage =(int)(Atker.m_iUnitAtk * AttackMag);
        int finalAttackDamage = AttackDamage - Defender.m_iUnitDef;
        
        Debug.Log("BaseAttack!" + finalAttackDamage);

        Defender.m_iCurrentHP -= finalAttackDamage;
        return finalAttackDamage;
    }
}
