﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public struct Timer
    {
        [field: SerializeField]
        public float TimeToTrigger { get; set; }

        public float CurrentTime { get; set; }
        public float Progress
        {
            get => CurrentTime / TimeToTrigger;
            set
            {
                CurrentTime = value * TimeToTrigger;
            }
        }
        public bool IsTriggering => CurrentTime >= TimeToTrigger;


        public Timer(float timeToTrigger) : this()
        {
            TimeToTrigger = timeToTrigger;
        }


        public bool Update(bool autoRestart = false)
        {
            return Update(Time.deltaTime, autoRestart);
        }

        public bool FixedUpdate(bool autoRestart = false)
        {
            return Update(Time.fixedDeltaTime, autoRestart);
        }

        public bool Update(float deltaTime, bool autoRestart = false)
        {
            CurrentTime += deltaTime;

            var result = IsTriggering;

            if (result && autoRestart)
                Restart();

            return result;
        }

        public void Restart()
        {
            CurrentTime = 0f;
        }


        public YieldInstruction ToYield()
        {
            return new WaitForSeconds(TimeToTrigger);
        }
    }
}
