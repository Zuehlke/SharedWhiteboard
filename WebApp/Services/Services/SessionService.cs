using System.Collections.Generic;
using System.Linq;
using Services.Interfaces;
using SharedWhiteBoard.Models;

namespace Services.Services
{
    public class SessionService : ISessionService
    {
        private static readonly IList<Session> Sessions = new List<Session>();

        public Session GetSession(long sessionPin)
        {
            return Sessions.SingleOrDefault(s => s.SessionPin == sessionPin);
        }

        public Session CreateSession()
        {
            var session = new Session();
            Sessions.Add(session);

            return session;
        }

        public bool JoinSession(long sessionPin)
        {
            var session = GetSession(sessionPin);

            if (session == null || !session.IsActive)
            {
                return false;
            }

            session.BothParticipantsJoined = true;

            return true;
        }

        public void EndSession(long sessionPin)
        {
            var session = GetSession(sessionPin);

            if (session == null || !session.IsActive)
            {
                return;
            }

            session.IsActive = false;
        }
    }
}