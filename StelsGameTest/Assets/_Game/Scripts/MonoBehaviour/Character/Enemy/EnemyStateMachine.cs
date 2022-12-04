using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    private Enemy _enemy;

    protected override void Awake()
    {
        _enemy = GetComponent<Enemy>();

        base.Awake();
    }

    protected override void InitializeStates()
    {
        States[typeof(EnemyStateWalking)] = new EnemyStateWalking(_enemy);
        States[typeof(EnemyStateChase)] = new EnemyStateChase(_enemy);
    }

    public void SetStateWalking()
    {
        SetState(GetState<EnemyStateWalking>());
    }

    public void SetStateChase()
    {
        SetState(GetState<EnemyStateChase>());
    }
}