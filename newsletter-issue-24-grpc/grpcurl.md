# Testing the gRPC service

This issue is a **gRPC** service (HTTP/2 + Protocol Buffers), not a REST/JSON API, so plain
cURL and `.http` files don't apply. Use [`grpcurl`](https://github.com/fullstorydev/grpcurl)
(or a gRPC GUI such as Postman's gRPC request, BloomRPC, or Kreya) instead.

Server address: `localhost:5206` (HTTP/2) / `https://localhost:7209`. Adjust to match
`GprcServer/Properties/launchSettings.json`. The `GrpcClient` console app is the intended
client and already streams requests to the server.

### List the services exposed by the server (requires server reflection)
```bash
grpcurl -plaintext localhost:5206 list
```

### Describe a service
```bash
grpcurl -plaintext localhost:5206 describe
```

### Call a method using the .proto directly (no reflection needed)
```bash
grpcurl -plaintext -proto GprcServer/Protos/<your-service>.proto \
  -d '{ "id": "1", "timestamp": 0, "number": 7 }' \
  localhost:5206 <package>.<Service>/<Method>
```

> Postman: **New → gRPC Request**, enter `localhost:5206`, import the `.proto` from
> `GprcServer/Protos`, pick the method, and send.
