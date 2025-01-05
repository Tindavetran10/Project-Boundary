using UnityEngine;

namespace _Tin.Scripts.Characters.Player.StateMachine.Movement.States
{
    public class PlayerArcherState : IState
    {
        private readonly PlayerArcherStateMachine archerStateMachine;
        private Vector2 MovementInput { get; set; }
        private const float BaseSpeed = 5f;
        private const float SpeedModifier = 1f;
        
        protected Vector3 currentTargetRotation;
        protected Vector3 timeToReachTargetRotation;
        protected Vector3 dampedTargetRotationCurrentVelocity;
        protected Vector3 dampedTargetRotationPassedTime;

        protected PlayerArcherState(PlayerArcherStateMachine archerStateMachine)
        {
            this.archerStateMachine = archerStateMachine;
            InitializeData();
        }

        private void InitializeData()
        {
            currentTargetRotation = Vector3.zero;
            timeToReachTargetRotation.y = 0.14f;
            dampedTargetRotationCurrentVelocity = Vector3.zero;
            dampedTargetRotationPassedTime = Vector3.zero;
        }

        #region IState Methods
        public virtual void Enter() => Debug.Log("PlayerArcherState Enter: " + GetType().Name);
        public virtual void Exit(){}
        public virtual void HandleInput() => ReadMovementInput();
        public virtual void Update(){}
        public virtual void PhysicsUpdate() => AddForceToPlayer();
        public virtual void OnAnimationEnterEvent(){}
        public virtual void OnAnimationExitEvent(){}
        public virtual void OnAnimationTransitionEvent(){}
        public virtual void OnTriggerEnter(Collider collider){}
        public virtual void OnTriggerExit(Collider collider){}
        #endregion

        #region Main Methods
        private void ReadMovementInput() => 
            MovementInput = archerStateMachine.PlayerArcher.ArcherInput.PlayerArcherActions.Movement.ReadValue<Vector2>();

        private void AddForceToPlayer() //Move
        {
            if (MovementInput == Vector2.zero || SpeedModifier == 0f)
                return;
            
            var movementDirection = GetMovementDirection();
            var movementSpeed = GetMovementSpeed();
            
            var currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
            
            archerStateMachine.PlayerArcher.Rigidbody.AddForce(
                movementDirection * (5F * movementSpeed) - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }

        private float RotatePlayer(Vector3 direction)
        {
            var directionAngle = GetDirectionAngle(direction);

            directionAngle = AddCameraToRotationAngle(directionAngle);
            
            RotateTowardsTargetRotation();

            return directionAngle;
        }
        
        private static float GetDirectionAngle(Vector3 direction)
        {
            var directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if(directionAngle < 0)
                directionAngle += 360f;
            return directionAngle;
        }

        private float AddCameraToRotationAngle(float angle)
        {
            angle += archerStateMachine.PlayerArcher.MainCameraTransform.eulerAngles.y;

            if(angle > 360f)
                angle -= 360f;
            return angle;
        }
        #endregion

        #region Reusable Methods
        private Vector3 GetPlayerHorizontalVelocity()
        {
            var playerHorizontalVelocity = archerStateMachine.PlayerArcher.Rigidbody.velocity;
            playerHorizontalVelocity.y = 0f;

            return playerHorizontalVelocity;
        }
        
        private void RotateTowardsTargetRotation()
        {
            float currentYAngle = archerStateMachine.PlayerArcher.Rigidbody.rotation.eulerAngles.y;
            
            if (Mathf.Approximately(currentYAngle, currentTargetRotation.y))
                return;
            
            float smoothYAngle = Mathf.SmoothDampAngle(currentYAngle, currentTargetRotation.y, 
                ref dampedTargetRotationCurrentVelocity.y, timeToReachTargetRotation.y);
            
        }
        private Vector3 GetMovementDirection() => new(MovementInput.x, 0f, MovementInput.y);
        private static float GetMovementSpeed() => BaseSpeed * SpeedModifier;
        #endregion
    }
}