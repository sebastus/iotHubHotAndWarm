with step1 as (
    select 
        max(TIMET) as LastTime
        , SERVICEDESC
    FROM 
        hotPathEventHub timestamp by dateadd(S, TIMET, '1970-01-01')
    WHERE
        type='SERVICE'
    group by
        HoppingWindow(minute, 10, 5)
        , SERVICEDESC
)
select
  hotPathEventHub.SERVICEDESC
  , hotPathEventHub.SERVICESTATEID
into aggregator
from 
  hotPathEventHub timestamp by dateadd(S, TIMET, '1970-01-01')
inner join step1 
  on datediff(minute, hotPathEventHub, step1) BETWEEN 0 AND 10
  and hotPathEventHub.TIMET = step1.LastTime
  and hotPathEventHub.SERVICEDESC = step1.SERVICEDESC
WHERE
    hotPathEventHub.type='SERVICE'
