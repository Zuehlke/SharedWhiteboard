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

using System;
using System.Web.Http.Results;
using Models;
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
