using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SOAttackBase : ScriptableObject , IAttackBehavior
{
    //������ ������ �������̽��� ScriptableObject�� ��ӹ��� SOBase �����Դϴ�.
    public abstract int ExecuteAttack(UnitEntity Atker, UnitEntity Defender);
}
