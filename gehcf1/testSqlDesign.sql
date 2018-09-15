--select top 10 * from service
--where TimeT_datetime > '2018-09-09'
--order by TimeT_datetime desc

--declare @t_end datetime
--select @t_end = '2018-09-07 17:18'

--declare @t_begin datetime
--select @t_begin = '2018-09-07 16:23';

--select count(distinct SERVICEDESC) as count
--from service 
--where 
--  TimeT_datetime >= @t_begin
--  and TimeT_datetime < dateadd(minute, 10, dateadd(minute, 45, @t_begin))


-- select dateadd(s,1536337399, '1970-01-01') -- 2018-09-07 16:23 = beginning of data set
-- select dateadd(s,1536340699, '1970-01-01') -- 2018-09-07 17:18 = end of data set, total of 55 minutes
--           1         2         3         4         5         6
-- 0123456789012345678901234567890123456789012345678901234567890
-- window 1-> 24
--      window 2-> 24
--           window 3-> 24
--                window 4-> 24
--                     window 5-> 25
--                          window 6-> 25
--                               window 7-> 25
--                                    window 8-> 25
--                                         window 9-> 25
--                                              window10-> 25





--select count(distinct SERVICEDESC)
--with step1(maxt, SERVICEDESC) as 
--(
--select max(TIMET) as 'maxt', SERVICEDESC
--from service
--where 
--  TimeT_datetime >= @t_begin
--  and TimeT_datetime < dateadd(minute, 10, @t_begin)
--group by SERVICEDESC
--)

--select TIMET, TIMET_datetime, service.SERVICEDESC, SERVICESTATEID
--from service inner join step1
-- on service.TIMET = step1.maxt and service.SERVICEDESC=step1.SERVICEDESC
--order by SERVICEDESC


--select TIMET, TimeT_datetime 
--from service 
--where SERVICEDESC='5c51238d-05bc-4546-a909-da6f4ce05e9a' 
-- and TimeT_datetime >= '2018-09-07 16:23'
--order by TIMET asc

--select max(timet)
--from service
-- 1535967776 = min
-- 1536608677 = max

--select dateadd(second, 1535967776, '1970-01-01') -- = 2018-09-03 09:42:56.000

--select dateadd(second, 1536608677, '1970-01-01') -- = 2018-09-10 19:44:37.000


declare @t_end datetime
select @t_end = '2018-09-10 19:44';

declare @t_begin datetime
select @t_begin = '2018-09-03 09:42';

select count(distinct SERVICEDESC) as count
from service 
where 
  TimeT_datetime >= @t_begin
  and TimeT_datetime < dateadd(minute, 1, dateadd(minute, 100, @t_begin))

