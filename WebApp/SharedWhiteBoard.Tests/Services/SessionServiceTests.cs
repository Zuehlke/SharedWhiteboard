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
