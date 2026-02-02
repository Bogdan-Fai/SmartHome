using System;

namespace SmartHome.Devices
{
    public class Heater : Device
    {
        private int _desiredTemperature;
        public int DesiredTemperature
        {
            get => _desiredTemperature;
            set
            {
                if (value >= 0 && value <= 30)
                {
                    _desiredTemperature = value;
                }
            }
        }

        public Heater(string name, int desiredTemperature) : base(name)
        {
            DesiredTemperature = desiredTemperature;
        }

        public void SetDesiredTemperature(int temperature)
        {
            DesiredTemperature = temperature;
        }

        public override string GetStatus()
        {
            return $"Обогреватель: [Желаемая температура: {DesiredTemperature}°C]";
        }

        public string GetHeaterStatus(int currentTemperature)
        {
            if (currentTemperature < DesiredTemperature)
            {
                return $"Обогревает до {DesiredTemperature}°C";
            }
            else if (currentTemperature > DesiredTemperature)
            {
                return $"Охлаждает до {DesiredTemperature}°C";
            }
            else
            {
                return $"Поддерживает {DesiredTemperature}°C";
            }
        }

        public override string Type => "Heater";

        // Удален метод ApplyAutomation, так как теперь температура меняется моментально через термостат
    }
}