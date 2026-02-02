using System;
using System.Collections.Generic;
using System.Linq;
using SmartHome.Devices;
using SmartHome.Rooms;
using SmartHome.Controllers;
using SmartHome.Scenes;
using SmartHome.Events;

namespace SmartHome
{
    class Program
    {
        private static List<Room> _rooms = new();
        private static HomeController _homeController = null!;
        private static List<SceneBase> _scenes = new();

        static void Main(string[] args)
        {
            InitializeSystem();
            DisplayMainMenu();
        }

        static void InitializeSystem()
        {
            // Создание комнат
            var livingRoom = new Room("Гостиная");
            var bedroom = new Room("Спальня");
            var kitchen = new Room("Кухня");

            // Добавление устройств в гостиную
            livingRoom.AddDevice(new Light("Основной свет"));
            livingRoom.AddDevice(new Thermostat("Термостат"));
            livingRoom.AddDevice(new Heater("Обогреватель", 22));

            // Добавление устройств в спальню
            bedroom.AddDevice(new Light("Прикроватный свет"));
            bedroom.AddDevice(new Thermostat("Термостат"));
            bedroom.AddDevice(new Heater("Обогреватель", 22));

            // Добавление устройств на кухню
            kitchen.AddDevice(new Light("Кухонный свет"));
            kitchen.AddDevice(new Thermostat("Термостат"));
            kitchen.AddDevice(new Heater("Обогреватель", 22));

            _rooms.Add(livingRoom);
            _rooms.Add(bedroom);
            _rooms.Add(kitchen);

            // Инициализация контроллера
            _homeController = new HomeController(_rooms);

            // Инициализация сцен
            _scenes = new List<SceneBase>
            {
                new MorningScene(),
                new DayScene(),
                new EveningScene(),
                new NightScene()
            };
        }

        static void DisplayMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== УМНЫЙ ДОМ ===");
                Console.WriteLine("1. Показать все состояния");
                Console.WriteLine("2. Управление комнатой");
                Console.WriteLine("3. Управление всеми девайсами");
                Console.WriteLine("4. Задать желаемые параметры дома");
                Console.WriteLine("5. Задать текущие параметры");
                Console.WriteLine("6. Сцены");
                Console.WriteLine("7. Включить автоматизацию");
                Console.WriteLine("8. Имитировать движение");
                Console.WriteLine("9. Включить все устройства в комнате"); // ← НОВЫЙ ПУНКТ
                Console.WriteLine("0. Выход");
                Console.Write("Выберите опцию: ");

                var input = Console.ReadLine();
                switch (input)
                {
                    case "1": ShowAllStatuses(); break;
                    case "2": ManageRoom(); break;
                    case "3": ManageAllDevices(); break;
                    case "4": SetGlobalDesiredSettings(); break;
                    case "5": SetCurrentParameters(); break;
                    case "6": ShowScenesMenu(); break;
                    case "7": RunAutomation(); break;
                    case "8": SimulateMotion(); break;
                    case "9": TurnAllOnInRoom(); break; // ← НОВЫЙ ПУНКТ
                    case "0": return;
                    default: ShowError("Неверный выбор"); break;
                }
            }
        }

        static void ShowAllStatuses()
        {
            Console.Clear();
            Console.WriteLine("=== СОСТОЯНИЕ ВСЕХ УСТРОЙСТВ ===");

            foreach (var room in _rooms)
            {
                room.DisplayStatus();
            }

            WaitForContinue();
        }

        static void ManageRoom()
        {
            Console.Clear();
            Console.WriteLine("=== УПРАВЛЕНИЕ КОМНАТОЙ ===");

            var room = SelectRoom();
            if (room == null) return;

            Console.WriteLine($"\nУправление комнатой: {room.Name}");
            Console.WriteLine("1. Управление светом");
            Console.WriteLine("2. Управление температурой");
            Console.Write("Выберите опцию: ");

            var input = Console.ReadLine();
            switch (input)
            {
                case "1": ManageRoomLight(room); break;
                case "2": ManageRoomTemperature(room); break;
                default: ShowError("Неверный выбор"); break;
            }
        }

        static void ManageRoomLight(Room room)
        {
            var light = room.GetDevices().OfType<Light>().FirstOrDefault();
            if (light == null)
            {
                ShowError("В этой комнате нет света");
                return;
            }

            Console.WriteLine($"\nУправление светом в {room.Name}");
            Console.WriteLine("1. Включить свет");
            Console.WriteLine("2. Выключить свет");
            Console.WriteLine("3. Установить яркость");
            Console.Write("Выберите опцию: ");

            var input = Console.ReadLine();
            try
            {
                switch (input)
                {
                    case "1":
                        light.DesiredIsOn = true;
                        ShowSuccess("Свет включен");
                        break;
                    case "2":
                        light.DesiredIsOn = false;
                        ShowSuccess("Свет выключен");
                        break;
                    case "3":
                        // Цикл с валидацией яркости
                        int brightness;
                        do
                        {
                            Console.Write("Введите яркость (0-100): ");
                            if (int.TryParse(Console.ReadLine(), out brightness) && brightness >= 0 && brightness <= 100)
                            {
                                break;
                            }
                            ShowError("Неверный формат яркости. Введите число от 0 до 100.");
                        } while (true);

                        light.DesiredBrightness = brightness;
                        ShowSuccess($"Яркость установлена на {brightness}%");
                        break;
                    default: ShowError("Неверный выбор"); break;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

            WaitForContinue();
        }

        static void ManageRoomTemperature(Room room)
        {
            var heater = room.GetDevices().OfType<Heater>().FirstOrDefault();
            if (heater == null)
            {
                ShowError("В этой комнате нет обогревателя");
                return;
            }

            Console.WriteLine($"\nУправление температурой в {room.Name}");
            
            // Цикл с валидацией температуры
            decimal temperature;
            do
            {
                Console.Write("Введите желаемую температуру (0-30°C): ");
                if (decimal.TryParse(Console.ReadLine(), out temperature) && temperature >= 0 && temperature <= 30)
                {
                    break;
                }
                ShowError("Неверный формат температуры. Введите число от 0 до 30.");
            } while (true);

            try
            {
                heater.DesiredTemperature = (int)temperature;
                ShowSuccess($"Желаемая температура установлена на {(int)temperature}°C");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

            WaitForContinue();
        }

        static void ManageAllDevices()
        {
            Console.Clear();
            Console.WriteLine("=== УПРАВЛЕНИЕ ВСЕМИ УСТРОЙСТВАМИ ===");
            Console.WriteLine("1. Установить одинаковую температуру для всех обогревателей");
            Console.WriteLine("2. Установить одинаковую яркость для всех светов");
            Console.WriteLine("3. Включить все устройства");
            Console.Write("Выберите опцию: ");

            var input = Console.ReadLine();
            try
            {
                switch (input)
                {
                    case "1":
                        // Цикл с валидацией температуры
                        decimal temp;
                        do
                        {
                            Console.Write("Введите температуру (0-30°C): ");
                            if (decimal.TryParse(Console.ReadLine(), out temp) && temp >= 0 && temp <= 30)
                            {
                                break;
                            }
                            ShowError("Неверный формат температуры. Введите число от 0 до 30.");
                        } while (true);

                        foreach (var heater in _rooms.SelectMany(r => r.GetDevices().OfType<Heater>()))
                        {
                            heater.DesiredTemperature = (int)temp;
                        }
                        ShowSuccess($"Температура установлена на {(int)temp}°C для всех обогревателей");
                        break;
                    case "2":
                        // Цикл с валидацией яркости
                        int brightness;
                        do
                        {
                            Console.Write("Введите яркость (0-100): ");
                            if (int.TryParse(Console.ReadLine(), out brightness) && brightness >= 0 && brightness <= 100)
                            {
                                break;
                            }
                            ShowError("Неверный формат яркости. Введите число от 0 до 100.");
                        } while (true);

                        foreach (var light in _rooms.SelectMany(r => r.GetDevices().OfType<Light>()))
                        {
                            light.DesiredBrightness = brightness;
                        }
                        ShowSuccess($"Яркость установлена на {brightness}% для всех светов");
                        break;
                    case "3":
                        foreach (var light in _rooms.SelectMany(r => r.GetDevices().OfType<Light>()))
                        {
                            light.DesiredIsOn = true;
                        }
                        ShowSuccess("Все устройства включены");
                        break;
                    default: ShowError("Неверный выбор"); break;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

            WaitForContinue();
        }

        static void SetGlobalDesiredSettings()
        {
            Console.Clear();
            Console.WriteLine("=== ГЛОБАЛЬНЫЕ НАСТРОЙКИ ===");

            try
            {
                // Цикл с валидацией температуры
                decimal temp;
                do
                {
                    Console.Write("Введите желаемую температуру для всех комнат (0-30°C): ");
                    if (decimal.TryParse(Console.ReadLine(), out temp) && temp >= 0 && temp <= 30)
                    {
                        break;
                    }
                    ShowError("Неверный формат температуры. Введите число от 0 до 30.");
                } while (true);

                foreach (var heater in _rooms.SelectMany(r => r.GetDevices().OfType<Heater>()))
                {
                    heater.DesiredTemperature = (int)temp;
                }

                Console.Write("Включить свет во всех комнатах? (y/n): ");
                var lightInput = Console.ReadLine()?.ToLower();
                if (lightInput == "y")
                {
                    foreach (var light in _rooms.SelectMany(r => r.GetDevices().OfType<Light>()))
                    {
                        light.DesiredIsOn = true;
                    }
                }

                ShowSuccess("Глобальные настройки применены");
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

            WaitForContinue();
        }

        static void SetCurrentParameters()
        {
            Console.Clear();
            Console.WriteLine("=== ТЕКУЩИЕ ПАРАМЕТРЫ ===");

            var room = SelectRoom();
            if (room == null) return;

            var thermostat = room.GetDevices().OfType<Thermostat>().FirstOrDefault();
            if (thermostat == null)
            {
                ShowError("В этой комнате нет термостата");
                return;
            }

            // Цикл с валидацией температуры
            decimal temp;
            do
            {
                Console.Write($"Введите текущую температуру в {room.Name} (°C): ");
                if (decimal.TryParse(Console.ReadLine(), out temp) && temp >= 0 && temp <= 30)
                {
                    break;
                }
                ShowError("Неверный формат температуры. Введите число от 0 до 30.");
            } while (true);

            thermostat.CurrentTemperature = temp;
            ShowSuccess($"Текущая температура установлена на {temp}°C");

            WaitForContinue();
        }

        static void ShowScenesMenu()
        {
            Console.Clear();
            Console.WriteLine("=== СЦЕНЫ ===");

            for (int i = 0; i < _scenes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_scenes[i].Name}");
            }
            Console.Write("Выберите сцену: ");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= _scenes.Count)
            {
                _scenes[choice - 1].Execute(_rooms);
                ShowSuccess($"Сцена '{_scenes[choice - 1].Name}' применена");
            }
            else
            {
                ShowError("Неверный выбор");
            }

            WaitForContinue();
        }

        static void RunAutomation()
        {
            Console.Clear();
            _homeController.Execute();
            ShowSuccess("Автоматизация выполнена!");
            WaitForContinue();
        }

        static void SimulateMotion()
        {
            Console.Clear();
            Console.WriteLine("=== ИМИТАЦИЯ ДВИЖЕНИЯ ===");

            var room = SelectRoom();
            if (room == null) return;

            Console.WriteLine($"💡 Имитация движения в комнате {room.Name}...");
            EventBus.Instance.Publish("MotionDetected", room.Name);
            ShowSuccess($"Движение имитировано в комнате {room.Name} - свет включен");
            WaitForContinue();
        }

        // НОВЫЙ МЕТОД: Включение всех устройств в комнате
        static void TurnAllOnInRoom()
        {
            Console.Clear();
            Console.WriteLine("=== ВКЛЮЧЕНИЕ ВСЕХ УСТРОЙСТВ В КОМНАТЕ ===");
            
            var room = SelectRoom();
            if (room == null) return;

            room.TurnAllOn();
            ShowSuccess($"Все устройства включены в комнате {room.Name}");
            WaitForContinue();
        }

        static Room? SelectRoom()
        {
            Console.WriteLine("\nВыберите комнату:");
            for (int i = 0; i < _rooms.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_rooms[i].Name}");
            }
            Console.Write("Введите номер: ");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= _rooms.Count)
            {
                return _rooms[choice - 1];
            }

            ShowError("Неверный выбор комнаты");
            return null;
        }

        static void ShowError(string message)
        {
            Console.WriteLine($"\n❌ Ошибка: {message}");
        }

        static void ShowSuccess(string message)
        {
            Console.WriteLine($"\n✅ {message}");
        }

        static void WaitForContinue()
        {
            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }
}