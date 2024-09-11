<<< [Technical specification](techspec.md)

## LogCollector Browser

This module allows you to view logs and monitors, and create new monitors. 
It consists of two main views:

### Logs
![LogCollector1](https://github.com/user-attachments/assets/55b8be60-73cb-42ce-8354-eeda99cdae7d)

The log view allows you to view all logs sent to the solution and saved in the database.

The user can filter the results and check the details of individual logs.

### Monitors
![LogCollector2](https://github.com/user-attachments/assets/6559c57d-8cb7-4a3f-bd18-4b0f242143cf)

The monitor view allows you to browse all existing monitors, create new ones, and edit existing ones.

After entering the monitor details, at the very bottom of the window we can see a list of all alerts performed within a given monitor.

To create a new monitor, click the **Add Monitor** button located above the list, under the filter panel.

A new record will be created at the bottom of the list. After entering the parameters and clicking save, the monitor will be ready to operate
from the start of the next cycle of the HangFire module. Detailed information on creating monitors [here](Monitors.md#Create-monitors).
