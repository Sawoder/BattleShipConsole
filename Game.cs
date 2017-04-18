using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipConsole
{
    class Game
    { 
        static void Main(string[] args)
        {
            bool isGame = true;
            bool turn = true;
            int scorePlayer = 0;
            int scoreComputer = 0;
            Random rnd = new Random(DateTime.Now.Millisecond);
            int x, y;
            BoardCell[,] enemyField = new BoardCell[10, 10];
            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 10; j++)
                    enemyField[i, j] = new BoardCell(i, j);

            Board player = new Board();
            Board computer = new Board();

            computer.Setup();
            computer.CreateShips(false);

            player.Setup();
            player.CreateShips(true);

            player.ShowBoard();

            while (isGame)
            {

                if (turn)
                {
                    Console.Clear();
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                            Console.Write("{0} ", (int)enemyField[i, j].stateCell);
                        Console.WriteLine();
                    }
                    Console.WriteLine("Your score: {0}, Enemy score: {1}", scorePlayer, scoreComputer);
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                            Console.Write("{0} ", (int)player.field[i, j].stateCell);
                        Console.WriteLine();
                    }
                    try
                    {
                        x = int.Parse(Console.ReadLine());
                        if (x > 9 || x < 0) throw new FormatException();
                        y = int.Parse(Console.ReadLine());
                        if (y > 9 || y < 0) throw new FormatException();
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Invalid coord");
                        continue;
                    }

                    if (computer.field[x, y].stateCell == BoardCell.StateCell.Hit || computer.field[x, y].stateCell == BoardCell.StateCell.Miss) continue;
                    if (computer.field[x, y].stateCell == BoardCell.StateCell.Ship)
                    {
                        computer.field[x, y].stateCell = BoardCell.StateCell.Hit;
                        enemyField[x, y].stateCell = BoardCell.StateCell.Hit;
                        scorePlayer++;
                        if (scorePlayer != 20)
                            continue;
                    }
                    else
                    {
                        computer.field[x, y].stateCell = BoardCell.StateCell.Miss;
                        enemyField[x, y].stateCell = BoardCell.StateCell.Miss;
                    }
                    turn = false;
                }
                else
                {
                    x = rnd.Next(0, 10);
                    y = rnd.Next(0, 10);
                    if (player.field[x, y].stateCell == BoardCell.StateCell.Hit || player.field[x, y].stateCell == BoardCell.StateCell.Miss) continue;
                    if (player.field[x, y].stateCell == BoardCell.StateCell.Ship)
                    {
                        player.field[x, y].stateCell = BoardCell.StateCell.Hit;
                        scoreComputer++;
                        if (scoreComputer != 20)
                            continue;
                    }
                    else player.field[x, y].stateCell = BoardCell.StateCell.Miss;
                    turn = true;
                }

                if (scorePlayer == 20 || scoreComputer == 20) // Закончить игру, если потоплены все корабли
                    isGame = false;
            }
            Console.WriteLine(scorePlayer == 20 ? "Player win" : "Computer win");
        }
    }
}
