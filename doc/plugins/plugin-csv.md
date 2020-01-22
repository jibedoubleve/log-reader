# Plugin CSV

It reads CSV files

## Connection String
Path to the directory that contains all the days (a day is a unique CSV file)

## Query Day
| key         | Values                                  | Explanations                         |
| ----------- | --------------------------------------- | ------------------------------------ |
| file:       | `[0-9]{4}-[0-9]{2}-[0-9]{2}\\..*\\.csv` | Regex to select a CSV file           |
| date:       | `[0-9]{4}-[0-9]{2}-[0-9]{2}`            | Regex to extract date from file name |
| dateformat: | `yyyy-MM-dd`                            |                                      |

## Query Logs
| Key      | Values                            | Explanations                                   |
| -------- | --------------------------------- | ---------------------------------------------- |
| encoding | `windows-1252` or `UTF8`(default) | Indicates the format of the files              |
| Time     | time:time                         | (facultative) use to bind csv column to the UI |
| Level    | level:level                       | (facultative) use to bind csv column to the UI |
| ThreadId | threadid:threadid                 | (facultative) use to bind csv column to the UI |
| Logger   | logger:logger                     | (facultative) use to bind csv column to the UI |
| Message  | message:message                   | (facultative) use to bind csv column to the UI |