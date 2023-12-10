using Task4.Models.Enums;

namespace Task4.Converters
{
    public static class StringConverter
    {
        public static UserManageActions ToUserManageActions(this string action) =>
            action switch
            {
                "block" => UserManageActions.Block,
                "unblock" => UserManageActions.Unblock,
                "delete" => UserManageActions.Delete,
                _ => throw new ArgumentException()
            };
    }
}
