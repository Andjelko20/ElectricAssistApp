namespace Server.Models
{
    public class UserConstants
    {
            public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() { Username = "user",Password = "user" },
            new UserModel() { Username = "user1", Password = "user1" },
        };
    }
}

