using System;
using SmartHome.Events;

namespace SmartHome.Devices
{
    public class Light : Device, IControllable
    {
        private int _brightness;
        private bool _desiredIsOn;
        private int _desiredBrightness;

        public new bool IsOn 
        { 
            get => base.IsOn; 
            private set
            {
                if (value) base.TurnOn();
                else base.TurnOff();
                if (!value) _brightness = 0;
            }
        }

        public int Brightness 
        { 
            get => _brightness; 
            private set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("Brightness must be between 0 and 100");
                
                _brightness = value;
                if (value > 0) IsOn = true;
            }
        }

        public bool DesiredIsOn 
        { 
            get => _desiredIsOn; 
            set
            {
                _desiredIsOn = value;
                if (!value) _desiredBrightness = 0;
                Log($"Light '{Name}' desired state changed to: {(value ? "ON" : "OFF")}");
            }
        }

        public int DesiredBrightness 
        { 
            get => _desiredBrightness; 
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentOutOfRangeException("Brightness must be between 0 and 100");
                
                _desiredBrightness = value;
                if (value > 0) _desiredIsOn = true;
                Log($"Light '{Name}' desired brightness changed to: {value}%");
            }
        }

        public override string Type => "Light";

        public Light(string name) : base(name)
        {
            _brightness = 0;
            _desiredIsOn = false;
            _desiredBrightness = 0;
        }

        public void ApplyAutomation()
        {
            // Моментальное применение желаемых настроек
            if (IsOn != DesiredIsOn)
            {
                IsOn = DesiredIsOn;
            }

            if (Brightness != DesiredBrightness && DesiredIsOn)
            {
                Brightness = DesiredBrightness;
            }

            Log($"Light '{Name}' automation applied: State={IsOn}, Brightness={Brightness}%");
        }

        public override string GetStatus()
        {
            var status = IsOn ? $"ON, {Brightness}%" : "OFF";
            var desiredStatus = DesiredIsOn ? $"ON, {DesiredBrightness}%" : "OFF";
            return $"{Name}: [Текущее: {status}] [Желаемое: {desiredStatus}]";
        }

        public override void TurnOn()
        {
            IsOn = true;
            Brightness = 50;
        }

        public override void TurnOff()
        {
            IsOn = false;
        }
    }
}