using Incision.Interfaces;
using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incision.PLayer
{
    public class Player : MonoBehaviour, ICreature
    {
        public string Name { get; private set; }

        [Header("Templates")]
        [SerializeField] private PlayerMovementData _playerMovementData;
        [SerializeField] private PlayerStats _playerStatsData;

        [Header("Camera")]
        [SerializeField] private Camera _camera;
        [SerializeField] private PlayerCamera _playerCamera;

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

        [Header("UI")]
        [SerializeField] private PlayerHealthUI _healthUI;
        [SerializeField] private StaminaBarsUI _standingBarsUI;

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

        private void Start() // to rewrite!!!!!
        {
            Initialize();
        }

        public void Initialize ()
        {
            Cursor.lockState = CursorLockMode.Locked;

            _playerInput.Intialize(_character, _playerCamera, _hand, _playerMovementData, _playerStatsData);
            _character.Initialize(_playerMovementData, _playerInput, _motor, _dashStamina, _effector, _animation);
            _playerCamera.Initialize(_character.GetCameraTarget(), _playerMovementData, _camera, _character);
            _slamAttack.Initialize(_character, _playerStatsData);
            _dashStamina.Initialize(_playerMovementData);

            _hand.Initialize(_attack, _animation);

            _playerHealth.InitializePlayerHealth(_playerStatsData, _healthUI);
            _standingBarsUI.Initialize(_dashStamina);

            _animationsMethods.Initialize(_attack);
            _steppedAnimations.Initialize();
        }
    }
}
