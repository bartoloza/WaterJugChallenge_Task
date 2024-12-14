namespace AppModels
{
    // Represents the state of the jugs at a particular point in the process
    public record JugState(int Jug1, int Jug2, JugState? PreviousState, int StepNumber, string? Action = null);
}

