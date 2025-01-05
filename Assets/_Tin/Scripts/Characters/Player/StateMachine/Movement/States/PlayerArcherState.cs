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
            
            var targetRotationYAngle = RotatePlayer(movementDirection);

            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
            
            var movementSpeed = GetMovementSpeed();
            
            var currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();
            
            archerStateMachine.PlayerArcher.Rigidbody.AddForce(
                targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }
        
        private float RotatePlayer(Vector3 direction)
        {
            var directionAngle = UpdateTargetRotation(direction);
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
        
        private void UpdateTargetRotationData(float targetAngle)
        {
            currentTargetRotation.y = targetAngle;
            dampedTargetRotationPassedTime.y = 0f;
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
            // Retrieve the current Y-axis rotation angle of the player's Rigidbody.
            var currentYAngle = archerStateMachine.PlayerArcher.Rigidbody.rotation.eulerAngles.y;
    
            // Check if the current Y-angle is approximately equal to the target Y-angle.
            // If they are approximately equal, exit the function early to avoid unnecessary calculations.
            if (Mathf.Approximately(currentYAngle, currentTargetRotation.y))
                return;
    
            // Calculate a smooth transition angle from the current Y-angle to the target Y-angle.
            // This uses a damping function to gradually change the angle over time.
            var smoothYAngle = Mathf.SmoothDampAngle(
                currentYAngle, // The current angle.
                currentTargetRotation.y, // The target angle to reach.
                // Reference to the current velocity, used internally by SmoothDampAngle.
                ref dampedTargetRotationCurrentVelocity.y, 
                // The time it should take to reach the target angle.
                timeToReachTargetRotation.y - dampedTargetRotationPassedTime.y 
            );
    
            // Increment the time passed since the rotation started by the time elapsed since the last frame.
            dampedTargetRotationPassedTime.y += Time.deltaTime;
    
            // Create a new Quaternion representing the target rotation with the smoothly calculated Y-angle.
            var targetRotation = Quaternion.Euler(0f, smoothYAngle, 0f);
    
            // Apply the calculated target rotation to the player's Rigidbody.
            archerStateMachine.PlayerArcher.Rigidbody.MoveRotation(targetRotation);
        }
        
        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
        {
            var directionAngle = GetDirectionAngle(direction);

            if (shouldConsiderCameraRotation) directionAngle = AddCameraToRotationAngle(directionAngle);

            if (Mathf.Approximately(directionAngle, currentTargetRotation.y))
                UpdateTargetRotationData(directionAngle);

            return directionAngle;
        }
        
        private static Vector3 GetTargetRotationDirection(float targetAngle) =>
            Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        private Vector3 GetMovementDirection() => new(MovementInput.x, 0f, MovementInput.y);
        private static float GetMovementSpeed() => BaseSpeed * SpeedModifier;
        #endregion
    }
}