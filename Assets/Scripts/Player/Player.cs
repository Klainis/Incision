using Incision.Enemies.Components;
using Incision.Events;
using Incision.Interfaces;
using Incision.UI.Components;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Incision
{
    public class Player : MonoBehaviour, ICreature
    {
        public string Name { get; private set; }
        public Transform Transform { get; }

        [Header("Templates")]
        [SerializeField] private PlayerMovementData _playerMovementData;
        [SerializeField] private PlayerStats _playerStatsData;

        [Header("Camera")]
        [SerializeField] private Camera _cameraMain;
        [SerializeField] private PlayerCamera _playerCamera;
        [SerializeField] private Camera _cameraUI;
        [SerializeField] private UICamera _uiCamera;

        [Header("Character")]
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private PlayerCharacter _character;
        [SerializeField] private PlayerAttack _attack;
        [SerializeField] private PlayerHealth _playerHealth;

        [Header("Abilities")]
        [SerializeField] private SlamAttack _slamAttack;
        [SerializeField] private DashStamina _dashStamina;
        [SerializeField] private CardHand _hand;
        [SerializeField] private CardInHandVisual _inHandVisual;

        [Header("Animation")]
        [SerializeField] private PlayerAnimation _animation;
        [SerializeField] private PlayerAnimationsMethods _animationsMethods;
        [SerializeField] private SteppedAnimations _steppedAnimations;
        [SerializeField] private AttackAnimationSpeed _attackAnimationSpeed;

        [Header("UI")]
        [SerializeField] private PlayerHealthUI _healthUI;
        [SerializeField] private StaminaBarsUI _standingBarsUI;
        [SerializeField] private CardHandHUD _handHUD;

        [SerializeField] private PlayerEffector _effector;

        [Space(5)]
        [SerializeField] private KinematicCharacterMotor _motor;

        public PlayerMovementData MovementData => _playerMovementData;
        public PlayerStats Stats => _playerStatsData;

        public PlayerCharacter PlayerCharacter => _character;
        public PlayerCamera PlayerCamera => _playerCamera;
        public PlayerAnimation Animation => _animation;
        public PlayerHealth PlayerHealth => _playerHealth;
        public PlayerEffector Effector => _effector;
        public PlayerAttack PlayerAttack => _attack;
        public CardHand Hand => _hand;
        public CardInHandVisual InHandVisual => _inHandVisual;

        [Inject] private IEventBus _event;

        private void Start() // to rewrite!!!!!
        {
            Initialize();
        }

        public void Initialize ()
        {
            Cursor.lockState = CursorLockMode.Locked;

            _playerInput.Intialize(_character, _playerCamera, _hand, _playerMovementData, _playerStatsData);
            _character.Initialize(_playerMovementData, _playerInput, _motor, _dashStamina, _effector, _animation);
            
            _slamAttack.Initialize(_character, _playerStatsData);
            _dashStamina.Initialize(_playerMovementData);

            _playerCamera.Initialize(_character.GetCameraTarget(), _playerMovementData, _cameraMain, _character);
            _uiCamera.Initialize(_cameraUI);

            _hand.Initialize(_attack, _animation);
            _inHandVisual.Initialize(_hand, _animation);

            _handHUD.Initialize(_hand);
            _playerHealth.InitializePlayerHealth(_playerStatsData, _healthUI);
            _playerHealth.OnDied += PlayerDie;

            _standingBarsUI.Initialize(_dashStamina);

            _animationsMethods.Initialize(_attack);
            _steppedAnimations.Initialize();
            _attackAnimationSpeed.Initialize(_hand);
        }

        private void OnDisable()
        {
            _playerHealth.OnDied -= PlayerDie;
        }

        private void PlayerDie(Health health)
        {
            Destroy(gameObject);
            _event.Publish(new OnGameFinish(false, "Player died"));
        }
    }
}
