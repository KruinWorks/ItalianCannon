# ItalianCannon

直接下载预编译的程序包：

[![直接下载预编译包](https://ci.appveyor.com/api/projects/status/rnt9jq5w7p0kdepa?svg=true)](https://ci.appveyor.com/api/projects/Elepover/ItalianCannon/artifacts/ItalianCannon.zip)

CI: https://ci.appveyor.com/project/Elepover/italiancannon

二营长！你他娘的意大利炮呢！

基于 .NET Core 的简易多线程请求工具，看谁不爽打他付费 CDN（

支持 OS 见[巨硬官方](https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0-supported-os.md#net-core-20---supported-os-versions)。

# 运行使用

## 普通操作

*(适用于 RHEL, CentOS, Oracle Linux, Fedora, Debian, Ubuntu, Linux Mint, openSUSE, SLES.)*

配置 .NET Core 运行环境。具体可在[巨硬](https://www.microsoft.com/net)获得详细信息。

获得应用程序源码:

`git clone https://github.com/KruinWorks/ItalianCannon.git`

进入项目文件夹:

`cd ./ItalianCannon/ItalianCannon`

运行 `dotnet run` 即可。

## Arch Linux ~~邪教~~特权

直接安装 `community` 仓库里的 `dotnet-runtime-2.0` 包。

`pacman -Syu dotnet-runtime-2.0`

获得编译好的二进制文件，直接 `dotnet <文件名>` 就好。

## 注意

初次使用会自动创建配置文件，修改配置后确认回车就 OK。

详细配置请参见下方。

# 配置说明

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

| 名称 | 内容 |
| :----- | -----: |
| `Note` | 注释，应用程序会忽略此项目 |
| `TeaCupTarget` | ~~喝茶~~目标 |
| `Threads` | 线程数量 |
| `IntervalPerThread` | 每线程请求间隔时间 |
| `MaxRequestsPerThread` | 每个线程的最大请求总数 |
| `UserAgent` | 请求时使用的用户代理 |
| `AppearsToBeDefault` | 必须修改，是否为未修改的配置文件 |

# 命令行参数

```
  -v           静默模式，不输出任何内容。
                 *-a, --help 参数会无视此选项。
  -c           忽略单线程最大请求数限制。
                 *相当于在配置文件中写入 'MaxRequestsPerThread = 0'
  -g           关闭带颜色的控制台输出。
  -a           启用动画。
                 *输出动画栏会无视 -v 和 -g 选项，即使已经指定。
                 *此选项会同时屏蔽每个独立线程的输出。
  --help       显示帮助信息后退出。
                 *-v 选项将被无视。
  --genconf    生成配置文件，并退出。
                 *除非配置文件不存在或者存在错误，否则程序不会修改配置文件。
                 *本参数优先级高于 --help 参数，如果同时指定，将被忽略。
```

 所有命令行选项均不大小写敏感。
