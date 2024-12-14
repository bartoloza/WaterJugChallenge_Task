import http from 'k6/http';
import { check, sleep } from 'k6';

// Function to calculate the greatest common divisor (GCD) of two numbers
function gcd(a, b) {
    while (b !== 0) {
        let temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

// Function to generate random integer in a range [min, max]
function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

// Function to generate valid zAmountWanted given xCapacity and yCapacity
function generateValidZAmountWanted(xCapacity, yCapacity) {
    let greatestCommonDivisor = gcd(xCapacity, yCapacity);

    // zAmountWanted must be divisible by the GCD and less than or equal to the max capacity
    let maxCapacity = Math.max(xCapacity, yCapacity);
    let zAmountWanted = getRandomInt(1, maxCapacity);

    // Ensure that zAmountWanted is divisible by GCD
    while (zAmountWanted % greatestCommonDivisor !== 0) {
        zAmountWanted = getRandomInt(1, maxCapacity);
    }

    return zAmountWanted;
}

// Improved function to swap xCapacity and yCapacity randomly for more varied scenarios
function swapCapacity(x, y) {
    return Math.random() > 0.5 ? [x, y] : [y, x];
}

// Set up options with stages and custom thresholds
export let options = {
    stages: [
        { duration: '30s', target: 50 },  // Ramp-up to 50 VUs in 30 seconds
        { duration: '30s', target: 50 },  // Hold at 50 VUs for 30 seconds
        { duration: '30s', target: 100 }, // Ramp-up to 100 VUs in 30 seconds
        { duration: '30s', target: 100 }, // Hold at 100 VUs for 30 seconds
        { duration: '30s', target: 150 }, // Ramp-up to 150 VUs in 30 seconds
        { duration: '30s', target: 150 }, // Hold at 150 VUs for 30 seconds
        { duration: '30s', target: 0 },   // Ramp-down to 0 VUs over 30 seconds
    ],
    thresholds: {
        'http_req_duration': ['p(95)<500'],  // 95% of requests should complete in under 500ms
        'http_req_failed': ['rate<0.01'],   // Ensure less than 1% of requests fail
        'checks': ['rate>0.95'],             // Ensure 95% of checks pass
    },
};

// Main function for each virtual user
export default function () {
    // Generate random capacities (within a larger range)
    let xCapacity = getRandomInt(1, 20); // xCapacity range between 1 and 20
    let yCapacity = getRandomInt(1, 20); // yCapacity range between 1 and 20

    // Swap x and y randomly for additional test cases
    [xCapacity, yCapacity] = swapCapacity(xCapacity, yCapacity);

    // Generate a valid zAmountWanted that follows the rules
    let zAmountWanted = generateValidZAmountWanted(xCapacity, yCapacity);

    // Perform the GET request with the generated parameters
    let url = `https://localhost:7064/api/WaterJugRiddle/${xCapacity}/${yCapacity}/${zAmountWanted}`;
    let response = http.get(url);

    // Perform checks to verify the correctness of the response
    check(response, {
        'status is 200': (r) => r.status === 200,
        'response time is under 500ms': (r) => r.timings.duration < 500,
        'response contains valid data': (r) => r.body.includes('solution')  // Check if the body contains the word 'solution'
    });

    // Sleep to simulate real user thinking time
    sleep(1);
}
