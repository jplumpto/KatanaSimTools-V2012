using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ArduinoCalibration
{
    [XmlRoot("ConfigSettings")]
    public class ConfigSettings
    {
        #region public vars
        [XmlElement("ArduinoPort")]
        public string ArduinoPort;

        [XmlElement("ThrottleMin")]
        public int ThrottleMin;

        [XmlElement("ThrottleMax")]
        public int ThrottleMax;

        [XmlElement("ThrottleInvert")]
        public int ThrottleInvert;

        [XmlElement("PitchMin")]
        public int PitchMin;

        [XmlElement("PitchMax")]
        public int PitchMax;

        [XmlElement("PitchInvert")]
        public int PitchInvert;

        [XmlElement("RollMin")]
        public int RollMin;

        [XmlElement("RollMax")]
        public int RollMax;

        [XmlElement("RollInvert")]
        public int RollInvert;

        [XmlElement("YawMin")]
        public int YawMin;

        [XmlElement("YawMax")]
        public int YawMax;

        [XmlElement("YawInvert")]
        public int YawInvert;

        [XmlElement("PropSpeedMin")]
        public int PropSpeedMin;

        [XmlElement("PropSpeedMax")]
        public int PropSpeedMax;
        
        [XmlElement("PropSpeedInvert")]
        public int PropSpeedInvert;

        [XmlElement("CarbHeatMin")]
        public int CarbHeatMin;

        [XmlElement("CarbHeatMax")]
        public int CarbHeatMax;
        
        [XmlElement("CarbHeatInvert")]
        public int CarbHeatInvert;

        [XmlElement("ParkBrakeMin")]
        public int ParkBrakeMin;

        [XmlElement("ParkBrakeMax")]
        public int ParkBrakeMax;

        [XmlElement("ParkBrakeInvert")]
        public int ParkBrakeInvert;

        [XmlElement("LeftBrakeMin")]
        public int LeftBrakeMin;

        [XmlElement("LeftBrakeMax")]
        public int LeftBrakeMax;

        [XmlElement("LeftBrakeInvert")]
        public int LeftBrakeInvert;

        [XmlElement("RightBrakeMin")]
        public int RightBrakeMin;

        [XmlElement("RightBrakeMax")]
        public int RightBrakeMax;

        [XmlElement("RightBrakeInvert")]
        public int RightBrakeInvert;


        [XmlElement("ChokeMin")]
        public int ChokeMin;

        [XmlElement("ChokeMax")]
        public int ChokeMax;

        [XmlElement("ChokeInvert")]
        public int ChokeInvert;

        #endregion

        #region ctor
        public ConfigSettings()
        {
            ArduinoPort = "COM1";
            ThrottleMin = 0;
            ThrottleMax = 1024;
            ThrottleInvert = 0;
            PitchMin = 0;
            PitchMax = 1024;
            PitchInvert = 0;
            RollMin = 0;
            RollMax = 1024;
            RollInvert = 0;
            YawMin = 0;
            YawMax = 1024;
            YawInvert = 0;
            PropSpeedMin = 0;
            PropSpeedMax = 1024;
            PropSpeedInvert = 0;
            CarbHeatMin = 0;
            CarbHeatMax = 1024;
            CarbHeatInvert = 0;
            ParkBrakeMin = 0;
            ParkBrakeMax = 1024;
            ParkBrakeInvert = 0;
            ChokeMin = 0;
            ChokeMax = 1024;
            ChokeInvert = 0;
            LeftBrakeMin = 0;
            LeftBrakeMax = 1024;
            LeftBrakeInvert = 0;
            RightBrakeMin = 0;
            RightBrakeMax = 1024;
            RightBrakeInvert = 0;
        }
        #endregion


        public void Serialize(string fname)
        {
            XmlSerializer s = new XmlSerializer(typeof(ConfigSettings));
            TextWriter w = new StreamWriter(fname);
            s.Serialize(w, this);
            w.Close();
        }

        public static ConfigSettings DeSerialize(string fname)
        {
            ConfigSettings newSettings = null;
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(ConfigSettings));
                TextReader r = new StreamReader(fname);
                newSettings = (ConfigSettings)s.Deserialize(r);
                r.Close();
            }
            catch (System.Exception ex)
            {
                newSettings = null;
                throw ex;
            }

            return newSettings;
        }
    }


    
}
