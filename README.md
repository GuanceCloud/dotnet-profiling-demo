# dotnet-profiling-demo
One simple ASP.NET demo app for GuanceCloud continuous profiling

## Prerequisites
- .Net7.0

## Install .Net7.0

see page [https://dotnet.microsoft.com/zh-cn/download](https://dotnet.microsoft.com/zh-cn/download)

## Build

```shell
cd dotnet-profiling-demo
dotnet publish -c Release --self-contained true -o build
ls -l build
total 229248
-rwxr--r--   1 zy  staff   5549568 Jul  3 21:28 Datadog.Trace.dll
-rwxr--r--   1 zy  staff    114808 Jun 22 03:41 Microsoft.AspNetCore.Antiforgery.dll
-rwxr--r--   1 zy  staff     64672 Jun 22 03:41 Microsoft.AspNetCore.Authentication.Abstractions.dll
-rwxr--r--   1 zy  staff    116384 Jun 22 03:41 Microsoft.AspNetCore.Authentication.Cookies.dll
-rwxr--r--   1 zy  staff     88736 Jun 22 03:41 Microsoft.AspNetCore.Authentication.Core.dll
-rwxr--r--   1 zy  staff    105632 Jun 22 03:41 Microsoft.AspNetCore.Authentication.OAuth.dll
-rwxr--r--   1 zy  staff    163448 Jun 22 03:41 Microsoft.AspNetCore.Authentication.dll
-rwxr--r--   1 zy  staff     74872 Jun 22 03:41 Microsoft.AspNetCore.Authorization.Policy.dll
-rwxr--r--   1 zy  staff    107128 Jun 22 03:41 Microsoft.AspNetCore.Authorization.dll
...
```

## Run
```shell
./build/dotnet-profiling-demo
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://[::]:8080
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /home/zhangyi/project/dotnet-profiling-demo
```

程序启动后会监听 `8080` 端口，可以利用`curl`进行测试：
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


## Profiling

- On Linux (Arm not supported yet)

```shell
(DDTRACE_HOME="$(pwd)/build/datadog"; 
DD_DOTNET_TRACER_HOME="$DDTRACE_HOME" \
CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER="{846F5F1C-F9AE-4B07-969E-05C26BC060D8}" \
CORECLR_PROFILER_PATH="$DDTRACE_HOME/linux-x64/Datadog.Trace.ClrProfiler.Native.so" \
LD_PRELOAD="$DDTRACE_HOME/linux-x64/Datadog.Linux.ApiWrapper.x64.so" \
DD_PROFILING_ENABLED=1 \
DD_PROFILING_WALLTIME_ENABLED=1 \
DD_PROFILING_CPU_ENABLED=1 \
DD_PROFILING_EXCEPTION_ENABLED=1 \
DD_PROFILING_ALLOCATION_ENABLED=1 \
DD_PROFILING_LOCK_ENABLED=1 \
DD_PROFILING_HEAP_ENABLED=1 \
DD_PROFILING_GC_ENABLED=1 \
DD_SERVICE=dotnet-profiling-demo DD_ENV=testing DD_VERSION=1.2.3 \
DD_AGENT_HOST=127.0.0.1 DD_TRACE_AGENT_PORT=9529 \
build/dotnet-profiling-demo)
```

- On Windows IIS

Download and install https://github.com/DataDog/dd-trace-dotnet/releases/download/v2.34.0/datadog-dotnet-apm-2.34.0-x64.msi on your server.

### .Net Core

Edit your `web.config` at the web app root path, add `\<environmentVariables>\</environmentVariables>` node in `\<aspNetCore>\</aspNetCore>` node like below:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <location path="." inheritInChildApplications="false">
        <system.webServer>
            <handlers>
                <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
            </handlers>
            <aspNetCore processPath=".\dotnet-profiling-demo.exe" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" hostingModel="InProcess">
              <environmentVariables>
                <environmentVariable name="CORECLR_ENABLE_PROFILING" value="1" />
                <environmentVariable name="CORECLR_PROFILER" value="{846F5F1C-F9AE-4B07-969E-05C26BC060D8}" />
                <environmentVariable name="DD_PROFILING_ENABLED" value="1" />
                <environmentVariable name="DD_PROFILING_CPU_ENABLED" value="1" />
                <environmentVariable name="DD_PROFILING_WALLTIME_ENABLED" value="1" />
                <environmentVariable name="DD_PROFILING_ALLOCATION_ENABLED" value="1" />
                <environmentVariable name="DD_PROFILING_HEAP_ENABLED" value="1" />
                <environmentVariable name="DD_PROFILING_EXCEPTION_ENABLED" value="1" />
                <environmentVariable name="DD_PROFILING_LOCK_ENABLED" value="1" />
                <environmentVariable name="DD_PROFILING_GC_ENABLED" value="1" />
                <environmentVariable name="DD_ENV" value="production" />
                <environmentVariable name="DD_VERSION" value="1.2.3" />
                <environmentVariable name="DD_SERVICE" value="my-dotnet-core-app" />
                <environmentVariable name="DD_AGENT_HOST" value="127.0.0.1" />
                <environmentVariable name="DD_TRACE_AGENT_PORT" value="9529" />
              </environmentVariables>
            </aspNetCore>
        </system.webServer>
    </location>
</configuration>
```

Restart your IIS

```shell
net stop /y was
net start w3svc
```