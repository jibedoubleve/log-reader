# Plugin Text

It reads any text files. It uses only regex to select logs.

## Connection String
Path to the directory that contains all the days (a day is a unique text file)

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
The regex should be like:
```
(?<time>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{3}).*(?<level>(trace|debug|info|warn|error|fatal)):(?<message>.*)
```
The regex should contain in group the year, month, day of the date of log. The group name should be exactly these one.

| group name | Explanations                                                  |
| ---------- | ------------------------------------------------------------- |
| time       | get time, should able to be pased by `DateTime.Parse(string)` |
| level      | level of logs                                                 |
| threadid   | thread id                                                     |
| logger     | logger                                                        |
| message    | message                                                       |
| exception  | exception                                                     |

## Preconfigured
### Github desktop log files
| Query | Regular expression                                                                                                  |
| ----- | ------------------------------------------------------------------------------------------------------------------- |
| Day   | `(?<year>[0-9]{4})-(?<month>[0-9]{2})-(?<day>[0-9]{2})\..*\.log`                                                    |
| Log   | `(?<time>\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d{3}).*(?<level>(trace|debug|info|warn|error|fatal)):(?<message>.*)` |
