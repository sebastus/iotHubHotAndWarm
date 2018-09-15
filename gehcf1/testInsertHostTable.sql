INSERT INTO host (id
, TIMET
, HOSTNAME
, HOSTSTATEID
, HOSTOUTPUT
, start_time
, end_time
, latency
, deviceId
, EventProcessedUtcTime
, PartitionId
, EventEnqueuedUtcTime)
VALUES (
'ab68d4ca-eacc-4021-b563-3185285d1184'
, 1535463969
, 'ab68d4ca-eacc-4021-b563-3185285d1184'
, 0
, 'OK - 127.0.0.1: rta 0.037ms, lost 0%'
, 1535464389
, 1535464389
, 0.003712
, 'simdevice1'
, '2018-08-28T13:53:10.3610094Z'
, 0
, '2018-08-28T13:53:10.267Z'
)

select * from host
