using System;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlightControlInput
{
    class FlightTestData
    {
        #region Variables
        private const uint n_fileRowCount = 1024 * 2 * 2 * 2;
        private int n_lastTimeIndex = 0;
        private int n_dataPoints = 0;
        private float f_maxTime = 0.0f;
        private float f_startTime = 0.0f;

        private float[] f_timeInputArray = new float[n_fileRowCount];
        private float[] f_rollInputArray = new float[n_fileRowCount];
        private float[] f_pitchInputArray = new float[n_fileRowCount];
        private float[] f_yawInputArray = new float[n_fileRowCount];

        private byte[] b_lastestInputArray = new byte[3 * 4]; //3 Floats x 4 bytes / float
        public byte[] LatestInputArray() { return b_lastestInputArray; }
        #endregion

        #region Constructor
        public FlightTestData()
        {
        }

        public void Dispose()
        {
            
        }
        #endregion //Constructor


        #region ParsingData
        public bool ParseData(string filename)
        {
            using (CsvReader csv = new CsvReader(new StreamReader(filename), true))
            {
                int fieldCount = csv.FieldCount;

                int rollFieldIndex = csv.GetFieldIndex("Roll Input (ratio)");
                int pitchFieldIndex = csv.GetFieldIndex("Pitch Input (ratio)");
                int yawFieldIndex = csv.GetFieldIndex("Yaw Input (ratio)");

                if (rollFieldIndex == -1 || pitchFieldIndex == -1 || yawFieldIndex == -1)
                {
                    return false;
                }

                int currentIndex = 0;
                
                while (csv.ReadNextRecord() && currentIndex < n_fileRowCount)
                {
                    f_timeInputArray[currentIndex] = float.Parse(csv[0]);
                    f_rollInputArray[currentIndex] = float.Parse(csv[rollFieldIndex]);
                    f_pitchInputArray[currentIndex] = float.Parse(csv[pitchFieldIndex]);
                    f_yawInputArray[currentIndex] = float.Parse(csv[yawFieldIndex]);
                    f_maxTime = f_timeInputArray[currentIndex];
                    currentIndex++;
                }
                f_startTime = f_timeInputArray[0];
                n_dataPoints = currentIndex;
            }

            return true;
        }
        #endregion

        #region InterpolateData
        public bool UpdateData(float elapsedTime)
        {
            //First check elapsedTime doesn't exceed max elapsedTime
            if (elapsedTime + f_startTime >= f_maxTime)
            {
                return false;
            }

            //Need to find index of first interval where f_time[i] > elapsedTime
            int index = 0;

            for (index = n_lastTimeIndex; index < n_dataPoints; index++)
            {
                if (f_timeInputArray[index] > elapsedTime + f_startTime)
                {
                    break;
                }
            }

            n_lastTimeIndex = index;

            float rollInput = f_rollInputArray[index] + (elapsedTime - f_timeInputArray[index]) * (f_rollInputArray[index + 1] - f_rollInputArray[index]) / (f_timeInputArray[index + 1] - f_timeInputArray[index]);
            float pitchInput = f_pitchInputArray[index] + (elapsedTime - f_timeInputArray[index]) * (f_pitchInputArray[index + 1] - f_pitchInputArray[index]) / (f_timeInputArray[index + 1] - f_timeInputArray[index]);
            float yawInput = f_yawInputArray[index] + (elapsedTime - f_timeInputArray[index]) * (f_yawInputArray[index + 1] - f_yawInputArray[index]) / (f_timeInputArray[index + 1] - f_timeInputArray[index]);

            float[] floatArray = new float[] { rollInput, pitchInput, yawInput };
            Buffer.BlockCopy(floatArray, 0, b_lastestInputArray, 0, b_lastestInputArray.Length);
            
            return true;
        }

        #endregion








    }
}
