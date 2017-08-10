using SharedWhiteBoard.Models;

namespace Services.Interfaces
{
    public interface ISessionService
    {
        Session GetSession(long sessionPin);

        Session CreateSession();

        bool JoinSession(long sessionPin);

        void EndSession(long sessionPin);
    }
}
