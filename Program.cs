using System;
using System.Collections.Generic;
using System.Threading;
class Program
{
    static void Main()
    {
        Console.Title = "Змейка";
        Console.CursorVisible = false;
        Console.SetWindowSize(80, 25);
        Console.SetBufferSize(80, 25);
        // Создаем генератор случайных чисел
        Random random = new Random();
                // Змейка
        List<(int X, int Y)> snake = new List<(int, int)>();
        snake.Add((40, 12));
        snake.Add((39, 12));
        snake.Add((38, 12));
        // === НОВОЕ: ЕДА ===
        // Объявляем переменные для координат еды
        int foodX;
        int foodY;
                // Функция, которая генерирует еду в случайном месте
        // Мы её опишем позже, а пока просто вызовем
        GenerateFood(random, snake, out foodX, out foodY);
        // ==================
        int directionX = 1;
        int directionY = 0;
        while (true)
        {
            // Управление (без изменений)
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow: directionX = 0; directionY = -1; break;
                    case ConsoleKey.DownArrow: directionX = 0; directionY = 1; break;
                    case ConsoleKey.LeftArrow: directionX = -1; directionY = 0; break;
                    case ConsoleKey.RightArrow: directionX = 1; directionY = 0; break;
                }
            }
            // Логика движения
            var head = snake[0];
            int newHeadX = head.X + directionX;
            int newHeadY = head.Y + directionY;
            snake.Insert(0, (newHeadX, newHeadY));
                        // === ИЗМЕНЕНИЕ: проверяем, съели ли мы еду ===
            // Если новая голова встала на то же место, где еда
            if (newHeadX == foodX && newHeadY == foodY)
            {
                // Мы съели еду! НЕ удаляем хвост (змейка растет)
                // И генерируем новую еду
                GenerateFood(random, snake, out foodX, out foodY);
            }
            else
            {
                // Еду не съели - удаляем хвост (змейка движется как обычно)
                snake.RemoveAt(snake.Count - 1);
            }
            // =============================================
            Console.Clear();
            // Рисуем границы (без изменений)
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write('#');
                Console.SetCursorPosition(i, Console.WindowHeight - 1);
                Console.Write('#');
            }
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write('#');
                Console.SetCursorPosition(Console.WindowWidth - 1, i);
                Console.Write('#');
            }
            // Рисуем змейку (без изменений)
            foreach (var segment in snake)
            {
                Console.SetCursorPosition(segment.X, segment.Y);
                Console.Write('O');
            }
            // === НОВОЕ: Рисуем еду ===
            Console.SetCursorPosition(foodX, foodY);
            Console.Write('@'); // Еда - символ @
            // ========================
            Thread.Sleep(100);
        }
    }
    // === НОВЫЙ МЕТОД для генерации еды ===
    // static - значит метод принадлежит классу, а не объекту
    // void - ничего не возвращает
    // out int foodX, out int foodY - мы как бы возвращаем два числа через параметры
    static void GenerateFood(Random random, List<(int X, int Y)> snake, out int foodX, out int foodY)
    {
        bool isOnSnake; // Объявляем переменную здесь
            do
        {
            foodX = random.Next(1, Console.WindowWidth - 1);
            foodY = random.Next(1, Console.WindowHeight - 1);
                    isOnSnake = false; // Сбрасываем флаг
                    foreach (var segment in snake)
            {
                if (segment.X == foodX && segment.Y == foodY)
                {
                    isOnSnake = true;
                    break;
                }
            }
        
        } while (isOnSnake); // А теперь переменная видна!
    }
}
