using WikipediaMaze.Core;

namespace WikipediaMaze.ViewModels
{
    public class SolutionViewModel
    {
        private readonly Solution _solution;
        private readonly User _user;
        private UserInfoViewModel _userInfo;

        public SolutionViewModel(Solution solution, User user)
        {
            _solution = solution;
            _user = user;
        }

        public UserInfoViewModel UserInfo
        {
            get
            {
                if(_userInfo == null)
                    _userInfo = new UserInfoViewModel(_user);

                return _userInfo;
            }
        }
        public int StepCount
        {
            get { return _solution.StepCount; }
        }
    }
}