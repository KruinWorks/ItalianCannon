# ItalianCannon

Download pre-compiled package: [![Download Directly](https://ci.appveyor.com/api/projects/status/rnt9jq5w7p0kdepa?svg=true)](https://ci.appveyor.com/api/projects/Elepover/ItalianCannon/artifacts/ItalianCannon.zip)

CI: https://ci.appveyor.com/project/Elepover/italiancannon

A simple multithreading web resource request tool. Based on .NET Core.

Supported OS: [See Microsoft Official](https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0-supported-os.md#net-core-20---supported-os-versions).

# Runtime

## Ordinary

*(Suitable for RHEL, CentOS, Oracle Linux, Fedora, Debian, Ubuntu, Linux Mint, openSUSE, SLES.)*

Configure .NET Core SDK.

Details could be found on [Microsoft Official](https://www.microsoft.com/net).

Get application source code: 

`git clone https://github.com/KruinWorks/ItalianCannon.git`

Enter project directory: 

`cd ./ItalianCannon/ItalianCannon`

Just run `dotnet run` directly and... There you go!

## Arch Linux Special

Directly install the `dotnet-runtime-2.0` package in `community` repository.

`pacman -Syu dotnet-runtime-2.0`

Get the pre-compiled binary file, then run `dotnet <filename>` and it's done.

## Notice

On the first use, it will ask you to edit the default configurations.

When you're done, press Enter to continue.

Details could be found below.

# Configurations

```json
{
  "Note": "Please change 'AppearsToBeDefault' to False after changing settings. ItalianCannon will ignore this configuration entry. For headers help, see https://github.com/dotnet/corefx/blob/master/src/System.Net.WebHeaderCollection/src/System/Net/HttpRequestHeader.cs",
  "TeaCupTarget": "https://www.baidu.com",
  "Threads": 1,
  "IntervalPerThread": 500,
  "MaxRequestsPerThread": 1000,
  "UserAgent": "Mozilla/5.0 (Linux) AppleWebKit/888.88 (KHTML, like Gecko) Chrome/66.6.2333.66 Safari/233.33",
  "AppearsToBeDefault": true,
  "DisableSSLValidation": false,
  "IgnoreHTTPError": false,
  "ExtraHTTPHeaders": [
    {
      "HType": 10,
      "Content": "GET"
    }
  ],
  "EnableAnimations": false,
  "EnableColors": true,
  "VerboseMode": false
}
```

| Name | Description |
| :----- | -----: |
| `Note` | The description. Doesn't have any use. |
| `TeaCupTarget` | Request target. |
| `Threads` | Quantity of threads. |
| `IntervalPerThread` | Request interval of each thread. |
| `MaxRequestsPerThread` | Maximum request count of each thread. |
| `UserAgent` | User agent. |
| `AppearsToBeDefault` | MUST be edited to false. |
| `DisableSSLValidation` | Disables the SSL Certificate Validation. |
| `IgnoreHTTPError` | Ignore HTTP 4xx/5xx errors. |
| `ExtraHTTPHeaders` | Extra HTTP Headers sent along with requests. |
| `EnableAnimations` | Enable animations to get a direct view of status and statistics. |
| `EnableColors` | Enables colored outputs. |
| `VerboseMode` | Disable most info outputs. |

*Note: For helps of ExtraHTTPHeaders option, refer to default configurations and you can find HType Codes in [Microsoft Repository](https://github.com/dotnet/corefx/blob/master/src/System.Net.WebHeaderCollection/src/System/Net/HttpRequestHeader.cs).*