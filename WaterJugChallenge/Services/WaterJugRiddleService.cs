using AppModels;
using Microsoft.Extensions.Caching.Memory;

namespace WaterJugChallenge.Services
{
    public class WaterJugRiddleService : IWaterJugRiddleService
    {
        private readonly IMemoryCache _cache;

        public WaterJugRiddleService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public SolutionResponse SolveWaterJugProblem(int xCapacity, int yCapacity, int zAmountWanted)
        {
            var cacheKey = $"{xCapacity}-{yCapacity}-{zAmountWanted}"; // Unique cache key for the problem parameters

            // Check if result is already cached
            if (_cache.TryGetValue(cacheKey, out SolutionResponse cachedResult))
            {
                return cachedResult;
            }

            var steps = new List<Step>();
            var status = "Unsolved";

            // Step 1: Check if the problem is solvable
            if (zAmountWanted > xCapacity && zAmountWanted > yCapacity)
            {
                throw new AppException("No Solution");
            }

            if (zAmountWanted % GCD(xCapacity, yCapacity) != 0)
            {
                throw new AppException("No Solution");
            }

            // Step 2: Initialize BFS
            var visited = new HashSet<(int, int)>();
            var queue = new Queue<JugState>();
            queue.Enqueue(new JugState(0, 0, null, 0)); // Starting point: both jugs are empty, step 1

            while (queue.Count > 0)
            {
                var currentState = queue.Dequeue();

                // If we reach the goal, we can backtrack the steps
                if (currentState.Jug1 == zAmountWanted || currentState.Jug2 == zAmountWanted)
                {
                    BacktrackSteps(currentState, steps);
                    status = "Solved";

                    // Set "status" for the last step only
                    if (steps.Count > 0)
                    {
                        steps[steps.Count - 1].Status = status;
                    }

                    var response = new SolutionResponse
                    {
                        Solution = steps
                    };
                    _cache.Set(cacheKey, response, TimeSpan.FromMinutes(10));
                    return response;
                }

                // Mark this state as visited
                if (visited.Contains((currentState.Jug1, currentState.Jug2)))
                    continue;

                visited.Add((currentState.Jug1, currentState.Jug2));

                // Generate possible next states and enqueue them
                EnqueueNextStates(currentState, xCapacity, yCapacity, queue);
            }

            // If no solution is found, return empty steps with status "Unsolved"
            return new SolutionResponse
            {
                Solution = steps
            };
        }

        private void EnqueueNextStates(JugState currentState, int xCapacity, int yCapacity, Queue<JugState> queue)
        {
            // Fill Jug1
            queue.Enqueue(new JugState(xCapacity, currentState.Jug2, currentState, currentState.StepNumber + 1, "Fill bucket X"));
            // Fill Jug2
            queue.Enqueue(new JugState(currentState.Jug1, yCapacity, currentState, currentState.StepNumber + 1, "Fill bucket Y"));
            // Empty Jug1
            queue.Enqueue(new JugState(0, currentState.Jug2, currentState, currentState.StepNumber + 1, "Empty bucket X"));
            // Empty Jug2
            queue.Enqueue(new JugState(currentState.Jug1, 0, currentState, currentState.StepNumber + 1, "Empty bucket Y"));
            // Transfer from Jug1 to Jug2
            int transferToJug2 = Math.Min(currentState.Jug1, yCapacity - currentState.Jug2);
            queue.Enqueue(new JugState(currentState.Jug1 - transferToJug2, currentState.Jug2 + transferToJug2, currentState, currentState.StepNumber + 1, "Transfer from bucket X to Y"));
            // Transfer from Jug2 to Jug1
            int transferToJug1 = Math.Min(currentState.Jug2, xCapacity - currentState.Jug1);
            queue.Enqueue(new JugState(currentState.Jug1 + transferToJug1, currentState.Jug2 - transferToJug1, currentState, currentState.StepNumber + 1, "Transfer from bucket Y to X"));
        }

        private void BacktrackSteps(JugState state, List<Step> steps)
        {
            // Backtrack to generate the sequence of steps
            while (state.PreviousState != null)
            {
                steps.Add(new Step
                {
                    StepNumber = state.StepNumber,  // StepNumber instead of "step"
                    BucketX = state.Jug1,
                    BucketY = state.Jug2,
                    Action = state.Action
                });

                state = state.PreviousState;
            }
            steps.Reverse(); // Reverse the list to get the correct order of actions
        }

        private int GCD(int a, int b)
        {
            // Calculate the Greatest Common Divisor (GCD) of two numbers
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}
