# ItalianCannon
二营长！你他娘的意大利炮呢！

基于 .NET Core.

简易的多线程工具，看谁不爽打他付费 CDN（

运行 `dotnet ItalianCannon.dll` 即可。

初次使用会自动创建配置文件，修改配置后确认回车就 OK。

**在运行前，记得把 `AppearsToBeDefault` 修改为 `true`**

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
| `AppearsToBeDefault` | 是否为未修改的配置文件 |
