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
    public Transform waitStation;

    // ���� �÷��̾�� ���� ������ ��ũ��Ʈ
    UnitEntity playerUnit;
    UnitEntity enemyUnit;

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
        BattleInit();
        state = BattleState.START;
        // ���� ���� ���·� �ʱ�ȭ�ϰ�, ������ �����ϴ� �ڷ�ƾ ����
        StartCoroutine(SetupBattle());
    }


    #region ���� ���� �޼���
    void BattleInit()
    {
        //Instantiate�� �ؼ� �����ϴ� ������ prefab�� 1���� ����ϴٺ��� prefab�� �����ϸ� �������ɷ� �� instantiate �Ǳ� �����Դϴ�.
        enemyPrefab = Resources.Load<GameObject>("Prefabs/UnitEntity");
        enemyPrefab.GetComponent<UnitEntity>().m_sUnitName = "��������";
        enemyPrefab = Instantiate(enemyPrefab, enemyBattleStation);

        playerPrefabs[0] = Resources.Load<GameObject>("Prefabs/UnitEntity");
        playerPrefabs[0].GetComponent<UnitEntity>().m_sUnitName = "��������";
        playerPrefabs[0] = Instantiate(playerPrefabs[0], waitStation);

        playerPrefabs[1] = Resources.Load<GameObject>("Prefabs/UnitEntity");
        playerPrefabs[1].GetComponent<UnitEntity>().m_sUnitName = "��������";
        playerPrefabs[1] = Instantiate(playerPrefabs[1], waitStation);

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
        dialogueText.text = playerUnit.m_sUnitName + "�� ��� �� �� �ΰ�..";
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
        playerUnit = newPlayerGO.GetComponent<UnitEntity>();

        // �÷��̾��� ü���� HUD�� ������Ʈ
        playerHUD.SetHUD(playerUnit);

        // �� ������ ��ȯ
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    #endregion

    #region ���� ���� �ڷ�ƾ
    // ���� ������ ó���ϴ� �ڷ�ƾ
    IEnumerator SetupBattle()
    {
        // �÷��̾�� ���� ������ �����ϰ� ��ġ
        int randomPlayerIndex = Random.Range(0, playerPrefabs.Length);
        playerUnit = playerPrefabs[randomPlayerIndex].GetComponent<UnitEntity>();

        enemyUnit = enemyPrefab.GetComponent<UnitEntity>();

        playerUnit.transform.position = playerBattleStation.position;

        // ��ȭ �ؽ�Ʈ�� ���� �̸��� ǥ��
        dialogueText.text = "�߻��� " + enemyUnit.m_sUnitName + " ��(��) ��Ÿ����...";

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
        bool isDead = enemyUnit.TakeDamage(playerUnit.Attack(playerUnit,enemyUnit));

        // ���� ü���� HUD�� ������Ʈ
        enemyHUD.SetHP(enemyUnit.m_iCurrentHP);
        dialogueText.text = playerUnit.m_sUnitName + "�� ����!!";

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
        dialogueText.text = enemyUnit.m_sUnitName + " �� ����!";

        yield return new WaitForSeconds(1f);

        // �÷��̾ �������� �ް� ü�� ������Ʈ
        bool isDead = playerUnit.TakeDamage(enemyUnit.Attack(enemyUnit,playerUnit));
        playerHUD.SetHP(playerUnit.m_iCurrentHP);

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
    // �÷��̾� ȸ���� ó���ϴ� �ڷ�ƾ
    IEnumerator PlayerHeal()
    {
        // �÷��̾� ü�� ȸ��
        playerUnit.Heal(5);

        // �÷��̾��� ü���� HUD�� ������Ʈ�ϰ� ��ȭ �ؽ�Ʈ ǥ��
        playerHUD.SetHP(playerUnit.m_iCurrentHP);
        dialogueText.text = "You feel renewed strength!";

        yield return new WaitForSeconds(2f);

        // ���� ������ ���� ��ȯ
        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    #endregion
}
