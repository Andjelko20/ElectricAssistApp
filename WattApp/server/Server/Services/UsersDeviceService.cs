using Microsoft.Data.Sqlite;
using Server.Models;

namespace Server.Services
{
    public class UsersDeviceService
    {
        // Putanja do SQLite baze podataka
        private string ConnectionString = "Data Source=sqlite.db";
        private String SqlQuery = null;

        // servis za uredjaj za korisnika ciji je ID prosledjen
        public async Task<List<Device>> GetUsersDevicesByUserId(long id)
        {
            SqlQuery = $"SELECT * FROM Devices WHERE UserId={id}";
            List<Device> Devices = new List<Device>();

            try
            {
                using (SqliteConnection _connection = new SqliteConnection(ConnectionString))
                {
                    await _connection.OpenAsync();
                    using (SqliteCommand command = new SqliteCommand(SqlQuery, _connection))
                    {
                        using (SqliteDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Device Device = new Device();
                                Device.Id = reader.GetInt64(0);
                                Device.UserId = reader.GetInt64(2);
                                Device.DeviceModelId = reader.GetInt64(3);
                                Device.Name = reader.GetString(4);
                                Device.EnergyInKwh = reader.GetFloat(5);
                                Device.Visibility = reader.GetBoolean(6);
                                Device.Controlability = reader.GetBoolean(7);
                                Device.TurnOn = reader.GetBoolean(8);

                                Devices.Add(Device);
                            }
                        }
                    }
                    _connection.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
            }
            return Devices; // vrati sve korisnikove uredjate ili praznu listu, proveri da li je prazna
        }
    }
}
