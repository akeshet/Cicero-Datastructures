using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DataStructures
{
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class LogicalChannel
    {
        private bool enabled;

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        //Hotkey for toggling the override value.
        public char hotkeyChar;

        //Hotkey for turning override on and off.
        public char overrideHotkeyChar;

        public bool overridden;

        public bool digitalOverrideValue;
        public double analogOverrideValue;

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public HardwareChannel hardwareChannel;

        /// <summary>
        /// True if this channel will actually be a toggling channel... ie when the buffer is generated, it will oscillate between 0 and 1.
        /// </summary>
        private bool togglingChannel;

        public bool TogglingChannel
        {
            get { return togglingChannel; }
            set { togglingChannel = value; }
        }

        public LogicalChannel()
        {
            name = "";
            description = "";
            hardwareChannel = HardwareChannel.Unassigned;
            enabled = true;
            overridden = false;
            digitalOverrideValue = false;
            analogOverrideValue = 0;
            togglingChannel = false;
        }

        /// <summary>
        /// Meaningfull only for analog logical channels. If true, then when running "output now" the analog channel
        /// gets its value from the end of the dwell word. Otherwise, it gets its value from the end of the output word.
        /// </summary>
        private bool analogChannelOutputNowUsesDwellWord;

        public bool AnalogChannelOutputNowUsesDwellWord
        {
            get { return analogChannelOutputNowUsesDwellWord; }
            set { analogChannelOutputNowUsesDwellWord = value; }
        }

        private bool doOverrideDigitalColor;

        public bool DoOverrideDigitalColor
        {
            get { return doOverrideDigitalColor; }
            set { doOverrideDigitalColor = value; }
        }
        private System.Drawing.Color overrideColor;

        public System.Drawing.Color OverrideColor
        {
            get { return overrideColor; }
            set { overrideColor = value; }
        }

    }
}
