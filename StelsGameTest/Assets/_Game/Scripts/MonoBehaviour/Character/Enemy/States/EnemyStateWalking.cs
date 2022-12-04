using UnityEngine;
using System.Collections;

public class EnemyStateWalking : IState
{
    private Enemy _enemy;
    private Coroutine _playingRoutine;
    private bool _isWaiting;

    public EnemyStateWalking(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.NPC.Stop();
    }

    public void Update()
    {
        if (_enemy.NPC.Agent.hasPath == false)
        {
            if (_isWaiting == false) _playingRoutine = _enemy.StartCoroutine(GoToNewPointRoutine());
        }
        else _enemy.NPC.TryFollowPath();
    }

    public void Exit()
    {
        if (_playingRoutine != null) _enemy.StopCoroutine(_playingRoutine);
        _enemy.NPC.Stop();
    }

    private IEnumerator GoToNewPointRoutine()
    {
        _isWaiting = true;

        yield return new WaitForSeconds(_enemy.WalkingDelay.RandomValue);

        Vector3 newPoint = Vector3.zero;
        bool isAchievablePointFound = false;
        while (isAchievablePointFound == false)
        {
            newPoint = _enemy.NPC.FindRandomPoint(_enemy.transform.position, _enemy.WalkingRadius);
            if (_enemy.NPC.CheckPointAchievement(newPoint)) isAchievablePointFound = true;
        }

        Debug.Log("Enemy go to point");
        _enemy.NPC.Agent.SetDestination(newPoint);
        _isWaiting = false;
    }
}
