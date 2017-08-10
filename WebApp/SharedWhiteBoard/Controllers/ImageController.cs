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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Services.Interfaces;
using Services.Services;
using WhiteBoardDetection;

namespace SharedWhiteBoard.Controllers
{
    public class ImageController : ApiController
    {
        private readonly ISessionService _sessionService;

        public ImageController()
        {
            _sessionService = new SessionService();
        }

        [HttpPost]
        [Route("ImageApi/Session/{sessionPin:long}/Image/{participantOrder}")]
        public async Task<IHttpActionResult> UploadImage(long sessionPin, string participantOrder)
        {
            if (!SessionIsValid(sessionPin))
            {
                return BadRequest(Resources.Resources.NoSessionWithTheGivenPin);
            }

            try
            {
                var image = await Request.Content.ReadAsByteArrayAsync();

                var storageFolderPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}";

                var participantStorageFolderPath = $"{storageFolderPath}\\{sessionPin}\\{participantOrder}";
                var inputDirectoryFullPath = $"{participantStorageFolderPath}\\{Resources.Resources.InputFolder}\\image.jpg";

                System.IO.File.WriteAllBytes(inputDirectoryFullPath, image);

                // TODO Use IoC
                var imageRotator = new ImageRotator();
                var whiteBoardExtractor = new WhiteBoardExtractor(
                    new CornerFinderAccordingToRectangles(
                        new SimilarityChecker(), 
                        imageRotator, 
                        new RectangleFinder()), 
                    imageRotator, 
                    new DarkAreaExtractor());

                var templatesFolderPath = $"{storageFolderPath}\\{Resources.Resources.TemplatesFolder}";
                whiteBoardExtractor.DetectAndCrop(participantStorageFolderPath, templatesFolderPath);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpGet]
        [Route("ImageApi/Session/{sessionPin:long}/Image/{participantOrder}")]
        public HttpResponseMessage GetLastImage(long sessionPin, string participantOrder)
        {
            var session = _sessionService.GetSession(sessionPin);

            if (session == null || !session.IsActive)
            {
                return CreateBadRequestResponse(Resources.Resources.NoSessionWithTheGivenPin);
            }

            if (!session.BothParticipantsJoined)
            {
                return CreateBadRequestResponse(Resources.Resources.NotAllParticipantsJoinedSession);
            }

            var filePath = GetOutputFilePath(participantOrder);

            return CreateResponseMessageFromFile(filePath);
        }

        private static HttpResponseMessage CreateBadRequestResponse(string responseMessage)
        {
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(responseMessage)
            };

            return response;
        }

        private static HttpResponseMessage CreateResponseMessageFromFile(string filePath)
        {
            var fileContent = System.IO.File.ReadAllBytes(filePath);
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileContent)
            };

            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return result;
        }

        [HttpGet]
        [Route("ImageApi/Session/{sessionPin:long}/Image/{participantOrder}/Dark")]
        public HttpResponseMessage GetLastImageWithOnlyDarkAreas(long sessionPin, string participantOrder)
        {
            if (!SessionIsValid(sessionPin))
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(Resources.Resources.NoSessionWithTheGivenPin)
                };

                return response;
            }

            var filePath = GetDarkOutputFilePath(participantOrder);
            
            return CreateResponseMessageFromFile(filePath);
        }

        private bool SessionIsValid(long sessionPin)
        {
            var session = _sessionService.GetSession(sessionPin);
            return session != null && session.IsActive && session.BothParticipantsJoined;
        }

        private static string GetOutputFilePath(string participantOrder)
        {
            var outputFolderParentFolder = participantOrder == "A" ? "B" : "A";
            var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\{outputFolderParentFolder}\\{Resources.Resources.OutputFolder}\\image.jpg";

            return filePath;
        }

        private static string GetDarkOutputFilePath(string participantOrder)
        {
            var outputFolderParentFolder = participantOrder == "A" ? "B" : "A";
            var filePath1 =
                $"{AppDomain.CurrentDomain.BaseDirectory}\\{Resources.Resources.StorageFolder}\\{outputFolderParentFolder}\\{Resources.Resources.OutputFolder}\\dark.jpg";
            var filePath = filePath1;
            return filePath;
        }
    }
}
