<<< [Technical specification](techspec.md)

## LogCollector Monitor

Backend module that executes tasks created by the user, sending a query to the database in search of records that meet the conditions specified in the query. 
Then the action specified by the user in the monitor definition is executed, which include:

|Action|Required parameters|Handler|Description|
|--|--|--|--|
|Email| <li>Email</li><li>Subject</li> |[SendGrid](https://sendgrid.com)|The information is sent to the specified email address, with the specified subject and the content of the log in the message.|
|SMS|<li>Phone number</li>|[Twilio](https://www.twilio.com)|The information is sent to the specified phone number, in which the text message contains the content of the log.|
|CustomApiCall|<li>Url</li><li>AuthKey</li>|http|The information is sent via http message to the specified url address, the content will include the log content.|


The queuing of executed tasks was handled using [Hangfire](https://www.hangfire.io). 

It checks the logs every minute for compliance with the monitor condition and creates an alert if the condition is met.

All alerts created by the monitor are saved in the database and then processed based on the type of action specified in the monitor definition. 
Actions are also queued using Hangfire.
