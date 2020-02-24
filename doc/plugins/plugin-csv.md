# Plugin CSV

It reads CSV files

## Connection String
Path to the directory that contains all the days (a day is a unique CSV file).

## Query Day
The regex should be like:
```
(?<year>[0-9]{4})-(?<month>[0-9]{2})-(?<day>[0-9]{2})\..*\.csv
```
The regex should contain the *year*, *month*, *day* of the date of log into `groups`. The group name should be exactly these one. These `groups` will be used internally to search and display the days with logs.

| group name | Explanations       |
| ---------- | ------------------ |
| year       | Contains the year  |
| month      | Contains the month |
| day        | Contains the day   |

## Query Logs
| Key      | Default                 | Valid values           | Explanations                        |
| -------- | ----------------------- | ---------------------- | ----------------------------------- |
| Encoding | `encoding:windows-1252` | `windows-1252`, `UTF8` | Indicates the encoding of the file. |
| Time     | `time:time`             |                        | Bind csv column to the UI           |
| Level    | `level:level`           |                        | Bind csv column to the UI           |
| ThreadId | `threadid:threadid`     |                        | Bind csv column to the UI           |
| Logger   | `logger:logger`         |                        | Bind csv column to the UI           |
| Message  | `message:message`       |                        | Bind csv column to the UI           |

## Preconfigured
### Probel log files

| Query | Regular expression                                                                                                  |
| ----- | ------------------------------------------------------------------------------------------------------------------- |
| Day   | `(?<year>[0-9]{4})-(?<month>[0-9]{2})-(?<day>[0-9]{2})\..*\.csv`                                                    |