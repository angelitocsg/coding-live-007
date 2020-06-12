# Memory Cache and Distributed Cache | Coding Live #007

## Getting Started

These instructions is a part of a live coding video.

### Prerequisites

- .NET Core 3.1 SDK - https://dotnet.microsoft.com/download
- Redis server - https://redis.io

## Projects

Create a base folder `CodingLive007`.

Create the `.gitignore` file based on file https://github.com/github/gitignore/blob/master/VisualStudio.gitignore

### Example Cached API

```shell
dotnet new webapi --name CachedAPI
dotnet add package Bogus
dotnet add package Microsoft.Extensions.Caching.Redis
```

```shell
docker volume create redis-data
docker run --name live-redis -v redis-data:/data -p 6379:6379 -d redis redis-server --appendonly yes
```

### Redis Commander

```shell
npm install -g redis-commander
redis-commander --redis-host docker.local
```

## References

https://redis.io/

https://www.npmjs.com/package/redis-commander

https://aws.amazon.com/pt/elasticache/what-is-redis/

https://stackexchange.github.io/StackExchange.Redis/
