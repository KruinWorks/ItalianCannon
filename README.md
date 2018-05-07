# ItalianCannon

Download pre-compiled package: https://see.wtf/BiMDY

[![AppVeyor Compile Status](https://ci.appveyor.com/api/projects/status/rnt9jq5w7p0kdepa?svg=true)](https://ci.appveyor.com/project/Elepover/italiancannon)

A simple multithreading web resource request tool. Based on .NET Core.

Supported OS: [See Microsoft Official](https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0-supported-os.md#net-core-20---supported-os-versions).

# Runtime

## Ordinary

*(Suitable for RHEL, CentOS, Oracle Linux, Fedora, Debian, Ubuntu, Linux Mint, openSUSE, SLES.)*

Set up the .NET Core SDK.

Details could be found on [Microsoft Official](https://www.microsoft.com/net).

Get application source code: 

`git clone https://github.com/KruinWorks/ItalianCannon.git`

Enter project directory: 

`cd ./ItalianCannon/ItalianCannon`

Restore NuGet packages:

`dotnet restore`

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

See https://see.wtf/6nhoB for further information.

You can run ItalianCannon with `--setup` argument to activate Setup Wizard.
