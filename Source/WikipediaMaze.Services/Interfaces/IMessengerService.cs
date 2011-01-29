using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikipediaMaze.Services
{
    public interface IMessengerService
    {
        void SendPuzzleCreatedMessage(int puzzleId);
        void SendNewPuzzleLeaderMessage(int userId, int stepCount, int puzzleId);
    }
}
