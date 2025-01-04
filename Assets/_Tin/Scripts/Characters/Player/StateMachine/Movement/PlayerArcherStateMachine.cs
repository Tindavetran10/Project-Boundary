using _Tin.Scripts.Characters.Player.StateMachine.Movement.States.Grounded;
using _Tin.Scripts.Characters.Player.StateMachine.Movement.States.Grounded.Moving;

namespace _Tin.Scripts.Characters.Player.StateMachine.Movement
{
    // Inherits from Tri's StateMachine
    public class PlayerArcherStateMachine : global::StateMachine
    {
        public PlayerArcher PlayerArcher { get; private set; }
        // This will present all the states of the player archer
        public PlayerArcherIdlingState ArcherIdlingState { get; private set; }
        public PlayerArcherRunningState ArcherRunningState { get; private set; }
        public PlayerArcherWalkingState ArcherWalkingState { get; private set; }
        public PlayerArcherSprintingState ArcherSprintingState { get; private set; }
        
        // Constructor of the PlayerArcherStateMachine
        public PlayerArcherStateMachine(PlayerArcher playerArcher)
        {
            PlayerArcher = playerArcher;
            // Initialize all the states
            ArcherIdlingState = new PlayerArcherIdlingState(this);
            ArcherRunningState = new PlayerArcherRunningState(this);
            ArcherWalkingState = new PlayerArcherWalkingState(this);
            ArcherSprintingState = new PlayerArcherSprintingState(this);
        }
    }
}