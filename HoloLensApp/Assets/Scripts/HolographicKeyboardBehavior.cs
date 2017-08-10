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

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HolographicKeyboardBehavior : MonoBehaviour
    {
        public InputField KeyboardInputField;

        private string _keyboardText;

        protected void Start()
        {
            _keyboardText = string.Empty;
            KeyboardInputField.text = _keyboardText;
        }

        public void NumberButtonClicked(int number)
        {
            _keyboardText += number;
            KeyboardInputField.text = _keyboardText;
        }

        public void BackspaceButtonClicked()
        {
            if (_keyboardText.Length > 0)
            {
                _keyboardText = _keyboardText.Substring(0, _keyboardText.Length - 1);
                KeyboardInputField.text = _keyboardText;
            }
        }

        public void OkButtonClicked()
        {
            if (_keyboardText.Length > 0)
            {
                var pin = int.Parse(_keyboardText);
                StateManager.Instance.ConnectWithPin(pin);
            }
        }
    }
}
