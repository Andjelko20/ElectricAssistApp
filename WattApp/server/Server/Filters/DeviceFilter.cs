using Server.Enums;
using Server.Models;

namespace Server.Filters
{
    public class DeviceFilter
    {
        public static IQueryable<Device> ApplyFilter(IQueryable<Device> devices, DeviceFilterModel filter)
        {
            //Filteri
            if (filter == null)
            {
                return devices;
            }
            if (filter.categoryId != null)
            {
                devices = devices.Where(src => src.DeviceModel.DeviceType.CategoryId == filter.categoryId);
            }
            if (filter.typeId != null)
            {
                devices = devices.Where(src => src.DeviceModel.DeviceTypeId == filter.typeId);
            }
            if (filter.brandId != null)
            {
                devices = devices.Where(src => src.DeviceModel.DeviceBrandId == filter.brandId);
            }
            if (filter.turnOn != null)
            {
                devices = devices.Where(src => src.TurnOn == filter.turnOn);
            }
            if (filter.visibility != null)
            {
                devices = devices.Where(src => src.Visibility == filter.visibility);
            }
            if (filter.controlability != null)
            {
                devices = devices.Where(src => src.Controlability == filter.controlability);
            }
            if(filter.energyByKwh != null)
            {
                if(filter.greaterThan == true)
                {
                    devices = devices.Where(src => src.EnergyInKwh > filter.energyByKwh);
                }
                else
                {
                    devices = devices.Where(src => src.EnergyInKwh <= filter.energyByKwh);
                }
            }

            //Sortiranje
            if(filter.sortCriteria != null)
            {
                switch (filter.sortCriteria)
                {
                    case SortValues.Name:
                        if (filter.byAscending == true)
                        {
                            devices = devices.OrderBy(src => src.Name);
                        }
                        else
                        {
                            devices = devices.OrderByDescending(src => src.Name);
                        }
                        break;
                    case SortValues.EnergyInKwh:
                        if (filter.byAscending == true)
                        {
                            devices = devices.OrderBy(src => src.EnergyInKwh);
                        }
                        else
                        {
                            devices = devices.OrderByDescending(src => src.EnergyInKwh);
                        }
                        break;
                    case SortValues.StandByKwh:
                        if (filter.byAscending == true)
                        {
                            devices = devices.OrderBy(src => src.StandByKwh);
                        }
                        else
                        {
                            devices = devices.OrderByDescending(src => src.StandByKwh);
                        }
                        break;
                }
            }

            if(filter.searchValue != null)
            {
                devices = devices.Where(src => src.Name.StartsWith(filter.searchValue));
            }

            return devices;
        }
    }
}
