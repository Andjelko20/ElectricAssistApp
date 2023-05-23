import sqlite3
import random
import datetime

# povezivanje sa bazom
conn = sqlite3.connect('sqlite.db')

def uredjajiKategorije2(DeviceId):
    randDays = random.randint(0, 99)
    randHours = random.randint(0, 5)
    randMinutes = random.randint(0, 59)
    randSeconds = random.randint(0, 59)
    #randRangeOfHour = random.randint(0,2)
    #randRangeOfMinutes = random.randint(0,59)
    #randRangeOfSecundes = random.randint(0,59)

    StartTime = datetime.datetime.now().replace(microsecond=0)
    StartTime = StartTime.replace(year=StartTime.year-1)
    time = datetime.datetime.now().replace(microsecond=0)
    time = time + datetime.timedelta(days=90)
    for i in range(20000):
        #StartTime = StartTime + datetime.timedelta(hours=randRangeOfHour, minutes=randRangeOfMinutes, seconds=randRangeOfSecundes)
        StartTime = StartTime + datetime.timedelta(hours=randHours, minutes=randMinutes, seconds=randSeconds)
        EndTime = StartTime + datetime.timedelta(hours=randHours, minutes=randMinutes, seconds=randSeconds)
        if StartTime >= time:
            break
        conn.execute(f"INSERT INTO DeviceEnergyUsages (DeviceId, StartTime, EndTime) VALUES ({DeviceId}, '{StartTime}', '{EndTime}')")

def uredjajiKategorije1(DeviceId):
    time = datetime.datetime.now().replace(microsecond=0)
    StartTime = datetime.datetime.now().replace(microsecond=0)
    StartTime = StartTime.replace(year=StartTime.year-1)
    time = time + datetime.timedelta(days=90)
    for i in range(20000):
        randHoursStart = random.randint(5, 7)
        randHoursEnd = random.randint(8, 12)
        randMinutes = random.randint(0, 39)
        randSeconds = random.randint(0, 59)
        
        StartTime = StartTime.replace(hour=randHoursStart)
        StartTime = StartTime + datetime.timedelta(days=1, minutes=randMinutes, seconds=randSeconds)
        EndTime = StartTime + datetime.timedelta(hours=randHoursEnd, minutes=randMinutes, seconds=randSeconds)
        if StartTime >= time:# or EndTime>=time:
            break
        conn.execute(f"INSERT INTO DeviceEnergyUsages (DeviceId, StartTime, EndTime) VALUES ({DeviceId}, '{StartTime}', '{EndTime}')")


def popunjavanjeTabeleDeviceEnergyUsage(DeviceId, DeviceCategoryId):
    if(DeviceCategoryId == 1):
        uredjajiKategorije1(DeviceId)
    elif(DeviceCategoryId == 2):
        uredjajiKategorije2(DeviceId)


# popunjavanje tabele DeviceEnergyUsage
cur = conn.cursor()
cur.execute(f"SELECT * FROM Devices")

curCategory = conn.cursor()
curCategory.execute("""
    SELECT dc.Id
    FROM Devices d
    JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
    JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id
    JOIN DeviceCategories dc ON dt.categoryId = dc.Id
""")

rows = cur.fetchall()
rowsCategories = curCategory.fetchall()
for i in range(0,len(rows)):
    DeviceCategoryId = rowsCategories[i][0]
    print(DeviceCategoryId)
    popunjavanjeTabeleDeviceEnergyUsage(i+1, DeviceCategoryId) # i+1 se salje jer se uredjaji popunjavaju od id 1, ne od id 0

conn.commit()
conn.close()