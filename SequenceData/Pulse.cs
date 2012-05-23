using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace DataStructures
{

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class Pulse
    {

        public static bool Equivalent(Pulse a, Pulse b)
        {
            if (a.endCondition != b.endCondition)
                return false;
            if (!DimensionedParameter.Equivalent(a.endDelay, b.endDelay))
                return false;
            if (a.endDelayed != b.endDelayed)
                return false;
            if (a.endDelayEnabled != b.endDelayEnabled)
                return false;
            if (a.PulseDescription != b.PulseDescription)
                return false;
            if (!DimensionedParameter.Equivalent(a.pulseDuration, b.pulseDuration))
                return false;
            if (a.PulseName != b.PulseName)
                return false;
            if (a.PulseValue != b.PulseValue)
                return false;
            if (a.startCondition != b.startCondition)
                return false;
            if (!DimensionedParameter.Equivalent(a.startDelay ,b.startDelay))
                return false;
            if (a.startDelayed != b.startDelayed)
                return false;
            if (a.startDelayEnabled != b.startDelayEnabled)
                return false;
            if (a.ValueFromVariable != b.ValueFromVariable)
                return false;
            if (a.ValueVariable != b.ValueVariable)
                return false;

            return true;
        }

        private bool valueFromVariable;

        public bool ValueFromVariable
        {
            get { return valueFromVariable; }
            set { 
                valueFromVariable = value;
                if (valueFromVariable == false)
                {
                    ValueVariable = null;
                }
            }
        }
        private Variable valueVariable;

        public Variable ValueVariable
        {
            get { return valueVariable; }
            set { valueVariable = value; }
        }

        public Pulse(Pulse copyMe)
        {
            this.endCondition = copyMe.endCondition;
            this.endDelay = new DimensionedParameter(copyMe.endDelay);
            this.endDelayed = copyMe.endDelayed;
            this.endDelayEnabled = copyMe.endDelayEnabled;
            this.pulseDescription = copyMe.pulseDescription;
            this.pulseDuration = new DimensionedParameter(copyMe.pulseDuration);
            this.pulseName = "Copy of " + copyMe.pulseName;
            this.pulseValue = copyMe.pulseValue;
            this.startCondition = copyMe.startCondition;
            this.startDelay = new DimensionedParameter(copyMe.startDelay);
            this.startDelayed = copyMe.startDelayed;
            this.startDelayEnabled = copyMe.startDelayEnabled;
        }

        private string pulseName;

        public string PulseName
        {
            get {
                if (pulseName == null)
                {
                    pulseName = "";
                }
                return pulseName; }
            set { pulseName = value; }
        }
        private string pulseDescription;

        public string PulseDescription
        {
            get {
                if (pulseDescription == null)
                    pulseDescription = "";
                return pulseDescription; 
            }
            set { pulseDescription = value; }
        }

        public Dictionary<Variable, string> usedVariables()
        {
            Dictionary<Variable, string> ans = new Dictionary<Variable, string>();

            if (startDelay.parameter.variable != null)
            {
                ans.Add(startDelay.parameter.variable, "start pretrig/delay.");
            }

            if (endDelay.parameter.variable != null)
            {
                if (!ans.ContainsKey(endDelay.parameter.variable))
                {
                    ans.Add(endDelay.parameter.variable, "end pretrig/delay.");
                }
            }

            if (pulseDuration.parameter.variable != null)
            {
                if (!ans.ContainsKey(pulseDuration.parameter.variable))
                {
                    ans.Add(pulseDuration.parameter.variable, "duration.");
                }
            }

            if (ValueFromVariable)
            {
                if (ValueVariable != null)
                {
                    if (!ans.ContainsKey(ValueVariable))
                    {
                        ans.Add(ValueVariable, "pulse value.");
                    }
                }
            }

            return ans;
        }


        public enum PulseTimingCondition { TimestepStart, TimestepEnd, Duration };

        public PulseTimingCondition startCondition;

        public PulseTimingCondition endCondition;

        /// <summary>
        /// True is startDelay means start in advance.
        /// False is startDelay means start delayed.
        /// </summary>
        public bool startDelayed;

        public bool startDelayEnabled;

        public DimensionedParameter startDelay;

        /// <summary>
        /// True if startDelay means start in advance.
        /// False is startDelay means start delayed.
        /// </summary>
        /// 
        public bool endDelayed;

        public DimensionedParameter endDelay;

        public bool endDelayEnabled;

        public DimensionedParameter pulseDuration;

        private bool pulseValue;

        public bool PulseValue
        {
            get {
                if (!ValueFromVariable)
                {
                    return pulseValue;
                }
                else {
                    if (ValueVariable==null)
                        return false;
                    if (ValueVariable.VariableValue!=0)
                        return true;
                    return false;
                }
            }
            set { pulseValue = value; }
        }

        public Pulse()
        {
            this.PulseName = "Unnamed";
            this.endCondition = PulseTimingCondition.TimestepEnd;
            this.endDelay = new DimensionedParameter(Units.s, 0);
            this.endDelayed = false;
            this.endDelayEnabled = false;

            this.pulseDuration = new DimensionedParameter(Units.s, 0);

            this.pulseValue = true;

            this.startCondition = PulseTimingCondition.TimestepStart;
            this.startDelay = new DimensionedParameter(Units.s, 0);
            this.startDelayed = false;
            this.startDelayEnabled = false;
            
        }


        public override string ToString()
        {
            return PulseName;
        }

        public string dataInvalidUICue()
        {
            if (startCondition == endCondition)
            {
                return "Cannot have same condition for both start and end.";
            }

            if (startCondition == PulseTimingCondition.Duration)
            {
                if (endCondition == PulseTimingCondition.Duration)
                {
                    return "Cannot have duration condition for both start and end.";
                }
            }

            if (startCondition == PulseTimingCondition.TimestepEnd)
            {
                if (endCondition == PulseTimingCondition.TimestepStart)
                {
                    return "Cannot have end before start.";
                }

                if (endCondition == PulseTimingCondition.TimestepEnd)
                {
                    return "Cannot have start and end at the same time.";
                }
            }

            if (startCondition == PulseTimingCondition.TimestepStart)
            {
                if (endCondition == PulseTimingCondition.TimestepStart)
                {
                    return "Cannot have start and end at the same time.";
                }
            }

            

            return null;
        }

        public bool dataValid()
        {
            if (dataInvalidUICue() != null)
                return false;

            return true;
        }

        /// <summary>
        /// Used in calculating buffers in the presence of digital pulses.
        /// </summary>
        public class PulseSampleTimes
        {
            /// <summary>
            /// Sample at which the pulse starts, relative to the beginning of the timestep its in.
            /// </summary>
            public int startSample;
            /// <summary>
            /// Sample at which the pulse ends, relative to the beginning of the timestep its in.
            /// </summary>
            public int endSample;

            /// <summary>
            /// True if startSample is neither 0 nor the number for sample in the sequence timestep.
            /// </summary>
            public bool startRequiresImpingement;

            /// <summary>
            /// True is endSample is neither 0 nor the number of samples in the sequence timestep.
            /// </summary>
            public bool endRequiresImpingement;
        }

        public PulseSampleTimes getPulseSampleTimes(double remainderTime, double sampleDuration, double sequenceTimestepDuration)
        {
            double remainderTimeAtEnd = remainderTime;
            int nSamplesInSequenceTimestep = 0;
            SequenceData.computeNSamplesAndRemainderTime(ref nSamplesInSequenceTimestep, ref remainderTimeAtEnd, sequenceTimestepDuration, sampleDuration);
            return getPulseSampleTimes(nSamplesInSequenceTimestep, sampleDuration);
        }

 
        public PulseSampleTimes getPulseSampleTimes(int nSamplesInSequenceTimestep, double sampleDuration )
        {
            if (!dataValid())
                throw new InvalidDataException("This pulse is invalid.");


            PulseSampleTimes ans = new PulseSampleTimes();

            if (startCondition == PulseTimingCondition.TimestepStart)
            {
                ans.startSample = 0;

                if (startDelayEnabled)
                {
                    int delaySamples = (int)(0.5 + startDelay.getBaseValue() / sampleDuration);
                    if (startDelayed) {
                        ans.startSample+=delaySamples;
                    }
                    else {
                        ans.startSample-=delaySamples;
                    }
                }
            }

            if (startCondition == PulseTimingCondition.TimestepEnd)
            {
                ans.startSample = nSamplesInSequenceTimestep;
                if (startDelayEnabled)
                {
                    int delaySamples = (int)(0.5 + startDelay.getBaseValue() / sampleDuration);
                    if (startDelayed) {
                        ans.startSample+=delaySamples;
                    }
                    else {
                        ans.startSample-=delaySamples;
                    }
                }
            }

            if (endCondition == PulseTimingCondition.TimestepStart)
            {
                ans.endSample = 0;

                if (endDelayEnabled)
                {
                    int delaySamples = (int)(0.5 + endDelay.getBaseValue() / sampleDuration);
                    if (endDelayed) {
                        ans.endSample+=delaySamples;
                    }
                    else {
                        ans.endSample-=delaySamples;
                    }
                }
            }

            if (endCondition == PulseTimingCondition.TimestepEnd)
            {
                ans.endSample = nSamplesInSequenceTimestep;

                if (endDelayEnabled)
                {
                    int delaySamples = (int)(0.5 + endDelay.getBaseValue() / sampleDuration);
                    if (endDelayed) {
                        ans.endSample+=delaySamples;
                    }
                    else {
                        ans.endSample-=delaySamples;
                    }
                }
            }

            if (endCondition == PulseTimingCondition.Duration)
            {
                ans.endSample = ans.startSample + (int)(0.5 + pulseDuration.getBaseValue() / sampleDuration);
            }

            if (startCondition == PulseTimingCondition.Duration)
            {
                ans.startSample = ans.endSample - (int)(0.5 + pulseDuration.getBaseValue() / sampleDuration);
            }

            if (ans.startSample!=0 && ans.startSample!=nSamplesInSequenceTimestep) {
                ans.startRequiresImpingement = true;
            }

            if (ans.endSample!=0 && ans.endSample!=nSamplesInSequenceTimestep) {
                ans.endRequiresImpingement = true;
            }

            return ans;

        }
    }
}
