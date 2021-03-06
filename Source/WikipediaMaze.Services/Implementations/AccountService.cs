﻿using System;
using System.Linq;
using System.Web.Security;
using WikipediaMaze.Data;
using WikipediaMaze.Core;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Data;
using MvcContrib.Pagination;
using System.Collections.Generic;
using WikipediaMaze.Data.NHibernate;
using facebook;
using System.Web;
using WikipediaMaze.Services;
using System.IO;

namespace WikipediaMaze.Services
{
    public sealed class AccountService : IAccountService
    {
        #region Fields

        private readonly IRepository _repository;
        private readonly IAuthenticationService _authenticationService;
        private readonly API _facebookService;

        #endregion

        #region Constants

        private const string FacebookSessionIdSessionKey = "Facebook_session_key";
        private const string FacebookUserIdSessionKey = "Facebook_userId";
        private const string FacebookSessionExpiresSessionKey = "Facebook_session_expires";

        #endregion

        #region Constructors

        public AccountService(IRepository repository, IAuthenticationService authenticationService)
        {
            _repository = repository;
            _authenticationService = authenticationService;

            _facebookService = new API
                                   {
                                       ApplicationKey =Settings.FacebookApiKey,
                                       Secret = Settings.FacebookSecret,
                                       IsDesktopApplication = false
                                   };
        }

        #endregion

        #region IAccountService Implementation

        /// <summary>
        /// Gets a user with the matching id.
        /// </summary>
        /// <param name="userId">The id of the user to retrieve.</param>
        /// <returns>The user record if found, otherwise null.</returns>
        public User GetUserById(int userId)
        {
            using (var session = _repository.OpenSession())
            {
                return _repository.All<User>().ById(userId);    
            }
        }

        /// <summary>
        /// Returns a users account based on their open Id
        /// </summary>
        public User GetUserByOpenId(string openId)
        {
            using (_repository.OpenSession())
            {
                return _repository.All<User>().Where(
                    x => x.OpenIdentifiers.Select(y => y.Identifier).Contains(openId, StringComparer.OrdinalIgnoreCase))
                    .SingleOrDefault();
            }
        }

        public IEnumerable<User> GetAllUsers(PlayerSortType playerSortType)
        {
            using (_repository.OpenSession())
            {
                switch (playerSortType)
                {
                    case PlayerSortType.Reputation:
                        return _repository.All<User>().OrderByDescending(x => x.Reputation).ToList();
                    case PlayerSortType.Newest:
                        return _repository.All<User>().OrderByDescending(x => x.DateCreated).ToList();
                    case PlayerSortType.Oldest:
                        return _repository.All<User>().OrderBy(x => x.DateCreated).ToList();
                    case PlayerSortType.Name:
                        return _repository.All<User>().OrderBy(x => x.DisplayName).ToList();
                    default:
                        return _repository.All<User>().OrderByDescending(x => x.Reputation).ToList();
                }
            }
        }
        public IPagination<User> GetUsers(int page, int pageSize, PlayerSortType playerSortType)
        {
            IList<User> users;
            int totalUsers;
            using (_repository.OpenSession())
            {
                totalUsers = _repository.All<User>().Count();
                switch (playerSortType)
                {
                    case PlayerSortType.Reputation:
                        users = _repository.All<User>().OrderByDescending(x => x.Reputation).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        break;
                    case PlayerSortType.Newest:
                        users = _repository.All<User>().OrderByDescending(x => x.DateCreated).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        break;
                    case PlayerSortType.Oldest:
                        users = _repository.All<User>().OrderBy(x => x.DateCreated).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        break;
                    case PlayerSortType.Name:
                        users = _repository.All<User>().OrderBy(x => x.DisplayName).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        break;
                    default:
                        users = _repository.All<User>().OrderByDescending(x => x.Reputation).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        break;
                }
            }
            return users.AsCustomPagination(page, pageSize, totalUsers);
        }

        public OpenIdProfile GetOpenIdProfile(string token)
        {
            if (token == null) return null;

            XElement xProfileData = null;

            try
            {
                xProfileData = Rpx.GetProfileData(token);
                if (xProfileData != null) xProfileData = xProfileData.XPathSelectElement("profile");

            }
            catch (InvalidOperationException)
            {
                return null;
            }

            if (xProfileData == null)
                return null;

            var profile = MapFields(xProfileData);
            profile.Name = MapName(xProfileData);
            profile.Address = MapAddress(xProfileData);

            return profile;
        }
        public User GetUserFromProfile(OpenIdProfile profile)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");

            using (_repository.OpenSession())
            {
                var user = GetExistingUserFromProfile(profile);
                if (user != null)
                {
                    if(string.IsNullOrEmpty(user.Email))
                    {
                        user.Email = (string.IsNullOrEmpty(profile.VerifiedEmail)
                                         ? user.Email = profile.Email
                                         : profile.VerifiedEmail) + "";
                    }
                    if(string.IsNullOrEmpty(user.RealName))
                    {
                        if (profile.Name != null)
                            user.RealName = profile.Name.FormattedName + "";
                        else
                            user.RealName = profile.DisplayName + "";
                    }
                    if(string.IsNullOrEmpty(user.DisplayName))
                    {
                        if (!string.IsNullOrEmpty(profile.DisplayName))
                            user.DisplayName = profile.DisplayName;
                        else if (profile.Name != null)
                            user.DisplayName = profile.Name.FormattedName;
                        else if (!string.IsNullOrEmpty(profile.PreferredUserName))
                            user.DisplayName = profile.PreferredUserName;

                        if ((user.DisplayName += "").Trim() == string.Empty)
                            user.DisplayName = "Uknown";
                    }
                    if(string.IsNullOrEmpty(user.Location))
                    {
                        if (profile.Address != null)
                            user.Location = (profile.Address.Locality + " " + profile.Address.Region).Trim();
                    }
                    user.BirthDate = user.BirthDate ?? profile.Birthday;
                    user.Photo = (string.IsNullOrEmpty(user.Photo) ? profile.Photo : user.Photo) + "";
                    user.PreferredUserName = profile.PreferredUserName + "";
                    user.LastVisit = DateTime.Now;

                    using (var tx = _repository.BeginTransaction())
                    {
                        _repository.Save(user);
                        _repository.Save(new ActionItem{Action = ActionType.LoggedIn, DateCreated = DateTime.Now, UserId = user.Id});
                        tx.Commit();
                    }

                    return user;
                }

                return CreateUserFromProfile(profile);
            }
        }

        private User GetExistingUserFromProfile(OpenIdProfile profile)
        {
            var openIdentifier = _repository.All<OpenIdentifier>().ByIdentifier(profile.Identifier);
            return openIdentifier == null ? null : _repository.All<User>().ById(openIdentifier.UserId);
        }

        private User CreateUserFromProfile(OpenIdProfile profile)
        {
            if (_repository.All<OpenIdentifier>().ByIdentifier(profile.Identifier) != null)
                throw new InvalidOperationException(
                    "The openId '{0}' is already associated with another user.".ToFormat(profile.Identifier));

            var openIdentifier = new OpenIdentifier
                                     {
                                         Identifier = profile.Identifier,
                                         IsPrimary = true
                                     };

            var user = new User
                           {
                               BirthDate = profile.Birthday,
                               DateCreated = DateTime.Now,
                               DisplayName = (!string.IsNullOrEmpty(profile.DisplayName)
                                                ? profile.DisplayName : profile.PreferredUserName) + "",
                               Location =
                                   (profile.Address != null
                                       ? (profile.Address.Locality + " " + profile.Address.Region).Trim()
                                       : string.Empty) + "",
                               Email =
                                   (string.IsNullOrEmpty(profile.VerifiedEmail)
                                       ? profile.Email
                                       : profile.VerifiedEmail) + "",
                               LastVisit = DateTime.Now,
                               Photo = profile.Photo + "",
                               PreferredUserName = profile.PreferredUserName + "",
                               RealName = (profile.Name != null ? profile.Name.FormattedName : string.Empty) + "",
                               Reputation = 0
                           };

            if (user.DisplayName.Trim() == string.Empty)
                user.DisplayName = "Uknown";

            using (var tx = _repository.BeginTransaction())
            {
                _repository.Save(user);

                openIdentifier.UserId = user.Id;

                _repository.Save(openIdentifier);

                _repository.Save(new ActionItem{Action = ActionType.Registered, DateCreated = DateTime.Now, UserId = user.Id});

                tx.Commit();
            }

            Rpx.Map(profile.Identifier, user.Id.ToString(System.Globalization.CultureInfo.CurrentCulture));

            return user;
        }

        public User LinkAccounts(int userId, OpenIdProfile profile)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");

            var user = _repository.All<User>().ById(userId);
            if (user == null)
                throw new ArgumentException("A user with the id '{0}' does not exist.");

            var openId = _repository.All<OpenIdentifier>().ByIdentifier(profile.Identifier);
            if (openId != null)
                throw new InvalidOperationException("The OpenId you are trying tolink already exists.");

            openId = new OpenIdentifier {Identifier = profile.Identifier, IsPrimary = false, UserId = user.Id};

            _repository.Save(openId);

            return user;
        }

        public IEnumerable<Notification> GetNotifications(int userId)
        {
            using(_repository.OpenSession())
            {
                return _repository.All<Notification>().Where(x => x.UserId == userId).ToList();
            }
        }

        public IEnumerable<Badge> GetAvailableBadges()
        {
            using(_repository.OpenSession())
            {
                return _repository.All<Badge>().ToList();
            }
        }

        public IPagination<User> GetLeaderBoard(int? page, int? pageSize)
        {
            page = page ?? 1;
            pageSize = pageSize ?? Settings.DefualtPageSize;

            using (_repository.OpenSession())
            {
                var users = _repository.All<User>().OrderByDescending(u => u.Reputation).Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
                return users.AsCustomPagination(page.Value, pageSize.Value, users.Count());
            }
        }

        public void EstablishFacebookSession(HttpRequest request, HttpContext httpContext)
        {
            var sessionKey = (string) httpContext.Session[FacebookSessionIdSessionKey];
            var userId = (long) httpContext.Session[FacebookUserIdSessionKey];
            var authToken = request.QueryString["auth_token"];

            if (!string.IsNullOrEmpty(sessionKey))
            {
                _facebookService.SessionKey = sessionKey;
                _facebookService.uid = userId;

            }
            else if (!String.IsNullOrEmpty(authToken))
            {
                _facebookService.CreateSession(authToken);
                httpContext.Session[FacebookSessionIdSessionKey] = _facebookService.SessionKey;
                httpContext.Session[FacebookUserIdSessionKey] = _facebookService.uid;
                httpContext.Session[FacebookSessionExpiresSessionKey] = _facebookService.SessionExpires;
            }
        }

        public DateTime? GetLastActivityDate(int userId)
        {
            using (_repository.OpenSession())
            {
                var action = _repository.All<ActionItem>().Where(x => x.UserId == userId).OrderByDescending(x => x.DateCreated).FirstOrDefault();

                if (action != null)
                    return action.DateCreated;

                return null;
            }
        }

        public void UpdateUser(User user)
        {
            using (_repository.OpenSession())
            {
                using (var tx = _repository.BeginTransaction())
                {
                    _repository.Update(user);
                    tx.Commit();
                }
            }
        }

        public void DeleteNotification(int playerId, int notificationId)
        {
            using(_repository.OpenSession())
            {
                var notification = _repository.All<Notification>().Where(x => x.Id == notificationId && x.UserId == playerId).FirstOrDefault();
                if(notification != null)
                {
                    _repository.Delete(notification);
                    _repository.Flush();
                }
            }
        }

        #endregion

        #region Profile Mappings

        private static Address MapAddress(XContainer xProfileData)
        {
            var address = new Address();
            var xAddress = xProfileData.Element("address");

            if (xAddress != null)
            {
                var elAddress = xAddress.Element("formatted");
                if (elAddress != null) address.Formatted = elAddress.Value;

                elAddress = xAddress.Element("streetAddress");
                if (elAddress != null) address.StreetAddress = elAddress.Value;

                elAddress = xAddress.Element("locality");
                if (elAddress != null) address.Locality = elAddress.Value;

                elAddress = xAddress.Element("region");
                if (elAddress != null) address.Region = elAddress.Value;

                elAddress = xAddress.Element("postalCode");
                if (elAddress != null) address.PostalCode = elAddress.Value;

                elAddress = xAddress.Element("country");
                if (elAddress != null) address.Country = elAddress.Value;
            }

            return address;
        }
        private static OpenIdProfile MapFields(XContainer xProfileData)
        {
            var profile = new OpenIdProfile();
            var el = xProfileData.Element("identifier");
            if (el != null) profile.Identifier = el.Value;

            el = xProfileData.Element("providerName");
            if (el != null) profile.ProviderName = el.Value;

            el = xProfileData.Element("displayName");
            if (el != null) profile.DisplayName = el.Value;

            el = xProfileData.Element("preferredUsername");
            if (el != null) profile.PreferredUserName = el.Value;

            el = xProfileData.Element("gender");
            if (el != null) profile.Gender = el.Value;

            el = xProfileData.Element("birthday");
            if (el != null)
            {
                DateTime birthday;
                if (DateTime.TryParse(el.Value, out birthday))
                {
                    profile.PrimaryKey = el.Value;
                }
            }

            el = xProfileData.Element("utcOffset");
            if (el != null) profile.UtcOffset = el.Value;

            el = xProfileData.Element("email");
            if (el != null) profile.Email = el.Value;

            el = xProfileData.Element("verifiedEmail");
            if (el != null) profile.VerifiedEmail = el.Value;

            el = xProfileData.Element("url");
            if (el != null) profile.Url = el.Value;

            el = xProfileData.Element("phoneNumber");
            if (el != null) profile.PhoneNumber = el.Value;

            el = xProfileData.Element("photo");
            if (el != null) profile.Photo = el.Value;

            el = xProfileData.Element("primaryKey");
            if (el != null) profile.PrimaryKey = el.Value;

            return profile;
        }
        private static Name MapName(XContainer xProfileData)
        {
            var name = new Name();
            var xName = xProfileData.Element("name");

            if (xName != null)
            {
                var elName = xName.Element("formatted");
                if (elName != null) name.FormattedName = elName.Value;

                elName = xName.Element("familyName");
                if (elName != null) name.FamilyName = elName.Value;

                elName = xName.Element("givenName");
                if (elName != null) name.GivenName = elName.Value;

                elName = xName.Element("middleName");
                if (elName != null) name.MiddleName = elName.Value;

                elName = xName.Element("honorificPrefix");
                if (elName != null) name.HonorificPrefix = elName.Value;

                elName = xName.Element("honorificSuffix");
                if (elName != null) name.HonorificSuffix = elName.Value;
            }
            return name;
        }

        #endregion

    }
}   