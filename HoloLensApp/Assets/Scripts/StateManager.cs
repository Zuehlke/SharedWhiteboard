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

using Assets.Enums;
using HoloToolkit.Unity;

namespace Assets.Scripts
{
    public class StateManager : Singleton<StateManager>
    {
        public State CurrentState { get; private set; }

        private State? _previousState;

        protected override void Awake()
        {
            base.Awake();

            if (!ConnectionManager.Instance.IsConnectedToInternet)
            {
                UserOutputManager.Instance.ShowPermanentOutput("No internet connection. \r\nPlease connect to internet and restart application.");
                return;
            }

            CurrentState = State.ChooseType;
            _previousState = null;
        }

        public void White()
        {
            if (CurrentState == State.Free || CurrentState == State.ChooseType)
            {
                WhiteBoardType.Instance.SetWhite();
                
                SwitchState(CurrentState == State.ChooseType ? State.Positioning : State.Free);

                if (CurrentState == State.Positioning)
                {
                    PositioningManager.Instance.Position();
                }
            }
        }

        public void Transparent()
        {
            if (CurrentState == State.Free || CurrentState == State.ChooseType)
            {
                WhiteBoardType.Instance.SetTransparent();

                SwitchState(CurrentState == State.ChooseType ? State.Positioning : State.Free);

                if (CurrentState == State.Positioning)
                {
                    PositioningManager.Instance.Position();
                }
            }
        }

        public void Adjust()
        {
            if (CurrentState == State.Free)
            {
                PositioningManager.Instance.Position();

                SwitchState(State.Positioning);
            }
        }

        public void DonePositioning()
        {
            if (CurrentState == State.Positioning)
            {
                PositioningManager.Instance.Done();

                if (_previousState == State.ChooseType)
                {
                    SwitchState(State.Pairing);

                    ConnectionManager.Instance.Connect();
                }
                else if (_previousState == State.Free)
                {
                    SwitchState(State.Free);
                }
            }
        }

        public void StartSession()
        {
            if (CurrentState == State.Pairing)
            {
                ConnectionManager.Instance.StartSession();

                SwitchState(State.Free);
            }
        }

        public void ConnectWithPin(int pin)
        {
            if (CurrentState == State.Pairing)
            {
                ConnectionManager.Instance.ConnectWithPin(pin);

                SwitchState(State.Free);
            }
        }

        public void Pair()
        {
            if (CurrentState == State.Free)
            {
                ConnectionManager.Instance.Connect();

                SwitchState(State.Pairing);
            }
        }

        private void SwitchState(State nextState)
        {
            _previousState = CurrentState;
            CurrentState = nextState;
        }

    }
}
