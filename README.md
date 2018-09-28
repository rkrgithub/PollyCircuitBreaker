# PollyCircuitBreaker


https://github.com/App-vNext/Polly/wiki/Circuit-Breaker

Created a Web API svc, say DataSvc with three end points, one with an input parameter for delay, one without delay and one which return an exception
To test memory/CPU consumption, two logic added. (Commented for the time being)
1) Searching a file in D:/ drive and read the product data from that file
2) Reading 1 MB file and read product data from that file

Created another Web API svc CircuitBreakerPolly which also have three end points to consume the respective end points available in above DataSvc
Added Polly Circuit Breaker implementation in CircuitBreakerPolly .
              static CircuitBreakerPolicy _circuitBreaker = CircuitBreakerPolicy

                                                                                   .Handle<Exception>()

                                                                                   .CircuitBreakerAsync(

                     exceptionsAllowedBeforeBreaking: 1, this set the number of times exception allowed before going to OPEN state. It invoke onBreak() event when it go to OPEN state.

                     durationOfBreak: _durationOfBreak,  this is a breaking period which disallow any request during this period.

                     onBreak: (exception, timespan) =>

                               {

                                  LogMessageToFile("On Break, time span : " + timespan.ToString());

                               },

                    onReset: () =>

                               {

                                  LogMessageToFile("On Reset");

                               },

                    onHalfOpen: () =>

                               {

Once breaking period over, any further request will set the CB to HALF_OPEN state by calling onHalfOpen() event. Based on success-failure case, it change the state from OPEN to CLOSE or vice versa

                                        LogMessageToFile("on HalfOpen");

                               }

                          );

Writing the event details in a local log file in the same project folder. (Clientlog.log)

