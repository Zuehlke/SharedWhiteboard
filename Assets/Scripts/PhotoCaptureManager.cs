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

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.VR.WSA.WebCam;

namespace Assets.Scripts
{
    [SuppressMessage("ReSharper", "PossibleLossOfFraction")]
    public class PhotoCaptureManager : Singleton<PhotoCaptureManager>
    {
        private PhotoCapture _capturedPhotoObject;
        private Texture2D _targetTexture;

        public UserOutputManager UserOutputManager;

        public GameObject WhiteBoard;

        public void CapturePhoto()
        {
            PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
        }

        private void OnPhotoCaptureCreated(PhotoCapture captureobject)
        {
            _capturedPhotoObject = captureobject;
            var cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending(res => res.width * res.height).First();

            _targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

            var cameraParameters = new CameraParameters
            {
                hologramOpacity = 0.0f,
                cameraResolutionWidth = cameraResolution.width,
                cameraResolutionHeight = cameraResolution.height,
                pixelFormat = CapturePixelFormat.BGRA32
            };

            captureobject.StartPhotoModeAsync(cameraParameters, OnPhotoModeStarted);
        }

        private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
        {
            if (result.success)
            {
                WhiteBoardFrameColorManager.Instance.UpdateSent();

                _capturedPhotoObject.TakePhotoAsync(OnPhotoCapturedToMemory);
            }
        }

        private void OnPhotoCapturedToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
        {
            if (result.success)
            {
                photoCaptureFrame.UploadImageDataToTexture(_targetTexture);

                var picture = _targetTexture.EncodeToJPG();

                StartCoroutine(UploadPhoto(picture));

                _capturedPhotoObject.StopPhotoModeAsync(OnPhotoModeStopped);
            }
        }

        private static IEnumerator UploadPhoto(byte[] picture)
        {
            if (!ConnectionManager.Instance.Pin.HasValue)
            {
                yield break;
            }

            var url = string.Format(Resources.Constants.ImageUploadUrl, Resources.Constants.ApplicationUrl, ConnectionManager.Instance.Pin.Value, ConnectionManager.Instance.ParticipantOrder);

            var www = new WWW(url, picture);

            yield return www;
        }

        private void OnPhotoModeStopped(PhotoCapture.PhotoCaptureResult result)
        {
            _capturedPhotoObject.Dispose();
            _capturedPhotoObject = null;
        }
    }
}
