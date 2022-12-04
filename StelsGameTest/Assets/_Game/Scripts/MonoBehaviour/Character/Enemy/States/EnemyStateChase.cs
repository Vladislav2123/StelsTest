using UnityEngine;

public class EnemyStateChase : IState
{
    private Enemy _enemy;
    private float _distanceToPlayer;

    private Vector3 Position => _enemy.transform.position;
    private EnemyStateMachine StateMachine => _enemy.StateMachine;
    private Player Player => _enemy.Player;
    private Vector3 PlayerPosition => Player.transform.position;

    public EnemyStateChase(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Update()
    {
        if (Player == null)
        {
            Debug.LogWarning("Enemy State Attack: Target is null");
            StateMachine.ResetState();
            return;
        }

        _distanceToPlayer = Vector3.Distance(Position, PlayerPosition);

        TryMoveToTarget();
        TryAttackTarget();
    }

    private void TryMoveToTarget()
    {
        if (_enemy.NPC.Agent.hasPath == false) PavePathToAttackPoint();

        Vector3 destination = _enemy.NPC.Agent.destination;
        destination.y = Player.transform.position.y;
        float distanceFromDestination = Vector3.Distance(destination, PlayerPosition);

        if (distanceFromDestination > _enemy.CatchDistance) PavePathToAttackPoint();

        _enemy.NPC.TryFollowPath();
    }

    private void TryAttackTarget()
    {
        if (_distanceToPlayer > _enemy.CatchDistance) return;

        Player.Defeat();
    }

    private void PavePathToAttackPoint()
    {
        Vector3 fromTarget = (Position - PlayerPosition).normalized;
        Vector3 targetPoint = PlayerPosition + fromTarget * (_enemy.CatchDistance / 2);

        _enemy.NPC.Agent.SetDestination(targetPoint);
    }

    public void Enter() 
    {
        _enemy.Renderer.SetMaterial(_enemy.ChaseMaterial);
    }

    public void Exit() 
    {
        _enemy.Renderer.SetMaterial(_enemy.DefaultMaterial);
    }
}