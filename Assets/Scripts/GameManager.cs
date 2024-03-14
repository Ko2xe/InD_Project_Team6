using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Unit
{
    // ���� ���¸� ��Ÿ���� ������
    public enum State
    {
        start, playerTurn, enemyTurn, win
    }

    // ���� ���� ����
    public State state;

    // �� ���� ����
    public bool isLive;

    // ���� ���� �� ȣ��Ǵ� Awake �Լ�
    void Awake()
    {
        // ���� ���� �˸�
        state = State.start;
        BattleStart();
    }

    // ���� ���� �� ȣ��Ǵ� �Լ�
    void BattleStart()
    {
        // ���� ���� �� ĳ���� ���� �ִϸ��̼� �� ȿ�� �߰�
        // �÷��̾�� ������ �� �ѱ��
        state = State.playerTurn;
    }

    // �÷��̾� ���� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void PlayerAttackButton()
    {
        // �÷��̾� ���� �ƴ� ���� �Լ� ����
        if (state != State.playerTurn)
        {
            return;
        }

        // �÷��̾� ���� �ڷ�ƾ ����
        StartCoroutine(PlayerAttack());
    }

    // �÷��̾� ���� �ڷ�ƾ
    IEnumerator PlayerAttack()
    {
        // 1�� ���
        yield return new WaitForSeconds(1f);
        Debug.Log("�÷��̾� ����");

        // ���� ��ų, ������ ���� �ڵ� �ۼ�
        // ���� ������ ���� ����
        if (!isLive)
        {
            state = State.win;
            EndBattle();
        }
        // ���� ��������� ���� �������� ��ȯ
        else
        {
            state = State.enemyTurn;
            StartCoroutine(EnemyTurn());
        }
    }

    // ���� ���� �� ȣ��Ǵ� �Լ�
    void EndBattle()
    {
        Debug.Log("���� ����");
    }

    // ���� �� �ڷ�ƾ
    IEnumerator EnemyTurn()
    {
        // 1�� ���
        yield return new WaitForSeconds(1f);

        // �� ���� �ڵ� �ۼ�
        // �� ���� ���� �� �÷��̾� ������ ��ȯ
        state = State.playerTurn;
    }
}