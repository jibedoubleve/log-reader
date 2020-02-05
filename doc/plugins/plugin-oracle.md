# Plugin Oracle

It reads logs stored into an Oracle database

## Connection String
Connection string to connect to the database. Report to your documentation to get the connection string

## Query Day

An SQL query to get all the days that has logs. The query should **only** return dates.

### example:

```SQL
select
    distinct trunc(logtime)
from
    table_log
order by trunc(logtime)    
```

## Query Logs
A query to return all the logs for a specified day.

 * The parameter  **@day** is mandatory in the query!
 * The mapping is done thanks to the `as` keyword. The query have to have this mapping with `time`, `level`, `threadid`, `logger`, `message`

### example:
```SQL
select 
    logtime  as time
    loglevel as level
    thid     as threadid
    source   as logger
    col_msg  as message
from
    table_log
where 
trunc(logtime) = trunc(:day)
```