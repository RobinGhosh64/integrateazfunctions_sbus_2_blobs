#
Read messages from SBUS using session enabled and in batches of 100

FOr every message, data is written in Blob Storage. I am using autoComplete = false in the host.json. Hence I have to ack every message so that it is dequeued!

You are welcome to set it to false. In which case, please comment 18,51 and 53!
