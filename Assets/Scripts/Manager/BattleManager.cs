using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� ���¸� �����ϴ� ������
public enum BattleState { START, ACTION, PLAYERTURN, PROCESS, ENEMYTURN, RESULT, END }


public class BattleManager : MonoBehaviour
{
    // �÷��̾�� ���� ������
    public GameObject[] playerPrefabs;
    public GameObject enemyPrefab;

    // �÷��̾�� ���� �����ϴ� ��ġ
    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    public Transform waitStation;

    private Coroutine BattleCoroutine;
    private bool isPlayed = false;
    private GameManager.Action m_ePlayerAction;
    private int m_iPlayerActionIndex;

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
        BattleCoroutine = StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        //Debug.Log(state);
    }


    #region ���� ���� �޼���
    void BattleInit()
    {
        //Instantiate�� �ؼ� �����ϴ� ������ prefab�� 1���� ����ϴٺ��� prefab�� �����ϸ� �������ɷ� �� instantiate �Ǳ� �����Դϴ�.
        playerPrefabs = new GameObject[2];
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





    // �÷��̾� �� ���� ó��   ----------------------------------------------------------------------------------------------------------------------
    private void PlayerAction()
    {
        state = BattleState.ACTION;
        dialogueText.text = playerUnit.m_sUnitName + "�� ��� �� �� �ΰ�..";
    }

    private void Process()
    {
        if (m_ePlayerAction == GameManager.Action.ATTACK)
            AttackProcess();
        else if (m_ePlayerAction == GameManager.Action.ITEM)
            ItemProcess();
        else if (m_ePlayerAction == GameManager.Action.CHANGE)
            ChangeProcess();
        else if (m_ePlayerAction == GameManager.Action.RUN)
            RunProcess();
    }
    

    private void AttackProcess()
    {
        if (state != BattleState.ENEMYTURN && state != BattleState.PLAYERTURN)
        {
            if (playerUnit.m_iUnitSpeed > enemyUnit.m_iUnitSpeed)
                BattleCoroutine = StartCoroutine(PlayerTurn());
            else if (playerUnit.m_iUnitSpeed < enemyUnit.m_iUnitSpeed)
                BattleCoroutine = StartCoroutine(EnemyTurn());
            else
            {
                if (playerUnit.m_iUnitLevel < enemyUnit.m_iUnitLevel)
                    StartCoroutine(EnemyTurn());
                else
                    StartCoroutine(PlayerTurn());
            }
        }
        else if (state == BattleState.ENEMYTURN)
            StartCoroutine(PlayerTurn());
        else if (state == BattleState.PLAYERTURN)
            StartCoroutine(EnemyTurn());
    }
    private void ItemProcess()
    {
        Debug.Log("������ ���");
    }
    private void ChangeProcess()
    {
        Debug.Log("������ ����");
    }
    private void RunProcess()
    {
        Debug.Log("����");
    }



    // ���� ���� ó��
    void AfterWin()
    {
        state = BattleState.END;
        dialogueText.text = "�¸�!";
    }

    void AfterLost()
    {
        state = BattleState.END;
        dialogueText.text = ".......����� ������ ����������.";
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
        PlayerAction();
    }

    IEnumerator Result()
    {
        dialogueText.text = "�� ���� �Ϸ�..";
        yield return new WaitForSeconds(1f);
        if (playerUnit.m_iCurrentHP <= 0)
            AfterLost();
        else if (enemyUnit.m_iCurrentHP <= 0)
            AfterWin();
        else
        {
            state = BattleState.ACTION;
            isPlayed = false;
            PlayerAction();
        }

    }


    // �÷��̾� ������ ó���ϴ� �ڷ�ƾ  --------------------------------------------------------------------------------------------------------
    IEnumerator PlayerTurn()
    {
        if (m_ePlayerAction == GameManager.Action.ATTACK)
        {
            state = BattleState.PLAYERTURN;
            //���� ����
            playerUnit.AttackByIndex(playerUnit, enemyUnit, m_iPlayerActionIndex);
            enemyHUD.SetHP(enemyUnit.m_iCurrentHP);
            dialogueText.text = playerUnit.m_sUnitName + "�� ����!!";
            yield return new WaitForSeconds(1f);
            if (enemyUnit.m_iCurrentHP <= 0)
                BattleCoroutine = StartCoroutine(Result());
            else if (!isPlayed)
                Process();
            else
                BattleCoroutine = StartCoroutine(Result());
            isPlayed = true;
        }
    }

    // ���� ���� ó���ϴ� �ڷ�ƾ
    IEnumerator EnemyTurn()
    {
        state = BattleState.ENEMYTURN;
        // ���� �����ϰ� ��ȭ �ؽ�Ʈ ������Ʈ
        enemyUnit.Attack(enemyUnit, playerUnit);
        playerHUD.SetHP(playerUnit.m_iCurrentHP);
        dialogueText.text = enemyUnit.m_sUnitName + " �� ����!";


        // �÷��̾ �������� �ް� ü�� ������Ʈ

        yield return new WaitForSeconds(1f);
        if (enemyUnit.m_iCurrentHP <= 0)
            BattleCoroutine = StartCoroutine(Result());
        else if (!isPlayed)
            Process();
        else
            BattleCoroutine = StartCoroutine(Result());
        isPlayed = true;

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
        BattleCoroutine = StartCoroutine(EnemyTurn());
    }

    #endregion

    #region ��ư Ŭ�� �̺�Ʈ
    // ���� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnButton(GameManager.Action action, int index)
    {
        // �÷��̾� ���� �ƴ� ��쿡�� �ƹ� �۾��� �������� ����
        if (state != BattleState.ACTION)
            return;
        m_ePlayerAction = action;
        m_iPlayerActionIndex = index;


        state = BattleState.PROCESS;

        // �÷��̾� ���� �ڷ�ƾ ����
        Process();
    }

    // ȸ�� ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnHealButton()
    {
        // �÷��̾� ���� �ƴ� ��쿡�� �ƹ� �۾��� �������� ����
        if (state != BattleState.ACTION)
            return;

        // �÷��̾� ȸ�� �ڷ�ƾ ����
        BattleCoroutine = StartCoroutine(PlayerHeal());
    }


    // �÷��̾� ��ü ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnChangePlayerButton()
    {
        // �÷��̾� ���� �ƴ� ��쿡�� �ƹ� �۾��� �������� ����
        if (state != BattleState.ACTION)
            return;

        // �÷��̾� ��ü
        int randomPlayerIndex = Random.Range(0, playerPrefabs.Length);
        GameObject newPlayerGO = playerPrefabs[randomPlayerIndex];
        playerUnit.transform.position = waitStation.position; // ���� �÷��̾� �̵�
        newPlayerGO.transform.position = playerBattleStation.position;
        playerUnit = newPlayerGO.GetComponent<UnitEntity>();


        // �÷��̾��� ü���� HUD�� ������Ʈ
        playerHUD.SetHUD(playerUnit);

        // �� ������ ��ȯ
        state = BattleState.ENEMYTURN;
        BattleCoroutine = StartCoroutine(EnemyTurn());
    }

    #endregion
}