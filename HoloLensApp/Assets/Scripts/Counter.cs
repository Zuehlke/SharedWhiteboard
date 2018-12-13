using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Counter: Singleton<Counter>
    {
        private double _counter;
        public Text CounterText;
        private Action _countdownFinished;
        private bool _countingInProgress;

        public void StartCountdown(Action countdownFinished)
        {
            _counter = 3;
            _countdownFinished = countdownFinished;
            gameObject.SetActive(true);
            _countingInProgress = true;
            CounterText.text = ((int)_counter).ToString();
        }

        public void Update()
        {
            _counter -= Time.deltaTime;

            if (_countingInProgress != true)
            {
                return;
            }

            var shownValue = Convert.ToInt32(_counter);

            if (shownValue > 0)
            {
                CounterText.text = shownValue.ToString();
            }
            else
            {
                CountdownFinished();
            }
        }

        private void CountdownFinished()
        {
            gameObject.SetActive(false);
            _countingInProgress = false;
            _countdownFinished();
        }
    }
}
