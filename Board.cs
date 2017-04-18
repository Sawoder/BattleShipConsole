using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsole
{
    class Board
    {
        public BoardCell[,] field;
        public List<Ship> ships;

        public Board()
        {
            this.field = new BoardCell[10, 10];
            this.ships = new List<Ship>();
        }

        public void Setup()
        {
            for (int i = 0; i < 10; i++) 
                for (int j = 0; j < 10; j++)
                    field[i, j] = new BoardCell(i, j); 
        }
        /* Координаты поля
         *   x
         * y 0 1 2
         *   1
         *   2
         */
        public void CreateShips(bool isPlayer)
        {
            int size = 4; // Размер корабля
            int count = 0; // Для подсчета количества кораблей нужного размера
            char orientation = 'h'; // Положение корабля на поле
            Random rnd = new Random(DateTime.Now.Millisecond);
            int x, y;
            for (int i = 0; i < 10; i++) // 10 кораблей
            { 
                try
                { // Можно ввести только координаты от 0 до 9 вручную или автоматически 
                    if (isPlayer)
                    {
                        Console.WriteLine("Coord for {0}-x ship", size);
                        x = int.Parse(Console.ReadLine());
                        if (x > 9 || x < 0) throw new FormatException();
                        y = int.Parse(Console.ReadLine());
                        if (y > 9 || y < 0) throw new FormatException();
                    }
                    else
                    {
                        x = rnd.Next(0, 10);
                        y = rnd.Next(0, 10);
                    }
                }
                catch (FormatException)
                {
                    if (isPlayer)
                        Console.WriteLine("Only numbers less than 10 or positive");
                    i--;
                    continue;
                }
                if (size != 1) // Для корабля размера 1 не нужно определять ориентацию
                {
                    if (isPlayer)
                    {
                        Console.WriteLine("'H' or 'h' for Horizontal and 'V' or 'v' for Vertical");
                        orientation = Console.ReadLine().ToCharArray()[0];
                    }
                    else 
                    {
                        orientation = rnd.Next(0, 2) == 0 ? 'h' : 'v';
                    }
                } 
                if (field[x, y].stateCell == BoardCell.StateCell.Empty) // Если клетка свободна, то ставим корабль
                    ships.Add(new Ship(size, orientation == 'H' || orientation == 'h' ? Ship.Orientation.Horizontal : Ship.Orientation.Vertical));
                else
                {
                    i--;
                    continue;
                }


                try
                {
                    for (int j = 0; j < size; j++) // Размещаем корабль в соответствии с его размером и положением в пространстве
                    {
                        Ship.Orientation ornt = ships.ToArray()[ships.Count - 1].orientation;
                        if (field[x, y].stateCell == BoardCell.StateCell.Ship || field[x, y].stateCell == BoardCell.StateCell.Closed) // Если следующая клетка занята, то пробуем сделать новый корабль
                            throw new IndexOutOfRangeException();
                        ships.ToArray()[ships.Count - 1].part[j] = new BoardCell(x, y) { stateCell = BoardCell.StateCell.Ship }; // Присваиваем клетке статус корабля
                        if (ornt == Ship.Orientation.Horizontal)
                            field[x, y++].stateCell = BoardCell.StateCell.Ship;
                        else
                            field[x++, y].stateCell = BoardCell.StateCell.Ship;
                    }
                }
                catch (IndexOutOfRangeException) // Пробуем еще раз ввести координаты, если вышел за пределы
                {
                    if (isPlayer)
                        Console.WriteLine("Index Out of Range\n\nEnter again:");
                    i--;
                    ships.Remove(ships.ToArray()[ships.Count - 1]);
                    continue;
                }

                count++; // Для правильной отработки количества кораблей в соответствии с его размером
                if (size == 4 && count == 1 || size == 3 && count == 2 || size == 2 && count == 3) 
                {
                    size--;
                    count = 0;
                }

                CheckRegion(); // Обработка запретной зоны вокруг корабля для других кораблей
                if (isPlayer) ShowBoard();
            }
            CheckCount(isPlayer);

            // После получения кораблей надо убрать закрытые зоны
            for (int q = 0; q < 10; q++)
                for (int p = 0; p < 10; p++)
                    if (field[q, p].stateCell == BoardCell.StateCell.Closed)
                        field[q, p].stateCell = BoardCell.StateCell.Empty;
        }

        // Отображение поля
        public void ShowBoard()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write("{0} ", (int)this.field[i, j].stateCell);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        // Простановка закрытой зоны вокруг корабля
        // После размещения корабля помечаем остальные пустые клетки как потенциально закрытые
        // Если вокруг данной клетки есть корабль, то делаем эту клетку закрытой
        // Иначе делаем пустой
        // В конце переводим все оставшиеся потенциально закрытые клетки в просто закрытые 
        public void CheckRegion()
        {
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                    if (field[x, y].stateCell == BoardCell.StateCell.Empty)
                        field[x, y].stateCell = BoardCell.StateCell.Hit;

            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                {
                    if (field[x, y].stateCell == BoardCell.StateCell.Hit)
                    {
                        if (x + 1 < 10)
                            if (field[x + 1, y].stateCell == BoardCell.StateCell.Ship)
                                continue;
                        if (x + 1 < 10 && y - 1 >= 0)
                            if (field[x + 1, y - 1].stateCell == BoardCell.StateCell.Ship)
                                continue;
                        if (y - 1 >= 0)
                            if (field[x, y - 1].stateCell == BoardCell.StateCell.Ship)
                                continue;
                        if (x - 1 >= 0 && y - 1 >= 0)
                            if (field[x - 1, y - 1].stateCell == BoardCell.StateCell.Ship)
                                continue;
                        if (x - 1 >= 0)
                            if (field[x - 1, y].stateCell == BoardCell.StateCell.Ship)
                                continue;
                        if (x - 1 >= 0 && y + 1 < 10)
                            if (field[x - 1, y + 1].stateCell == BoardCell.StateCell.Ship)
                                continue;
                        if (y + 1 < 10)
                            if (field[x, y + 1].stateCell == BoardCell.StateCell.Ship)
                                continue;
                        if (x + 1 < 10 && y + 1 < 10)
                            if (field[x + 1, y + 1].stateCell == BoardCell.StateCell.Ship)
                                continue;
                        field[x, y].stateCell = BoardCell.StateCell.Empty;
                    }

                }

            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                    if (field[x, y].stateCell == BoardCell.StateCell.Hit)
                        field[x, y].stateCell = BoardCell.StateCell.Closed;
        }

        // Кусок проверки на правильное количество кораблей, если ошибка, то создаем еще раз
        public void CheckCount(bool isPlayer) 
        {
            int shipCount = 0;
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    if (field[i, j].stateCell == BoardCell.StateCell.Ship) shipCount++;
            if (shipCount != 20)
            {
                Setup();
                ships.Clear();
                CreateShips(isPlayer);
            }
        }
    }
}
