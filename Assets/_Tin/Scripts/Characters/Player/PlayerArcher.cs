using _Tin.Scripts.Characters.Player.StateMachine.Movement;
using _Tin.Scripts.Characters.Player.Utilities.Input;
using UnityEngine;

namespace _Tin.Scripts.Characters.Player
{
    [RequireComponent(typeof(PlayerArcherInputs))]
    public class PlayerArcher : MonoBehaviour
    {
        public Rigidbody Rigidbody { get; private set; }
        public Transform MainCameraTransform { get; private set; }
        public PlayerArcherInputs ArcherInput { get; private set; }
        // This will present the player's state machine
        private PlayerArcherStateMachine ArcherStateMachine { get; set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            
            if (Camera.main != null) 
                MainCameraTransform = Camera.main.transform;
            
            ArcherInput = gameObject.AddComponent<PlayerArcherInputs>();
            ArcherStateMachine = new PlayerArcherStateMachine(this);
        }

        // After the player state machine is created, we will change the state to the idling state by default
        private void Start() => ArcherStateMachine.ChangeState(ArcherStateMachine.ArcherIdlingState);

        private void Update()
        {
            ArcherStateMachine.HandleInput();
            ArcherStateMachine.Update();
        }

        private void FixedUpdate() => ArcherStateMachine.PhysicsUpdate();
    }
}