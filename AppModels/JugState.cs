namespace AppModels
{
    // Represents the state of the jugs at a particular point in the process
    public class JugState
    {
        public int Jug1 { get; }
        public int Jug2 { get; }
        public JugState PreviousState { get; }
        public int StepNumber { get; }
        public string Action { get; }

        public JugState(int jug1, int jug2, JugState previousState, int stepNumber, string action = null)
        {
            Jug1 = jug1;
            Jug2 = jug2;
            PreviousState = previousState;
            StepNumber = stepNumber;
            Action = action;
        }
    }
}
