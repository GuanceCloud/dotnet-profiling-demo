# dotnet-profiling-demo
One simple ASP.NET demo app for GuanceCloud continuous profiling

## Prerequisites
- .Net7.0

## Install .Net7.0

see page [https://dotnet.microsoft.com/zh-cn/download](https://dotnet.microsoft.com/zh-cn/download)

## Build

```shell
cd dotnet-profiling-demo
dotnet publish -o build
ls -l build
```

## Run

```shell
DD_DOTNET_TRACER_HOME="$(pwd)/build/datadog" \
CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8} \
CORECLR_PROFILER_PATH="$DD_DOTNET_TRACER_HOME/linux-x64/Datadog.Trace.ClrProfiler.Native.so" \
DD_PROFILING_ENABLED=1 \
DD_PROFILING_WALLTIME_ENABLED=1 \
DD_PROFILING_CPU_ENABLED=1 \
DD_PROFILING_EXCEPTION_ENABLED=1 \
DD_PROFILING_ALLOCATION_ENABLED=1 \
DD_PROFILING_LOCK_ENABLED=1 \
DD_PROFILING_HEAP_ENABLED=1 \
DD_PROFILING_GC_ENABLED=1 \
LD_PRELOAD="$DD_DOTNET_TRACER_HOME/linux-x64/Datadog.Linux.ApiWrapper.x64.so" \
DD_SERVICE=dotnet-profiling-demo DD_ENV=testing DD_VERSION=1.2.3 DD_AGENT_HOST=127.0.0.1 DD_TRACE_AGENT_PORT=9529 \
build/dotnet-profiling-demo
```

程序启动后会监听 `8080` 端口：
```shell
curl 'http://127.0.0.1:8080/movies?q=batman'
```

```json
[
  {
    "title": "Batman Begins",
    "vote_average": 4.6358799822491275,
    "release_date": "2005-05-31"
  },
  {
    "title": "Batman: Mystery of the Batwoman",
    "vote_average": 3.9549411967914105,
    "release_date": "2003-10-21"
  },
  {
    "title": "Batman Beyond: Return of the Joker",
    "vote_average": 1.8787282761678148,
    "release_date": "2000-10-31"
  },
 ...
]

```