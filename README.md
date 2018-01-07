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

```
{
  "Note": "Please change 'AppearsToBeDefault' to False after changing settings. ItalianCannon will ignore this configuration entry.",
  "TeaCupTarget": "https://www.baidu.com",
  "Threads": 1,
  "IntervalPerThread": 500,
  "MaxRequestsPerThread": 1000,
  "UserAgent": "Mozilla/5.0 (Linux) AppleWebKit/888.88 (KHTML, like Gecko) Chrome/66.6.2333.66 Safari/233.33",
  "AppearsToBeDefault": true
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

# CLI options

```
  -v           Silent mode, outputs nothing.
                 *-a, --help arguments will ignore this option.
  -c           Ignore per-thread request limit.
                 *Equals to setting 'MaxRequestsPerThread = 0'
  -g           Disable colorized output.
  -a           Enable animations.
                 *Will ignore -v and -g options, even though set.
                 *This option will ignore per-thread outputs.
  --help       Display help information, and exit.
                 *-v option will be ignored.
  --genconf    Generate configurations file, and exit.
                 *Unless no configurations file found or configurations file is corrupted, no change will be applied.
                 *Its priority is higher than --help option.
```

 All CLI options were not case-sensitive.
