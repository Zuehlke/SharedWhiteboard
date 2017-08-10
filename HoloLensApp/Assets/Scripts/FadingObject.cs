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
    public class FadingObject : MonoBehaviour
    {
        private bool _fadeIn;
        private bool _fadeOut;
        private float _fadingPerSecond;
        
        private Color _originalColor;

        protected void Start()
        {
            _originalColor = gameObject.GetComponent<Text>().color;
            gameObject.GetComponent<Text>().color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0);
        }

        protected void Update()
        {
            var currentColor = gameObject.GetComponent<Text>().color;
            var newColor = currentColor;

            if (_fadeIn)
            {
                newColor = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a + _fadingPerSecond * Time.deltaTime);

                if (newColor.a >= _originalColor.a)
                {
                    _fadeIn = false;
                }
            }
            else if (_fadeOut)
            {
                newColor = new Color(currentColor.r, currentColor.g, currentColor.b,
                    currentColor.a - _fadingPerSecond * Time.deltaTime);

                if (newColor.a <= 0)
                {
                    _fadeOut = false;
                }

            }

            gameObject.GetComponent<Text>().color = newColor;
        }

        public void FadeIn(float fadingTimeInSeconds)
        {
            _fadeIn = true;
            _fadeOut = false;

            _fadingPerSecond = _originalColor.a / fadingTimeInSeconds;
        }

        public void FadeOut(float fadingTimeInSeconds)
        {
            _fadeOut = true;
            _fadeIn = false;

            _fadingPerSecond = _originalColor.a / fadingTimeInSeconds;
        }
    }
}
