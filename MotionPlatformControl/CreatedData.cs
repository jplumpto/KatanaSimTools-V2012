using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MotionPlatformControl
{
    class CreatedData
    {
        #region Variables
        private const double GRAVITY = 0;
        private float f_lastTime = 0.0f;
        private const float f_PHASEOUTMAG = 0.75f;
        private bool sinusoidalFunction = true;
        #region ActiveBools
        private bool b_xActive = false;
        private bool b_yActive = false;
        private bool b_zActive = false;
        private bool b_rollActive = false;
        private bool b_pitchActive = false;
        private bool b_yawActive = false;
        #endregion 

        #region AmplitudesFrequencies
        private float f_xAmp = 0.0f;
        private float f_yAmp = 0.0f;
        private float f_zAmp = 0.0f;
        private float f_rollAmp = 0.0f;
        private float f_pitchAmp = 0.0f;
        private float f_yawAmp = 0.0f;
        private float f_xFreq = 0.0f;
        private float f_yFreq = 0.0f;
        private float f_zFreq = 0.0f;
        private float f_rollFreq = 0.0f;
        private float f_pitchFreq = 0.0f;
        private float f_yawFreq = 0.0f;
        private float f_xPhase = 0.0f;
        private float f_yPhase = 0.0f;
        private float f_zPhase = 0.0f;
        private float f_rollPhase = 0.0f;
        private float f_pitchPhase = 0.0f;
        private float f_yawPhase = 0.0f;
        #endregion

        MDACommand localStr = new MDACommand();
        #endregion

        public CreatedData()
        {
            localStr.MCW = 0x80; //New MDA accelerations
        }

        public void SetFunction(bool sinusoidal)
        {
            sinusoidalFunction = sinusoidal;
        }

        public void SetXStatus(bool active, float amplitude = 0.0f, float frequency = 0.0f, float phase = 0.0f)
        {
            b_xActive = active;
            if (active)
            {
                f_xAmp = amplitude;
                f_xFreq = frequency;
                f_xPhase = phase;
            }
        }

        public void SetYStatus(bool active, float amplitude = 0.0f, float frequency = 0.0f, float phase = 0.0f)
        {
            b_yActive = active;
            if (active)
            {
                f_yAmp = amplitude;
                f_yFreq = frequency;
                f_yPhase = phase;
            }
        }

        public void SetZStatus(bool active, float amplitude = 0.0f, float frequency = 0.0f, float phase = 0.0f)
        {
            b_zActive = active;
            if (active)
            {
                f_zAmp = amplitude;
                f_zFreq = frequency;
                f_zPhase = phase;
            }
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

        public MDACommand UpdateData(float f_timeSeconds)
        {
            float dt = f_timeSeconds - f_lastTime;

            if (b_xActive)
            {
                if (sinusoidalFunction)
                    localStr.a_x = f_xAmp * (float)Math.Sin(f_xFreq * f_timeSeconds - f_xPhase);
                else
                {
                    if (localStr.a_x < f_xAmp)
                    {
                        localStr.a_x += f_xFreq * dt;
                    }
                }
            }
            else if (0.2 < Math.Abs(localStr.a_x))
            {
                localStr.a_x *= (1.0f - f_PHASEOUTMAG * dt);
            }
            else
            {
                localStr.a_x = 0;
            }

            if (b_yActive)
            {
                if (sinusoidalFunction)
                    localStr.a_y = f_yAmp * (float)Math.Sin(f_yFreq * f_timeSeconds - f_yPhase);
                else
                {
                    if (localStr.a_y < f_yAmp)
                    {
                        localStr.a_y += f_yFreq * dt;
                    }
                }
            }
            else if (0.2 < Math.Abs(localStr.a_y))
            {
                localStr.a_y *= (1.0f - f_PHASEOUTMAG * dt);
            }
            else
            {
               localStr.a_y = 0;
            }

            if (b_zActive)
            {
                if (sinusoidalFunction)
                    localStr.a_z = f_zAmp * (float)Math.Sin(f_zFreq * f_timeSeconds - f_zPhase) + (float)GRAVITY;
                else
                {
                    if (localStr.a_z < f_zAmp)
                    {
                        localStr.a_z += f_zFreq * dt;
                    }
                }
            }
            else if (0.2 < Math.Abs(localStr.a_x - (float)GRAVITY))
            {
                localStr.a_z = (localStr.a_z - (float)GRAVITY) * (1.0f - f_PHASEOUTMAG * dt) + (float)GRAVITY;
            }
            else
            {
                localStr.a_z = (float)GRAVITY;
            }

            if (b_rollActive)
            {
                localStr.roll = f_rollAmp * (float)Math.Sin(f_rollFreq * f_timeSeconds - f_rollPhase);
                localStr.v_roll = f_rollAmp * f_rollFreq * (float)Math.Cos(f_rollFreq * f_timeSeconds - f_rollPhase);
                localStr.a_roll = -f_rollAmp * f_rollFreq * f_rollFreq * (float)Math.Sin(f_rollFreq * f_timeSeconds - f_rollPhase);
            }
            else if (0.05 < Math.Abs(localStr.a_roll))
            {

                f_rollAmp *= (1.0f - f_PHASEOUTMAG * dt);
                localStr.roll = f_rollAmp * (float)Math.Sin(f_rollFreq * f_timeSeconds - f_rollPhase);
                localStr.v_roll = f_rollAmp * f_rollFreq * (float)Math.Cos(f_rollFreq * f_timeSeconds - f_rollPhase);
                localStr.a_roll = -f_rollAmp * f_rollFreq * f_rollFreq * (float)Math.Sin(f_rollFreq * f_timeSeconds - f_rollPhase);
            }
            else
            {
                localStr.a_roll = 0;
                localStr.v_roll = 0;
                localStr.roll = 0;
            }

            if (b_pitchActive)
            {
                localStr.pitch = f_pitchAmp * (float)Math.Sin(f_pitchFreq * f_timeSeconds - f_pitchPhase);
                localStr.v_pitch = f_pitchAmp * f_pitchFreq * (float)Math.Cos(f_pitchFreq * f_timeSeconds - f_pitchPhase);
                localStr.a_pitch = -f_pitchAmp * f_pitchFreq * f_pitchFreq * (float)Math.Sin(f_pitchFreq * f_timeSeconds - f_pitchPhase);
            }
            else if (0.05 < Math.Abs(localStr.a_pitch))
            {

                f_pitchAmp *= (1.0f - f_PHASEOUTMAG * dt);
                localStr.pitch = f_pitchAmp * (float)Math.Sin(f_pitchFreq * f_timeSeconds - f_pitchPhase);
                localStr.v_pitch = f_pitchAmp * f_pitchFreq * (float)Math.Cos(f_pitchFreq * f_timeSeconds - f_pitchPhase);
                localStr.a_pitch = -f_pitchAmp * f_pitchFreq * f_pitchFreq * (float)Math.Sin(f_pitchFreq * f_timeSeconds - f_pitchPhase);
            }
            else
            {
                localStr.a_pitch = 0;
                localStr.v_pitch = 0;
                localStr.pitch = 0;
            }

            if (b_yawActive)
            {
                localStr.yaw = f_yawAmp * (float)Math.Sin(f_yawFreq * f_timeSeconds - f_yawPhase);
                localStr.v_yaw = f_yawAmp * f_yawFreq * (float)Math.Cos(f_yawFreq * f_timeSeconds - f_yawPhase);
                localStr.a_yaw = -f_yawAmp * f_yawFreq * f_yawFreq * (float)Math.Sin(f_yawFreq * f_timeSeconds - f_yawPhase);
            }
            else if (0.05 < Math.Abs(localStr.a_yaw))
            {

                f_yawAmp *= (1.0f - f_PHASEOUTMAG * dt);
                localStr.yaw = f_yawAmp * (float)Math.Sin(f_yawFreq * f_timeSeconds - f_yawPhase);
                localStr.v_yaw = f_yawAmp * f_yawFreq * (float)Math.Cos(f_yawFreq * f_timeSeconds - f_yawPhase);
                localStr.a_roll = -f_rollAmp * f_rollFreq * f_rollFreq * (float)Math.Sin(f_rollFreq * f_timeSeconds - f_rollPhase);
            }
            else
            {
                localStr.a_yaw = 0;
                localStr.v_yaw = 0;
                localStr.yaw = 0;
            }

            f_lastTime = f_timeSeconds;
            return localStr;
        }

        
    }
}
