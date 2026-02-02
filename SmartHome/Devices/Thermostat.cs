using SmartHome.Events;

namespace SmartHome.Devices
{
    public class Thermostat : Device, ISensor
    {
        private decimal _currentTemperature;

        public decimal CurrentTemperature 
        { 
            get => _currentTemperature; 
            set
            {
                _currentTemperature = value;
                Log($"Thermostat '{Name}' current temperature changed to: {value}°C");
            }
        }

        public override string Type => "Thermostat";

        public Thermostat(string name) : base(name)
        {
            _currentTemperature = 20.0m;
        }

        public bool TryRead(out decimal value)
        {
            value = CurrentTemperature;
            return true;
        }

        public override string GetStatus()
        {
            return $"{Name}: [Текущая температура: {CurrentTemperature}°C]";
        }
    }
}