using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackBehavior 
{
    //SO�� ���߻���� �����ϱ� ���� �������̽��Դϴ�.
    //��/�Ʊ��� �Ӽ��̳� ���� ���� �˾ƿ��� ���� UnitEntity ������/ ����ڸ� �Ű������� �޽��ϴ�.
    int ExecuteAttack(UnitEntity Atker, UnitEntity Defender);
}
