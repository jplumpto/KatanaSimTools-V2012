using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlightControlInput
{
    class CreatedData
    {
        #region Variables
        private const double GRAVITY = 0;
        private float f_lastTime = 0.0f;
        private const float f_PHASEOUTMAG = 0.75f;
        #region ActiveBools
        private bool b_rollActive = false;
        private bool b_pitchActive = false;
        private bool b_yawActive = false;
        #endregion 

        #region AmplitudesFrequencies
        private float f_rollAmp = 0.0f;
        private float f_pitchAmp = 0.0f;
        private float f_yawAmp = 0.0f;
        private float f_rollFreq = 0.0f;
        private float f_pitchFreq = 0.0f;
        private float f_yawFreq = 0.0f;
        private float f_rollPhase = 0.0f;
        private float f_pitchPhase = 0.0f;
        private float f_yawPhase = 0.0f;
        #endregion

        private float rollInput = 0.0f;
        public float RollInput() { return rollInput; }

        private float pitchInput = 0.0f;
        public float PitchInput() { return pitchInput; }

        private float yawInput = 0.0f;
        public float YawInput() { return yawInput; }
        #endregion

        public CreatedData()
        {
            
        }

        public void SetRollStatus(bool active, float amplitude = 0.0f, float frequency = 0.0f, float phase = 0.0f)
        {
            b_rollActive = active;
            if (active)
            {
                f_rollAmp = amplitude;
                f_rollFreq = frequency;
                f_rollPhase = phase;
            }
        }

        public void SetPitchStatus(bool active, float amplitude = 0.0f, float frequency = 0.0f, float phase = 0.0f)
        {
            b_pitchActive = active;
            if (active)
            {
                f_pitchAmp = amplitude;
                f_pitchFreq = frequency;
                f_pitchPhase = phase;
            }
        }

        public void SetYawStatus(bool active, float amplitude = 0.0f, float frequency = 0.0f, float phase = 0.0f)
        {
            b_yawActive = active;
            if (active)
            {
                f_yawAmp = amplitude;
                f_yawFreq = frequency;
                f_yawPhase = phase;
            }
        }

        public byte [] UpdateData(float f_timeSeconds)
        {
            float dt = f_timeSeconds - f_lastTime;

            

            if (b_rollActive)
            {
                rollInput = f_rollAmp * (float)Math.Sin(f_rollFreq * f_timeSeconds - f_rollPhase);
            }
            else if (0.05 < Math.Abs(rollInput))
            {

                f_rollAmp *= (1.0f - f_PHASEOUTMAG * dt);
                rollInput = f_rollAmp * (float)Math.Sin(f_rollFreq * f_timeSeconds - f_rollPhase);
            }
            else
            {
                rollInput = 0;
            }

            if (b_pitchActive)
            {
                pitchInput = f_pitchAmp * (float)Math.Sin(f_pitchFreq * f_timeSeconds - f_pitchPhase);
            }
            else if (0.05 < Math.Abs(pitchInput))
            {

                f_pitchAmp *= (1.0f - f_PHASEOUTMAG * dt);
                pitchInput = f_pitchAmp * (float)Math.Sin(f_pitchFreq * f_timeSeconds - f_pitchPhase);
            }
            else
            {
                pitchInput = 0;
            }

            if (b_yawActive)
            {
                yawInput = f_yawAmp * (float)Math.Sin(f_yawFreq * f_timeSeconds - f_yawPhase);
            }
            else if (0.05 < Math.Abs(yawInput))
            {

                f_yawAmp *= (1.0f - f_PHASEOUTMAG * dt);
                yawInput = f_yawAmp * (float)Math.Sin(f_yawFreq * f_timeSeconds - f_yawPhase);
            }
            else
            {
                yawInput = 0;
            }
            f_lastTime = f_timeSeconds;

            float [] floatArray = new float[]{rollInput, pitchInput, yawInput};
            byte[] array = new byte[floatArray.Length * 4]; //3 Floats x 4 bytes / float
            Buffer.BlockCopy(floatArray, 0, array, 0, array.Length);
            return array;

            
        }

        
    }
}
