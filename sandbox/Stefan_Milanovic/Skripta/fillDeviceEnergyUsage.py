import sqlite3
import random
import datetime

# povezivanje sa bazom
conn = sqlite3.connect('sqlite.db')

def uredjajiKategorije1(DeviceId):
    randDays = random.randint(0, 99)
    randHours = random.randint(0, 5)
    randMinutes = random.randint(0, 59)
    randSeconds = random.randint(0, 59)

    StartTime = datetime.datetime.now().replace(microsecond=0)
    StartTime = StartTime.replace(year=StartTime.year-1)
    time = datetime.datetime.now().replace(microsecond=0)
    for i in range(20000):
        StartTime = StartTime + datetime.timedelta(hours=randHours, minutes=randMinutes, seconds=randSeconds)
        EndTime = StartTime + datetime.timedelta(hours=randHours, minutes=randMinutes, seconds=randSeconds)
        if StartTime >= time or EndTime>=time:
            break
        conn.execute(f"INSERT INTO DeviceEnergyUsages (DeviceId, StartTime, EndTime, EnergyInKwh) VALUES ({DeviceId}, '{StartTime}', '{EndTime}', 1.33)")

def uredjajiKategorije2(DeviceId):
    time = datetime.datetime.now().replace(microsecond=0)
    StartTime = datetime.datetime.now().replace(microsecond=0)
    StartTime = StartTime.replace(year=StartTime.year-1)
    for i in range(20000):
        randHoursStart = random.randint(5, 7)
        randHoursEnd = random.randint(8, 12)
        randMinutes = random.randint(0, 39)
        randSeconds = random.randint(0, 59)
        
        StartTime = StartTime.replace(hour=randHoursStart)
        StartTime = StartTime + datetime.timedelta(days=1, minutes=randMinutes, seconds=randSeconds)
        EndTime = StartTime + datetime.timedelta(hours=randHoursEnd, minutes=randMinutes, seconds=randSeconds)
        if StartTime >= time or EndTime>=time:
            break
        conn.execute(f"INSERT INTO DeviceEnergyUsages (DeviceId, StartTime, EndTime, EnergyInKwh) VALUES ({DeviceId}, '{StartTime}', '{EndTime}', 1.33)")


def popunjavanjeTabeleDeviceEnergyUsage(DeviceId, DeviceCategoryId):
    if(DeviceCategoryId == 1):
        uredjajiKategorije1(DeviceId)
    elif(DeviceCategoryId == 2):
        uredjajiKategorije2(DeviceId)


# popunjavanje tabele DeviceEnergyUsage
cur = conn.cursor()
cur.execute(f"SELECT * FROM Devices")

rows = cur.fetchall()
#print("---{}---".format(len(rows)))
for i in range(0,len(rows)):
    DeviceCategoryId = rows[i][2]
    #print(rows[i]) 
    #print("DeviceId: {} - DeviceCatgoryId: {}".format(i+1, DeviceCategoryId))
    popunjavanjeTabeleDeviceEnergyUsage(i+1, DeviceCategoryId) # i+1 se salje jer se uredjaji popunjavaju od id 1, ne od id 0

conn.commit()
conn.close()