using System;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using Services.Interfaces;
using SharedWhiteBoard.Controllers;
using SharedWhiteBoard.Models;

namespace SharedWhiteBoard.Tests.Controllers
{
    [TestFixture]
    public class SessionControllerTests
    {
        [Test]
        public void WhenStartSession_ThenNewSessionAndDirectoryStructureCreatedAndSessionPinReturned()
        {
            var dummySession = new Session();

            var sessionServiceMock = new Mock<ISessionService>();

            sessionServiceMock.Setup(m => m.CreateSession())
                .Returns(dummySession);

            var directoryServiceMock = new Mock<IDirectoryStructureService>();

            var sessionController = new SessionController(sessionServiceMock.Object, directoryServiceMock.Object);

            // When
            var resultSessionPin = ((OkNegotiatedContentResult<long>) sessionController.StartSession()).Content;
            

            // Then
            sessionServiceMock.Verify(m => m.CreateSession());
            directoryServiceMock.Verify(m => m.CreateDirectoryStructureForSession($"{AppDomain.CurrentDomain.BaseDirectory}{Resources.Resources.StorageFolder}\\{dummySession.SessionPin}"));
            Assert.AreEqual(dummySession.SessionPin, resultSessionPin);
        }

        [Test]
        public void GivenSession_WhenConnectToExistingSessionWithWrongPin_ThenBadRequest()
        {
            // Given
            var existingSession = new Session();

            var sessionServiceMock = new Mock<ISessionService>();
            sessionServiceMock.Setup(m => m.JoinSession(existingSession.SessionPin)).Returns(true);
            sessionServiceMock.Setup(m => m.JoinSession(It.IsAny<long>())).Returns(false);

            var directoryServiceMock = new Mock<IDirectoryStructureService>();

            var sessionController = new SessionController(sessionServiceMock.Object, directoryServiceMock.Object);

            // When
            var resultMessage = ((BadRequestErrorMessageResult)sessionController.ConnectToExistingSession(existingSession.SessionPin + 1)).Message;

            // Then
            Assert.AreEqual("There is no active session with the given pin.", resultMessage);
        }

        [Test]
        public void GivenSession_WhenConnectToExistingSessionWithCorrectPin_ThenConnectionSuccessfullAndJoinSessionInvoked()
        {
            // Given
            var existingSession = new Session();

            var sessionServiceMock = new Mock<ISessionService>();
            sessionServiceMock.Setup(m => m.JoinSession(It.IsAny<long>())).Returns(false);
            sessionServiceMock.Setup(m => m.JoinSession(existingSession.SessionPin)).Returns(true);

            var directoryServiceMock = new Mock<IDirectoryStructureService>();

            var sessionController = new SessionController(sessionServiceMock.Object, directoryServiceMock.Object);

            // When
            var resultMessage = ((OkNegotiatedContentResult<string>)sessionController.ConnectToExistingSession(existingSession.SessionPin)).Content;

            // Then
            sessionServiceMock.Verify(m => m.JoinSession(existingSession.SessionPin));
            Assert.AreEqual("Connection successfull", resultMessage);
        }

        [Test]
        public void WhenEndSession_ThenSessionEndedAndDirectoryDeleted()
        {
            const long sessionPin = 5;

            // When            
            var sessionServiceMock = new Mock<ISessionService>();
            var directoryServiceMock = new Mock<IDirectoryStructureService>();

            var sessionController = new SessionController(sessionServiceMock.Object, directoryServiceMock.Object);
            sessionController.EndSession(sessionPin);

            // Then
            sessionServiceMock.Verify(m => m.EndSession(sessionPin));
            directoryServiceMock.Verify(m => m.DeleteDirectoryStructureForSession($"{AppDomain.CurrentDomain.BaseDirectory}{Resources.Resources.StorageFolder}\\{sessionPin}"));
        }
    }
}
