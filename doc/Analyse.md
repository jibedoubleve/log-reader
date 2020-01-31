# Analyse
## Definitions

| Word         | definition                                                                             |
| ------------ | -------------------------------------------------------------------------------------- |
| `repository` | The source of all logs. Can be the database or the directory where all file are stored |
| `day`        | Container that has all the logs for a specified day                                    |
| `logline`    | A line in the log container. It contains all the information of an event               |

## Menu

```
   +-- File
      +-- Load repository 1
      +-- Load repository 2
      +-- Load repository 3
     
      +-- Manage repositories  
   +-- Filters
      +-- Apply filter 1
      +-- Apply filter 2
      +-- Apply filter 3

      +-- Manage filters
```


# Plugins
## How does it work?
 - `PluginManager` 
   - `Build(PluginConfig cfg)`
 - `Plugin`
   - `GetAllDays`
   - `GetLogs(Day day)`

## Configuration
| Group      | Name              | Value    |
| ---------- | ----------------- | -------- |
| plugin     | plugin-name       | `string` |
| plugin     | plugin-id         | `guid`   |
| repository | connection-string | `string` |
| day        | query-day         | `string` |
| logline    | query-log         | `string` |

## Configuration Examples

### SQLite

| Group      | Key               | Value                                |
| ---------- | ----------------- | ------------------------------------ |
| plugin     | plugin-name       | sqlite-logreader                     |
| plugin     | plugin-id         | c8d4b05b-8a59-42cc-996a-f6ca581f0edf |
| repository | connection-string | `Data Source=[...];Version=3;`       |
| day        | query-day         | `select [...] from [...]`            |
| log        | query-log         | `select [...] from [...]`            |

### CSV
| Group      | Name              | Value                                |
| ---------- | ----------------- | ------------------------------------ |
| plugin     | plugin-name       | csv-logreader                        |
| plugin     | plugin-id         | f55ea9ee-da17-4b6e-bab1-1805592507d0 |
| repository | connection-string | `C:\xxx\`                            |
| day        | query-day         | `\d{4}-\d{2}-\d{2}`   |
| log        | query-log         | `Time;Level;Logger;ThreadId;Message;Exception`        |

# Filters

| Filter   | Operator  | Operand            |
| -------- | --------- | ------------------ |
| Time     | less than | minutes            |
| Category | in list   | list of categories |
| Level    | in list   | list of level      |

## How does it work?
- `QueryBuilder`
  - `AddFilter(json)`
  - `Build()`