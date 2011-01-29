using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Core;
using MvcContrib.Pagination;
using System.IO;

namespace WikipediaMaze.Services
{
    public interface IAccountService
    {
        OpenIdProfile GetOpenIdProfile(string token);
        User GetUserFromProfile(OpenIdProfile profile);
        IPagination<User> GetLeaderBoard(int? page, int? pageSize);
        User GetUserById(int userId);
        User GetUserByOpenId(string openId);
        IEnumerable<User> GetAllUsers(PlayerSortType playerSortType);
        IPagination<User> GetUsers(int page, int pageSize, PlayerSortType playerSortType);
        User LinkAccounts(int userId, OpenIdProfile profile);
        IEnumerable<Notification> GetNotifications(int userId);
        IEnumerable<Badge> GetAvailableBadges();

        DateTime? GetLastActivityDate(int userId);

        void UpdateUser(User user);

        void DeleteNotification(int playerId, int notificationId);
    }
}
