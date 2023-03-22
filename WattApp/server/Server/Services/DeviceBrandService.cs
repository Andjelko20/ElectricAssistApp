using Server.Models.DropDowns.Devices;
using System.Diagnostics.Eventing.Reader;

namespace Server.Services
{
    public interface DeviceBrandService
    {
        public string getBrandNameById(long brandId);
    }
}
