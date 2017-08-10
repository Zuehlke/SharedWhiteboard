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
