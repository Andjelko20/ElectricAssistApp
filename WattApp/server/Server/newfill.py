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
    randMinutes = random.randint(0, 59)
    randSeconds = random.randint(0, 59)

    StartTime = datetime.datetime.now().replace(microsecond=0)
    StartTime = StartTime.replace(year=StartTime.year-1)
    time = datetime.datetime.now().replace(microsecond=0)
    time = time + datetime.timedelta(days=90)
    for i in range(100000):
        StartTime = StartTime + datetime.timedelta(minutes=randMinutes, seconds=randSeconds)
        EndTime = StartTime + datetime.timedelta(minutes=randMinutes, seconds=randSeconds)
        if StartTime >= time:
            break

        if StartTime.hour>5 and StartTime.hour<17:
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
    SELECT d.Id as Id, d.Name as Name, dt.CategoryId as CategoryId
    FROM Devices d
    JOIN DeviceModels dm ON d.DeviceModelId = dm.Id
    JOIN DeviceTypes dt ON dm.DeviceTypeId = dt.Id
""")

rows = cur.fetchall()
rowsCategories = curCategory.fetchall()
column_names = [desc[0] for desc in curCategory.description]

for row in rowsCategories:
    DeviceCategoryId = row[column_names.index('CategoryId')]
    DeviceId = row[column_names.index('Id')]
    Name = row[column_names.index('Name')]
    #print("ID:", DeviceId)
    #print("DeviceName:", Name)
    #print("DeviceCategoryId:", DeviceCategoryId)
    print(".")
    popunjavanjeTabeleDeviceEnergyUsage(DeviceId, DeviceCategoryId)

conn.commit()
conn.close()
print("Completed database filling.")