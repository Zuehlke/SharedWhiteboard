//
// Copyright 2017, Zühlke
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
// 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
// 
// 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer 
//    in the documentation and/or other materials  provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF  MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS 
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL  DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER  IN CONTRACT, STRICT 
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

using System.Collections.Generic;
using System.Linq;
using Models;
using Services.Interfaces;

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