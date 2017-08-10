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

using NUnit.Framework;
using Services.Interfaces;
using Services.Services;

namespace SharedWhiteBoard.Tests.Services
{
    [TestFixture]
    public class SessionServiceTests
    {
        private ISessionService _sessionService;

        [SetUp]
        public void TestSetUp()
        {
            _sessionService = new SessionService();
        }

        [Test]
        [Ignore("Ignored until actual db introduced.")]
        public void GivenNoSessions_WhenGetSession_ThenNullReturned()
        {
            // Given
            
            // When
            var session = _sessionService.GetSession(2);

            // Then
            Assert.IsNull(session);
        }

        [Test]
        public void GivenSession_WhenGetSessionWithWrongPin_ThenNullReturned()
        {
            // Given
            var createdSession = _sessionService.CreateSession();

            // When
            var receivedSession = _sessionService.GetSession(createdSession.SessionPin + 1);

            // Then
            Assert.IsNull(receivedSession);
        }

        [Test]
        public void GivenSession_WhenGetSessionWithCorrectPin_ThenSessionReturned()
        {
            // Given
            var createdSession = _sessionService.CreateSession();

            // When
            var receivedSession = _sessionService.GetSession(createdSession.SessionPin);

            // Then
            Assert.AreEqual(createdSession.SessionPin, receivedSession.SessionPin);
        }

        [Test]
        public void WhenSessionCreated_ThenSessionIsActiveSetToTrue()
        {
            // When
            var session = _sessionService.CreateSession();

            // Then
            Assert.IsTrue(session.IsActive);
        }

        [Test]
        public void WhenSessionCreated_ThenBothParticipantsJoinedSetToFalse()
        {
            // When
            var session = _sessionService.CreateSession();

            // Then
            Assert.IsFalse(session.BothParticipantsJoined);
        }

        [Test]
        public void GivenExistingSession_WhenAnotherSessionCreated_ThenSessionPinIncremented()
        {
            // Given
            var existingSession = _sessionService.CreateSession();

            // When
            var newSession = _sessionService.CreateSession();

            // Then
            Assert.AreEqual(existingSession.SessionPin + 1, newSession.SessionPin);
        }

        [Test]
        public void GivenSession_WhenSessionEnd_ThenSessionActiveSetToFalse()
        {
            // Given
            var session = _sessionService.CreateSession();

            // When
            _sessionService.EndSession(session.SessionPin);

            // Then
            var modifiedSession = _sessionService.GetSession(session.SessionPin);
            Assert.IsFalse(modifiedSession.IsActive);
        }
    }
}
