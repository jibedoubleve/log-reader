# Plugin CSV

It reads CSV files

## Connection String
Path to the directory that contains all the days (a day is a unique CSV file)

## Query Day
| key         | Default                                 | Explanations                               |
| ----------- | --------------------------------------- | ------------------------------------------ |
| file:       | `[0-9]{4}-[0-9]{2}-[0-9]{2}\\..*\\.csv` | Regex to select CSV files                  |
| date:       | `[0-9]{4}-[0-9]{2}-[0-9]{2}`            | Regex to extract date from file name       |
| dateformat: | `yyyy-MM-dd`                            | Pattern use to infer a date from file name |

## Query Logs
| Key      | Default             | Valid values           | Explanations                                   |
| -------- | ------------------- | ---------------------- | ---------------------------------------------- |
| encoding | `windows-1252`      | `windows-1252`, `UTF8` | Indicates the format of the files              |
| Time     | `time:time`         |                        | (facultative) use to bind csv column to the UI |
| Level    | `level:level`       |                        | (facultative) use to bind csv column to the UI |
| ThreadId | `threadid:threadid` |                        | (facultative) use to bind csv column to the UI |
| Logger   | `logger:logger`     |                        | (facultative) use to bind csv column to the UI |
| Message  | `message:message`   |                        | (facultative) use to bind csv column to the UI |