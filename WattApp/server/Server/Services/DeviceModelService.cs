using Server.Models.DropDowns.Devices;

namespace Server.Services
{
    public interface DeviceModelService
    {
        public string getModelNameById(long modelId);
        public DeviceModel getDeviceModel(long modelId);
    }
}
