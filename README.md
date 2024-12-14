# Water Jug Challenge API

This is a solution to the classic Water Jug Problem using a web API. The goal is to determine the steps required to measure a specific amount of water using two jugs with fixed capacities.

### API Endpoints
1. GET /api/waterjugriddle/{xCapacity}/{yCapacity}/{zAmountWanted}

This endpoint solves the Water Jug Problem using the specified jug capacities and the desired amount of water.
Parameters:

    xCapacity: Capacity of Jug 1.
    yCapacity: Capacity of Jug 2.
    zAmountWanted: The amount of water you want to measure.
    
Example Request:

_GET /api/waterjugriddle/3/5/4_

Example Response:


_{
  "solution":[
    {"step":1,"bucketX":0,"bucketY":5,"action":"Fill bucket Y"},
    {"step":2,"bucketX":3,"bucketY":2,"action":"Transfer from bucket Y to X"},
    {"step":3,"bucketX":0,"bucketY":2,"action":"Empty bucket X"},
    {"step":4,"bucketX":2,"bucketY":0,"action":"Transfer from bucket Y to X"},
    {"step":5,"bucketX":2,"bucketY":5,"action":"Fill bucket Y"},
    {"step":6,"bucketX":3,"bucketY":4,"action":"Transfer from bucket Y to X","status":"Solved"}
  ]
}_

2. POST /api/waterjugriddle

This endpoint accepts a JSON body with the jug capacities and desired water amount, and returns the solution.
Request Body:

_{
  "xCapacity": 4,
  "yCapacity": 20,
  "zAmountWanted": 12
}_

Example Response:

_{
    "solution": [
        {
            "step": 1,
            "bucketX": 0,
            "bucketY": 20,
            "action": "Fill bucket Y"
        },
        {
            "step": 2,
            "bucketX": 4,
            "bucketY": 16,
            "action": "Transfer from bucket Y to X"
        },
        {
            "step": 3,
            "bucketX": 0,
            "bucketY": 16,
            "action": "Empty bucket X"
        },
        {
            "step": 4,
            "bucketX": 4,
            "bucketY": 12,
            "action": "Transfer from bucket Y to X",
            "status": "Solved"
        }
    ]
}_

## How to Set Up and Run the Application
Prerequisites

    .NET 6.0 or higher
    Visual Studio or Visual Studio Code

Setup Steps

Clone the repository:
_git clone https://github.com/bartoloza/WaterJugChallenge_Task.git_

Navigate to the project directory:
_cd WaterJugChallenge_Task_

Restore dependencies:
_dotnet restore_

Build the application:
_dotnet build_

Run the application:
_dotnet run_

The application will run on https://localhost:7064.

## Algorithm Explanation

The algorithm uses a Breadth-First Search (BFS) approach to find the solution for the Water Jug Problem. Here’s how it works:

1. Initial Check: The problem is unsolvable if the desired amount of water (zAmountWanted) is greater than the capacities of both jugs (xCapacity and yCapacity) or if zAmountWanted is not a multiple of the greatest common divisor (GCD) of xCapacity and yCapacity.

2. BFS Implementation: Starting from the initial state (both jugs empty), the algorithm generates all possible states (filling, emptying, and transferring water between the jugs). It tracks visited states to avoid redundant calculations and keeps a queue to process states level by level.

3. Backtracking: Once a valid solution is found (i.e., one of the jugs contains the desired amount of water), the algorithm backtracks to generate the sequence of steps taken to reach the solution.

4. Caching: To improve performance, the results are cached for each unique set of parameters, reducing redundant calculations for repeated queries.

## How to Install and Run Performance Tests

To install and run performance tests, we will use k6, a popular load testing tool.
Installation:

    For Windows: Download the installer from the k6 website.

To run performance test:
    
    1. Navigate to desired project (in this case WaterJugChallange)
    2. Open in terminal
    3. k6 run performance-tests/waterjug-get-performance-test.js (make sure your API is running locally before starting tests)

![image](https://github.com/user-attachments/assets/e2cae407-4fbf-41e3-9881-83e0860150bc)





