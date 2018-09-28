# PollyCircuitBreaker


https://github.com/App-vNext/Polly/wiki/Circuit-Breaker

Created a Web API svc, say DataSvc with three end points, one with an input parameter for delay, one without delay and one which return an exception.

To test memory/CPU consumption, two logic added. (Commented for the time being)
  1) Searching a file in D:/ drive and read the product data from that file
  2) Reading 1 MB file and read product data from that file

Created another Web API svc CircuitBreakerPolly which also have three end points to consume the respective end points available in above DataSvc.

Added Polly Circuit Breaker implementation in CircuitBreakerPolly.

Writing the event details in a local log file in the same project folder. (Clientlog.log)

