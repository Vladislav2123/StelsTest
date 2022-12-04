using UnityEngine;
using Zenject;

[RequireComponent(typeof(NPC))]
[RequireComponent(typeof(EnemyStateMachine))]
public class Enemy : Character
{
    [Header("Walking")]
    [SerializeField] private Range _walkingDelay;
    [SerializeField] private float _walkingRadius;
    [Header("Chase")]
    [SerializeField] private float _cathDistance;
    [SerializeField] private Material _chaseMaterial;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Renderer _renderer;
    [Header("Detection")]
    [SerializeField] private float _detectionDistance;
    [SerializeField] private float _detectionAngle;
    [SerializeField] private LayerMask _obstaclesLayers;
    [SerializeField] private FieldOfView _fieldOfView;

    [Inject] private MapController _mapController;
    [Inject] private GameStateController _gameStateController;

    public NPC NPC { get; private set; }
    public EnemyStateMachine StateMachine { get; private set; }
    public Player Player => _mapController.Map.Player;

    // Walking
    public Range WalkingDelay => _walkingDelay;
    public float WalkingRadius => _walkingRadius;
    // Chase
    public float CatchDistance => _cathDistance;
    public Material DefaultMaterial => _defaultMaterial;
    public Material ChaseMaterial => _chaseMaterial;
    public Renderer Renderer => _renderer;

    protected override void Awake()
    {
        base.Awake();

        NPC = GetComponent<NPC>();
        StateMachine = GetComponent<EnemyStateMachine>();
    }

    private void OnEnable()
    {
        _mapController.OnGeneratedEvent += OnMapGenerated;
        _gameStateController.OnGameEndedEvent += StateMachine.ResetState;
        _gameStateController.OnGameStartedEvent += StateMachine.SetStateWalking;
    }
    private void OnDisable()
    {
        _mapController.OnGeneratedEvent -= OnMapGenerated;
        _gameStateController.OnGameEndedEvent -= StateMachine.ResetState;
        _gameStateController.OnGameStartedEvent -= StateMachine.SetStateWalking;
    }

    private void Start()
    {
        _fieldOfView.Fov = _detectionAngle;
        _fieldOfView.ViewDistance = _detectionDistance;
    }

    private void OnMapGenerated()
    {
        Player.OnMaxVolumeEvent += StateMachine.SetStateChase;
    }

    private void Update()
    {
        TryDetectPlayer();
    }

    private float _distanceToPlayer;
    private Vector3 _directionToPlayer;
    private float _angleToPlayer;
    private void TryDetectPlayer()
    {
        if (_gameStateController.IsPlaying == false) return;
        if (Player == null) return;
        if (StateMachine.CurrentState is EnemyStateChase) return;

        _distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);

        if (_distanceToPlayer > _detectionDistance) return;

        _directionToPlayer = (Player.transform.position - transform.position).normalized;
        _angleToPlayer = Vector3.Angle(_directionToPlayer, transform.forward);

        if (_angleToPlayer > _detectionAngle / 2) return;

        if (Physics.Linecast(transform.position + Vector3.up, Player.transform.position + Vector3.up, _obstaclesLayers)) return;

        StateMachine.SetStateChase();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, WalkingRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, CatchDistance);
    }
}