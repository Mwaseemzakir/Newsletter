# GrpcDemo

# Case scenerio
This is a gRPC application where client is sending 10K requests to server each second , each request contains following items
- Id (Unique_
- UNIX Timestamp
- Number (Random from 1-1000)
  
Server should check this number and do following things
- Check is the number prime or not and respond accordingly
- If due to some issue exception occured then manage it accordingly

Additionaly display the following details
- Is number prime or not along with number
- Top 10 requested prime numbers
- Total requests
  
# Creating client and server in .NET 6.0
- STEP 1 : Create a gRPC client project in .NET 6 which will act like Server
- STEP 2 : Create a console based application in .NET 6 which will act like Client

# Nuget packages for client side
Install these packages for gRPC purposes
- Grpc.Tools
- Google.ProtoBuf
- Grpc.Net.Client

Following packages to enable appsettings in console application
- Microsoft.Extensions.Hosting
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.Configuration.Json

# For Background Jobs
I am using IBackgroundService


