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
using Assets.Enums;
using UnityEngine;

namespace Assets.Scripts
{
    public class WhiteBoardBehaviour : MonoBehaviour
    {
        private bool _getTriggeredAutomatically;

        private void Start()
        {
            InvokeRepeating("Tick", 0, 10);
        }

        public void Tick()
        {
            if (!PositioningManager.Instance.PositioningInProgress)
            {
                _getTriggeredAutomatically = true;
                GetLastPicture();
            }
        }

        public void GetLastPicture()
        {
            if (!ConnectionManager.Instance.Pin.HasValue)
            {
                return;
            }
            
            var url = WhiteBoardType.Instance.CurrentBoard == BoardType.White
                ? string.Format(Resources.Constants.GetImageUrl, Resources.Constants.ApplicationUrl, ConnectionManager.Instance.Pin.Value, ConnectionManager.Instance.ParticipantOrder)
                : string.Format(Resources.Constants.GetDarkImageUrl, Resources.Constants.ApplicationUrl, ConnectionManager.Instance.Pin.Value, ConnectionManager.Instance.ParticipantOrder);

            StartCoroutine(GetImageFromUrl(url));
        }

        private IEnumerator GetImageFromUrl(string url)
        {
            var www = new WWW(url);

            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                if (!_getTriggeredAutomatically)
                {
                    UserOutputManager.Instance.ShowOutput(www.text);
                }
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material.mainTexture = www.texture;
            }

            _getTriggeredAutomatically = false;
        }
    }
}
