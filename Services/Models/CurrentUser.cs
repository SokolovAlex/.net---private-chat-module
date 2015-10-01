#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Web;

using Core.Models.User;
using Core.Enums;

#endregion

public static class CurrentUser
{
    #region Constants

    private const string _currentUserKey = "CurrentUser";

    #endregion

    public static UserSessionModel Info
    {
        get
        {
            return (UserSessionModel)HttpContext.Current.Session[_currentUserKey];
        }
        set
        {
            HttpContext.Current.Session[_currentUserKey] = value;
        }
    }

    public static bool IsLoggedIn
    {
        get
        {
            return HttpContext.Current.Session[_currentUserKey] != null && !IsExpirated();
        }
    }

    public static KeyValuePair<int, KeyValuePair<int, bool>[]>[] GetAccessByGroup(int GroupID)
    {
        var res = Info != null ? Info.SystemAccess.FirstOrDefault(x => x.Key == GroupID) : new KeyValuePair<int, KeyValuePair<int, KeyValuePair<int, bool>[]>[]>();


        return res.Value ?? new KeyValuePair<int, KeyValuePair<int, bool>[]>[1];
    }

    public static KeyValuePair<int, bool>[] GetAccessByGroup(int GroupID, int AccessMatrixEntityID)
    {
        var res = GetAccessByGroup(GroupID).FirstOrDefault(x => x.Key == AccessMatrixEntityID);

        return res.Value ?? new KeyValuePair<int, bool>[1];
    }

    public static bool GetAccessByGroup(int GroupID, int AccessMatrixEntityID, int CrudOperation)
    {
        return GetAccessByGroup(GroupID, AccessMatrixEntityID).FirstOrDefault(x => x.Key == CrudOperation).Value;
    }

    public static bool IsUserInRole(UserRole[] roles)
    {
        return roles.Contains((UserRole)Info.UserModel.RoleId);
    }

    private static bool IsExpirated()
    {
        var result = false;

        Info.IsExpirated = result;

        return result;
    }
}