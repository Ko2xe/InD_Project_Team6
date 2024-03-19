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
    int AttackMag = 1;
    
    public override int ExecuteAttack(UnitEntity Atker, UnitEntity Defender)
    {
        int finalAttackDamage = Atker.m_iUnitAtk * AttackMag;
        Debug.Log("BaseAttack!" + finalAttackDamage);
        return finalAttackDamage;
    }
}
