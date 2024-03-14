using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� ���¸� �����ϴ� ������
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WiN, LOST }

public class BattleSystem : MonoBehaviour
{
    // �÷��̾�� ���� ������
    public GameObject[] playerPrefabs;
    public GameObject enemyPrefab;

    // �÷��̾�� ���� �����ϴ� ��ġ
    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    // ���� �÷��̾�� ���� ����
    Unit playerUnit;
    Unit enemyUnit;

    // ���� �� �߻��ϴ� ��ȭ�� ǥ���ϴ� UI �ؽ�Ʈ
    public Text dialogueText;

    // �÷��̾�� ���� HUD(Head-Up Display)�� �����ϴ� ��ü
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    // ���� ����
    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        // ���� ���� ���·� �ʱ�ȭ�ϰ�, ������ �����ϴ� �ڷ�ƾ ����
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    // ���� ������ ó���ϴ� �ڷ�ƾ
    IEnumerator SetupBattle()
    {
        // �÷��̾�� ���� ������ �����ϰ� ��ġ
        int randomPlayerIndex = Random.Range(0, playerPrefabs.Length);
        GameObject playerGO = Instantiate(playerPrefabs[randomPlayerIndex], playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        // ��ȭ �ؽ�Ʈ�� ���� �̸��� ǥ��
        dialogueText.text = "�߻��� " + enemyUnit.unitName + " ��(��) ��Ÿ����...";

        // �÷��̾�� ���� HUD�� ������Ʈ
        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        // ���� ���� �� ��� ���
        yield return new WaitForSeconds(2f);

        // �÷��̾� ������ ���� ��ȯ
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    // �÷��̾� ������ ó���ϴ� �ڷ�ƾ  --------------------------------------------------------------------------------------------------------
    IEnumerator PlayerAttack()
    {
        // ������ �������� ������ ��� �޾ƿ�
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        // ���� ü���� HUD�� ������Ʈ
        enemyHUD.SetHP(enemyUnit.currentHP);
        dialogueText.text = playerUnit.unitName + "�� ����!!";

        // ���� �ð� ���
        yield return new WaitForSeconds(1f);

        // ���� �׾����� Ȯ���ϰ� ���� ��ȯ
        if (isDead)
        {
            state = BattleState.WiN;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    // ���� ���� ó���ϴ� �ڷ�ƾ
    IEnumerator EnemyTurn()
    {
        // ���� �����ϰ� ��ȭ �ؽ�Ʈ ������Ʈ
        dialogueText.text = enemyUnit.unitName + " �� ����!";

        yield return new WaitForSeconds(1f);

        // �÷��̾ �������� �ް� ü�� ������Ʈ
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        // �÷��̾ �׾����� Ȯ���ϰ� ���� ��ȯ
        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    // ���� ���� ó��
    void EndBattle()
    {
        // ���� ����� ���� ��ȭ �ؽ�Ʈ ������Ʈ
        if (state == BattleState.WiN)
        {
            dialogueText.text = "�¸�!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = ".......����� ������ ����������.";
        }
    }

    // �÷��̾� �� ���� ó��   ----------------------------------------------------------------------------------------------------------------------
    void PlayerTurn()
    {
        dialogueText.text = playerUnit.unitName + "�� ��� �� �� �ΰ�..";
    }


    // ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnAttackButton()
    {
        // �÷��̾� ���� �ƴ� ��쿡�� �ƹ� �۾��� �������� ����
        if (state != BattleState.PLAYERTURN)
            return;

        // �÷��̾� ���� �ڷ�ƾ ����
        StartCoroutine(PlayerAttack());
    }

    // ȸ�� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnHealButton()
    {
        // �÷��̾� ���� �ƴ� ��쿡�� �ƹ� �۾��� �������� ����
        if (state != BattleState.PLAYERTURN)
            return;

        // �÷��̾� ȸ�� �ڷ�ƾ ����
        StartCoroutine(PlayerHeal());
    }


    // �÷��̾� ��ü ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnChangePlayerButton()
    {
        // �÷��̾� ���� �ƴ� ��쿡�� �ƹ� �۾��� �������� ����
        if (state != BattleState.PLAYERTURN)
            return;

        // �÷��̾� ��ü
        int randomPlayerIndex = Random.Range(0, playerPrefabs.Length);
        GameObject newPlayerGO = Instantiate(playerPrefabs[randomPlayerIndex], playerBattleStation);
        Destroy(playerUnit.gameObject); // ���� �÷��̾� ����
        playerUnit = newPlayerGO.GetComponent<Unit>();

        // �÷��̾��� ü���� HUD�� ������Ʈ
        playerHUD.SetHUD(playerUnit);

        // �� ������ ��ȯ
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }



    // �÷��̾� ȸ���� ó���ϴ� �ڷ�ƾ
    IEnumerator PlayerHeal()
    {
        // �÷��̾� ü�� ȸ��
        playerUnit.Heal(5);

        // �÷��̾��� ü���� HUD�� ������Ʈ�ϰ� ��ȭ �ؽ�Ʈ ǥ��
        playerHUD.SetHP(playerUnit.currentHP);
        dialogueText.text = "You feel renewed strength!";

        yield return new WaitForSeconds(2f);

        // ���� ������ ���� ��ȯ
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    

    
}
