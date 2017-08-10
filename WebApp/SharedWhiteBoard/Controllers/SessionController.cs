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
using System.Web.Http;
using Services.Interfaces;

namespace SharedWhiteBoard.Controllers
{
    public class SessionController : ApiController
    {
        private readonly ISessionService _sessionService;
        private readonly IDirectoryStructureService _directoryService;

        public SessionController(ISessionService sessionService, IDirectoryStructureService directoryService)
        {
            _sessionService = sessionService;
            _directoryService = directoryService;
        }

        [HttpGet]
        [Route("SessionApi/Session")]
        public IHttpActionResult StartSession()
        {
            var session = _sessionService.CreateSession();

            var storageFolderPath = GetStorageFolderPath(session.SessionPin);
            _directoryService.CreateDirectoryStructureForSession(storageFolderPath);

            return Ok(session.SessionPin);
        }

        private static string GetStorageFolderPath(long sessionPin)
        {
            return $"{AppDomain.CurrentDomain.BaseDirectory}{Resources.Resources.StorageFolder}\\{sessionPin}";
        }

        [HttpGet]
        [Route("SessionApi/Session/{sessionPin:long}")]
        public IHttpActionResult ConnectToExistingSession(long sessionPin)
        {
            var connectionSucceded = _sessionService.JoinSession(sessionPin);

            if (!connectionSucceded)
            {
                return BadRequest(Resources.Resources.NoSessionWithTheGivenPin);
            }

            return Ok(Resources.Resources.ConnectionSuccessfull);
        }

        [HttpGet]
        [Route("SessionApi/Session/{sessionPin:long}/End")]
        public IHttpActionResult EndSession(long sessionPin)
        {
            _sessionService.EndSession(sessionPin);

            var storageFolderPath = GetStorageFolderPath(sessionPin);
            _directoryService.DeleteDirectoryStructureForSession(storageFolderPath);
            
            return Ok();
        }
    }
}
