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
using Assets.Resources;
using HoloToolkit.Unity;
using UnityEngine;

namespace Assets.Scripts
{
    public class ConnectionManager : Singleton<ConnectionManager>
    {
        public int? Pin { get; private set; }

        public GameObject UserInputObjects;
        public UserOutputManager UserOutputManager;

        public ParticipantOrder ParticipantOrder { get; private set; }

        public bool Connected
        {
            get { return Pin.HasValue; }
        }

        void Start()
        {
            SwitchInputObjectsActiveState(false);
        }

        public void Connect()
        {
            SwitchInputObjectsActiveState(true);
        }

        public void StartSession()
        {
            StartCoroutine(GetPin());
        }

        public void EndSession()
        {
            StartCoroutine(EndSessionCoroutine());
        }

        private IEnumerator EndSessionCoroutine()
        {
            var www = new WWW(string.Format(Constants.EndSessionUrl, Constants.ApplicationUrl, Pin));

            yield return www;
        }

        private IEnumerator GetPin()
        {
            var www = new WWW(string.Format(Constants.StartSessionUrl, Constants.ApplicationUrl));

            yield return www;

            Pin = int.Parse(www.text);

            SwitchInputObjectsActiveState(false);
            ShowPin();

            ParticipantOrder = ParticipantOrder.A;
        }

        public void ConnectWithPin(int pin)
        {
            Pin = pin;
            StartCoroutine(ConnectWithPin());
        }

        private IEnumerator ConnectWithPin()
        {
            var www = new WWW(string.Format(Constants.ConnectToExistingSessionUrl, Constants.ApplicationUrl, Pin));

            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                UserOutputManager.ShowOutput("Error");

            }
            else
            {
                SwitchInputObjectsActiveState(false);
                UserOutputManager.ShowOutput("Connected");

                ParticipantOrder = ParticipantOrder.B;
            }
        }

        public void ShowPin()
        {
            UserOutputManager.ShowOutput(Pin.HasValue ? string.Format("Session pin is {0}", Pin) : "No pin received");
        }

        public void SwitchInputObjectsActiveState(bool activeState)
        {
            foreach (Transform child in UserInputObjects.transform)
            {
                child.gameObject.SetActive(activeState);
            }
        }

        private void OnApplicationQuit()
        {
            EndSession();
        }

        public bool IsConnectedToInternet
        {
            get { return Application.internetReachability != NetworkReachability.NotReachable; }
        }
    }
}
