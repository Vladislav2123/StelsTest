using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPC : MonoBehaviour
{
    private Coroutine _runningFollowingRoutine;
    private Vector3 _velocity;

    public CharacterMovement Movement { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    protected virtual void Awake()
    {
        Movement = GetComponent<CharacterMovement>();
        Agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        if (Movement == null) Debug.LogWarning($"NPC {gameObject.name} has no Movement.");
        Agent.updatePosition = false;
        Agent.updateRotation = false;
    }

    public Vector3 FindRandomPoint(Vector3 sourcePosition, float radius)
    {
        Vector3 randomDirection = sourcePosition + (Random.insideUnitSphere * radius);
        NavMesh.SamplePosition(randomDirection, out NavMeshHit navMeshHit, radius, NavMesh.AllAreas);
        Debug.DrawLine(navMeshHit.position, navMeshHit.position + Vector3.up, Color.red, 10);
        return navMeshHit.position;
    }

    public void GoToPoint(Vector3 point)
    {
        if (Agent.hasPath) Stop();
        Agent.SetDestination(point);

        StartCoroutine(FollowingPathRoutine());
    }

    private IEnumerator FollowingPathRoutine()
    {
        while (Agent.hasPath)
        {
            if (TryFollowPath() == false) Stop();

            yield return null;
        }
    }

    float _distanceToDestination;
    public bool TryFollowPath()
    {
        if (Agent.hasPath == false) return false;
        if (Agent.path.corners.Length == 0) return false;

        _distanceToDestination = Vector3.Distance(transform.position, Agent.destination);
        if(_distanceToDestination <= Agent.stoppingDistance)
        {
            Agent.ResetPath();
            return false;
        }

        _velocity = Agent.desiredVelocity;
        return true;
    }

    public void Stop()
    {
        if (_runningFollowingRoutine != null) StopCoroutine(_runningFollowingRoutine);
        _runningFollowingRoutine = null;
        Agent.ResetPath();
    }

    protected virtual void Update()
    {
        if (Movement == null) return;

        if (_velocity != Vector3.zero) Movement.Move(_velocity);

        Agent.velocity = Movement.Velocity;
        _velocity = Vector3.zero;
    }

    public void MoveToPosition(Vector3 position)
    {
        RotateToPosition(position);

        Vector3 directionToPosition = GetDirectionToPosition(position);
        directionToPosition.y = 0;

        _velocity = directionToPosition.normalized;
    }

    public void RotateToPosition(Vector3 position)
    {
        Vector3 directionToPosition = GetDirectionToPosition(position);
        directionToPosition.y = 0;

        Movement.Rotate(directionToPosition);
    }

    public Vector3 GetDirectionToPosition(Vector3 targetPosition, float offset = 0)
    {
        return (targetPosition + Vector3.up * offset - transform.position + Vector3.up * offset).normalized;
    }

    public float GetAngleToDirection(Vector3 direction)
    {
        Quaternion rotationInDirection = Quaternion.LookRotation(direction);
        return Quaternion.Angle(transform.rotation, rotationInDirection);
    }

    public bool CheckPointAchievement(Vector3 point)
    {
        NavMeshPath testPath = new NavMeshPath();
        return NavMesh.CalculatePath(transform.position, point, NavMesh.AllAreas, testPath);
    }
}