using AppModels;

namespace WaterJugChallenge.Services
{
    public interface IWaterJugRiddleService
    {
        SolutionResponse SolveWaterJugProblem(int xCapacity, int yCapacity, int zAmountWanted);
    }
}
