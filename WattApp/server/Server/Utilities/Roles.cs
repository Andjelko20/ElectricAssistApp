using System.Xml.Linq;

namespace Server.Utilities
{
    public class Roles
    {
        public const string Admin = "admin";
        public const long AdminId = 1;
        public const string Dispatcher = "dispatcher";
        public const long DispatcherId = 2;
        public const string Prosumer = "prosumer";
        public const long ProsumerId = 3;
        public const string Guest = "guest";
        public const long GuestId = 4;
        public const string Superadmin = "superadmin";
        public const long SuperadminId = 5;
        public const string AdminPermission = "admin,superadmin";
        public const string Operater = "operater";
        public const long OperaterId = 6;
        public const string AdminOperaterPermission = AdminPermission+",operater";
    }
}
