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
using System.Collections;
using HoloToolkit.Unity;
using UnityEngine;

namespace Assets.Scripts
{
    public class WhiteBoardFrameColorManager : Singleton<WhiteBoardFrameColorManager>
    {
        public Material RedMaterial;
        public Material BlueMaterial;
        public Material GreenMaterial;

        public GameObject WhiteBoardFrame;

        private const float MinimumDistance = 1.0f;
        private const float MaximumDistance = 2f;
        private const float RotationDeviationTolerance = 0.03f;

        private bool _changingFrameColorLocked;

        void Update()
        {
            if (!_changingFrameColorLocked)
            {
                ChangeFrameMaterial(PositionIsGoodForSending ? GreenMaterial : RedMaterial);
            }
        }

        private bool PositionIsGoodForSending
        {
            get
            {
                return RotationIsSimilarToMainCamera && DistanceIsGood;
            }
        }

        private bool DistanceIsGood
        {
            get
            {
                var distance = Vector3.Distance(WhiteBoardFrame.transform.position, Camera.main.transform.position);
                return distance >= MinimumDistance && distance <= MaximumDistance;
            }
        }

        private bool RotationIsSimilarToMainCamera
        {
            get
            {
                return Math.Abs(Camera.main.transform.rotation.x - WhiteBoardFrame.transform.rotation.x) < RotationDeviationTolerance &&
                       Math.Abs(Camera.main.transform.rotation.y - WhiteBoardFrame.transform.rotation.y) < RotationDeviationTolerance &&
                       Math.Abs(Camera.main.transform.rotation.z - WhiteBoardFrame.transform.rotation.z) < RotationDeviationTolerance;
            }
        }

        public void UpdateSent()
        {
            StartCoroutine(ShowBlueFrame());
        }

        public IEnumerator ShowBlueFrame()
        {
            _changingFrameColorLocked = true;
            ChangeFrameMaterial(BlueMaterial);
            yield return new WaitForSeconds(5f);
            _changingFrameColorLocked = false;
        }

        private void ChangeFrameMaterial(Material frameMaterial)
        {
            WhiteBoardFrame.GetComponent<MeshRenderer>().material = frameMaterial;
        }
    }
}
