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
    public class RotationTextBehaviour : MonoBehaviour
    {
        public GameObject WhiteBoard;

        public Text WhiteBoardText;
        public Text CameraText;

        // Update is called once per frame
        void Update ()
        {
            CameraText.text = string.Format("Camera Pos ({0},{1},{2}), Rot ({3},{4},{5})",
                Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z,
                Camera.main.transform.rotation.x, Camera.main.transform.rotation.y, Camera.main.transform.rotation.z);

            WhiteBoardText.text = string.Format("WhiteBoard Pos ({0},{1},{2}), Rot ({3},{4},{5})",
                WhiteBoard.transform.position.x, WhiteBoard.transform.position.y, WhiteBoard.transform.position.z,
                WhiteBoard.transform.rotation.x, WhiteBoard.transform.rotation.y, WhiteBoard.transform.rotation.z);
        }
    }
}
