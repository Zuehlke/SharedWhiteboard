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

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class WhiteBoardFrameSizeManager : Singleton<WhiteBoardFrameSizeManager>
    {
        private const double WhiteBoardFrameOffset = 0.02f;

        public GameObject WhiteBoardFrame;

        public GameObject WidthInputObjects;
        public GameObject HeightInputObjects;

        public InputField HeightInput;
        public InputField WidthInput;

        private void Start()
        {
            SwitchInputObjectsActiveState(WidthInputObjects, false);
            SwitchInputObjectsActiveState(HeightInputObjects, false);
        }

        private void SetWidth(float width = 0.75f)
        {
            var currentScale = WhiteBoardFrame.transform.localScale;
            WhiteBoardFrame.transform.localScale = new Vector3((float) (width + WhiteBoardFrameOffset), currentScale.y, currentScale.z);
        }

        private void SetHeight(float height = 1.15f)
        {
            var currentScale = WhiteBoardFrame.transform.localScale;
            WhiteBoardFrame.transform.localScale = new Vector3(currentScale.x, (float)(height + WhiteBoardFrameOffset), currentScale.z);
        }

        public void StartResizing()
        {
            SwitchInputObjectsActiveState(WidthInputObjects, true);
        }

        private void SwitchInputObjectsActiveState(GameObject inputObjects, bool activeState)
        {
            foreach (Transform child in inputObjects.transform)
            {
                child.gameObject.SetActive(activeState);
            }
        }

        public void WidthEntered()
        {
            var text = WidthInput.text;
            if (text != string.Empty)
            {
                var widthInCentimeters = float.Parse(text);
                SetWidth(widthInCentimeters / 100);
            }
            else
            {
                SetDefaultWidth();
            }

            SwitchInputObjectsActiveState(WidthInputObjects, false);
            SwitchInputObjectsActiveState(HeightInputObjects, true);
        }

        private void SetDefaultWidth()
        {
            SetWidth();
        }

        public void HeightEntered()
        {
            var text = HeightInput.text;
            if (text != string.Empty)
            {
                var heightInCentimeters = float.Parse(text);
                SetHeight(heightInCentimeters / 100);
            }
            else
            {
                SetDefaultHeight();
            }

            SwitchInputObjectsActiveState(HeightInputObjects, false);
        }

        private void SetDefaultHeight()
        {
            SetHeight();
        }
    }
}
