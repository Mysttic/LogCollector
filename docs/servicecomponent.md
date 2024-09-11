<<< [Technical specification](techspec.md)

## LogCollector Service

Backend module used to receive logs and store them in the database.

It consists of one endpoint that accepts the log model parameter in the following form:

|Field|Description|
|--|--|
|DeviceId|Enabling device identification|
|ApplicationName|Allowing for application identification|
|IpAddress|Device IP address|
|LogType|e.g. Info, Error, Warning etc.|
|LogMessage|Containing general log information|
|LogContent|Containing detailed log information|

All parameters are text type, the user can freely customize their content 
and specify whether they want to use them in their configuration. 

Example of how to send logs for solution:

#### url: 
```
https://localhost:7267/api/LogEntry
````
#### body: 
```
{
    "deviceId": "Device01",
    "applicationName": "MobileApp",
    "ipAddress": "10.10.10.10",
    "logType": "Info",
    "logMessage": "User successfully logged in.",
    "logContent": "User ID: 12345, Session ID: abcde12345"
}
```
Authorization is done using a pre-configurable API key which must be entered in the header under the name ApiKey.
