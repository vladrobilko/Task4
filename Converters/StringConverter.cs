using Task4.Models.Enums;

namespace Task4.Converters
{
    public static class StringConverter
    {
        public static UserManageActions ToUserManageActions(this string action)
        {
            switch (action)
            {
                case "block":
                    return UserManageActions.Block;
                case "unblock":
                    return UserManageActions.Unblock;
                case "delete":
                    return UserManageActions.Delete;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
