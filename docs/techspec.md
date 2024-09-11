<<< [README.md](../README.md)

## General

![BasicDiagram](https://github.com/user-attachments/assets/9770fe4b-6350-4027-aa65-1d0e4a75457f)

The solution consists of three main components:
- [**LogCollector Service**](servicecomponent.md)

  An api application exposed to log collection. Other applications can connect to it by sending api calls containing logs.
  Using this application it is not possible to download logs from the database, this functionality is handled by the Browser application server.
  
- [**LogCollector Monitor**](monitorcomponent.md)

  A service whose purpose is to perform tasks created in monitors.
  It continuously analyzes the log database to detect logs that meet the conditions of a given monitor.
  In the monitor module, the Hangfire queue server is responsible for processing tasks to be performed.
  In addition to the basic functionality, it provides a graphical interface for viewing tasks being performed.
  
  
- [**LogCollector Browser**](browsercomponent.md)
  
  A browser application that allows viewing and managing logs and monitors.
  It consists of a backend part written in .NET 8 and a frontend in React.
  From the application level you can manage log browsing and browse and add/modify monitors.
  More about creating monitors [here](Monitors.md#Create-monitors).


On top of all these applications runs the .NET Aspire service where you can easily manage all services at once.
  
