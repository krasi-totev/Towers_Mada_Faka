﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Towers
{
    class Program
    {
        //Keys
        static ConsoleKey firstVelocityUpKey = ConsoleKey.D;
        static ConsoleKey firstVelocityDownKey = ConsoleKey.A;
        static ConsoleKey firstAngleUpKey = ConsoleKey.W;
        static ConsoleKey firstAngleDownKey = ConsoleKey.S;
        static ConsoleKey firstShootKey = ConsoleKey.Spacebar;

        static ConsoleKey secondVelocityUpKey = ConsoleKey.RightArrow;
        static ConsoleKey secondVelocityDownKey = ConsoleKey.LeftArrow;
        static ConsoleKey secondAngleUpKey = ConsoleKey.UpArrow;
        static ConsoleKey secondAngleDownKey = ConsoleKey.DownArrow;
        static ConsoleKey secondShootKey = ConsoleKey.Enter;


        static string firstPlayerName = "Player 1";
        static string secondPlayerName = "Player 2";
        static int firstPlayerScore = 0;
        static int secondPlayerScore = 0;
        static int terrainHeight = 70;
        static int terrainWidth = 150;
        static int firstTowerAngle = 45;
        static int secondTowerAngle = 45;
        static int firstTowerVelocity = 0;
        static int secondTowerVelocity = 0;
        static int firstPlayerLivePoints = 100;
        static int secondPlayerLivePoints = 100;
        static bool activePlayer = false;
        static int[] firstTowerCoordinates = new int[2];
        static int[] secondTowerCoordinates = new int[2];
        static char[,] terrain;
        private static int d1;

        static void Main()
        {
            SetGame();
            while (true)
            {
                DrawTerrain();
                PrintPanel();
                try
                {
                    while (true)
                    {
                        if (Console.KeyAvailable)
                        {
                            KeyPress(Console.ReadKey());
                            PrintPanel();
                        }
                    }
                }
                catch (Exception)
                {
                    Shoot();
                }
            }
        }

        static void SetConsole()
        {
            Console.BufferHeight = Console.WindowHeight = terrainHeight + 7;
            Console.BufferWidth = Console.WindowWidth = terrainWidth;
            terrain = new char[terrainHeight, terrainWidth];
        }

        static void SetGame()
        {
            SetConsole();
            BuildRandomTerrain();
            PrintFirstTower(10);
            PrintSecondTower(terrainWidth - 10);
        }

        static void Menu()
        {

        }

        static void BuildTerrainFromFile(char[,] terrain, string file)
        {

        }

        static void BuildRandomTerrain()
        {
            int minHeight = 35;
            int maxStep = 2;
            int maxHeight = 60;
            int currentHeight = 45;
            int nextHeight;
            Random rnd = new Random();

            for (int col = 0; col < terrain.GetLength(1); col++)
            {
                do
                {
                    nextHeight = rnd.Next(currentHeight - maxStep, currentHeight + maxStep + 1);
                } while (!(minHeight <= nextHeight && nextHeight <= maxHeight));
                currentHeight = nextHeight;
                for (int row = currentHeight; row < terrain.GetLength(0); row++)
                {
                    terrain[row, col] = '#';
                }
            }
        }

        static void PrintFirstTower(int x)
        {
            //set first tower coordinates
            int towerHeight = 5;
            firstTowerCoordinates[1] = x;
            for (int row = terrainHeight - 1; row >= 0; row--)
            {
                if (terrain[row, x] != '#')
                {
                    firstTowerCoordinates[0] = row;
                    break;
                }
            }
            //print first Tower
            for (int row = firstTowerCoordinates[0]; row > firstTowerCoordinates[0] - towerHeight; row--)
            {
                for (int col = x - 1; col < x + 1; col++)
                {
                    terrain[row, col] = '1';
                }
            }
        }

        static void PrintSecondTower(int x)
        {
            int towerHeight = 5;
            //set second tower coordinates
            secondTowerCoordinates[1] = x;
            for (int row = terrainHeight - 1; row >= 0; row--)
            {

                if (terrain[row, x] != '#')
                {
                    secondTowerCoordinates[0] = row;
                    break;
                }
            }
            //print second Tower
            for (int row = secondTowerCoordinates[0]; row > secondTowerCoordinates[0] - towerHeight; row--)
            {
                for (int col = x - 1; col < x + 1; col++)
                {
                    terrain[row, col] = '2';
                }
            }
        }

        static void Shoot()
        {
            if (activePlayer == true)
            {
                BallMovement(firstTowerVelocity, firstTowerAngle, activePlayer);
            }
            else
            {
                BallMovement(secondTowerVelocity, secondTowerAngle, activePlayer);
            }
            activePlayer = !activePlayer;
        }

        static void HitTerrain(int hitX, int hitY)
        {

        }

        static void HitTower(int hitX, int hitY)
        {

        }

        static void Impact(int hitX, int hitY)
        {

        }

        static void BallMovement(int velocity, int angle, bool activePlayer)
        {
            //return hitX and hitY;
            int startingPointX = 0;
            int startingPointY = 0;
            if (activePlayer == true)
            {
                startingPointX = firstTowerCoordinates[1] + 1;
                startingPointY = firstTowerCoordinates[0] - 5;
            }
            else if (activePlayer == false)
            {
                startingPointX = secondTowerCoordinates[1] - 2;
                startingPointY = secondTowerCoordinates[0] - 5;
            }
            int oldX = 0;
            int oldY = 0;
            int x;
            int y;
            int g = 1;
            double angleInRadians = angle * Math.PI / 180;

            if (activePlayer == true)
            {
                for (float time = 0; time < 2000; time += 0.1f)
                {
                    x = startingPointX + (int)(velocity * time * Math.Cos(angleInRadians));
                    y = (int)(startingPointY - (velocity * time * Math.Sin(angleInRadians) - (g * Math.Pow(time, 2)) / 2));
                    if (x > terrainWidth - 1 || y < 0 || x < 0 || y > terrainHeight - 1)
                    {
                        return;
                    }
                    if (terrain[y, x] == '#')
                    {
                        HitTerrain(y, x);
                        return;
                    }
                    if (terrain[y, x] == '2')
                    {
                        HitTower(y, x);
                        return;
                    }
                    Thread.Sleep(30);
                    PrintOnPosition(x, y + 7, '*', ConsoleColor.White);
                    oldX = x;
                    oldY = y;
                }
            }
            else if (activePlayer == false)
            {
                for (float time = 0; time < 2000; time += 0.1f)
                {
                    x = startingPointX - (int)(velocity * time * Math.Cos(angleInRadians));
                    y = (int)(startingPointY - (velocity * time * Math.Sin(angleInRadians) - (g * Math.Pow(time, 2)) / 2));
                    if (x > terrainWidth - 1 || y < 0 || x < 0 || y > terrainHeight - 1)
                    {
                        return;
                    }
                    if (terrain[y, x] == '#')
                    {
                        HitTerrain(y, x);
                        return;
                    }
                    if (terrain[y, x] == '1')
                    {
                        HitTower(y, x);
                        return;
                    }
                    Thread.Sleep(30);
                    PrintOnPosition(x, y + 7, '*', ConsoleColor.White);
                    oldX = x;
                    oldY = y;
                }
            }
        }

        static void DrawTerrain()
        {
            //Draw terrain
            StringBuilder terrainBuilder = new StringBuilder();

            for (int row = 0; row < terrainHeight; row++)
            {
                for (int col = 0; col < terrainWidth; col++)
                {
                    if (terrain[row, col] == '#' && (row != terrainHeight - 1 || col != terrainHeight - 1))
                    {
                        terrainBuilder.Append("#");
                    }
                    else if (terrain[row, col] == '1')
                    {
                        terrainBuilder.Append("1");
                    }
                    else if (terrain[row, col] == '2')
                    {
                        terrainBuilder.Append("2");
                    }
                    else if (row != terrainHeight - 1 || col != terrainHeight - 1)
                    {
                        terrainBuilder.Append(" ");
                    }
                }
            }

            Console.SetCursorPosition(0, 7);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(terrainBuilder.ToString());


            //Draw Towers
            Console.CursorVisible = false;
            for (int row = 0; row < terrainHeight; row++)
            {
                for (int col = 0; col < terrainWidth; col++)
                {
                    if (terrain[row, col] == '1')
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        PrintOnPosition(col, row + 7, terrain[row, col], ConsoleColor.Red);
                    }
                    else if (terrain[row, col] == '2')
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        PrintOnPosition(col, row + 7, terrain[row, col], ConsoleColor.Blue);
                    }
                }
            }
        }

        static void PrintOnPosition(int x, int y, char c, ConsoleColor color = ConsoleColor.White)
        {

            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(c);
            Console.BackgroundColor = ConsoleColor.Black;

        }

        static void ModifyAngle(ConsoleKeyInfo key)
        {
            const int sencitivity = 5;
            const int maxAngle = 90;
            const int minAngle = -45;

            if (key.Key == firstAngleUpKey && activePlayer == true && firstTowerAngle < maxAngle)
            {
                firstTowerAngle += sencitivity;
            }
            else if (key.Key == firstAngleDownKey && activePlayer == true && firstTowerAngle > minAngle)
            {
                firstTowerAngle -= sencitivity;
            }
            else if (key.Key == secondAngleUpKey && activePlayer == false && secondTowerAngle < maxAngle)
            {
                secondTowerAngle += sencitivity;
            }
            else if (key.Key == secondAngleDownKey && activePlayer == false && secondTowerAngle > minAngle)
            {
                secondTowerAngle -= sencitivity;
            }
        }

        static void ModifyVelocity(ConsoleKeyInfo key)
        {
            const int sencitivity = 1;
            const int maxVelocity = 15;
            const int minVelocity = 2;

            if (key.Key == firstVelocityUpKey && activePlayer == true && firstTowerVelocity < maxVelocity)
            {
                firstTowerVelocity += sencitivity;
            }
            else if (key.Key == firstVelocityDownKey && activePlayer == true && firstTowerVelocity > minVelocity)
            {
                firstTowerVelocity -= sencitivity;
            }
            else if (key.Key == secondVelocityDownKey && activePlayer == false && secondTowerVelocity < maxVelocity)
            {
                secondTowerVelocity += sencitivity;
            }
            else if (key.Key == secondVelocityUpKey && activePlayer == false && secondTowerVelocity > minVelocity)
            {
                secondTowerVelocity -= sencitivity;
            }
        }

        static void PrintPanel()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('=', terrainWidth);
            builder.Append(firstPlayerName);
            builder.Append(new string(' ', terrainWidth - firstPlayerName.Length - secondPlayerName.Length));
            builder.Append(secondPlayerName);
            Console.BackgroundColor = ConsoleColor.DarkMagenta;


            //Write current scores
            string currentResult = string.Format("{0}  {1}:{2}  {3}", firstPlayerName, firstPlayerScore, secondPlayerScore, secondPlayerName);
            builder.Append(' ', (terrainWidth - currentResult.Length) / 2);
            builder.Append(currentResult);
            builder.Append(' ', (terrainWidth - currentResult.Length) / 2 + 1);


            //Print players live points
            string firstPlayerLiveString = string.Format("Lives: {0}", firstPlayerLivePoints);
            string secondPlayerLiveString = string.Format("Lives: {0}", secondPlayerLivePoints);
            builder.Append(firstPlayerLiveString);
            builder.Append(' ', terrainWidth - firstPlayerLiveString.Length - secondPlayerLiveString.Length);
            builder.Append(secondPlayerLiveString);

            //Prin players shooting angles
            string firstTowerAngleString = string.Format("Angle: {0}", firstTowerAngle);
            string secondTowerAngleString = string.Format("Angle: {0}", secondTowerAngle);
            builder.Append(firstTowerAngleString);
            builder.Append(' ', terrainWidth - firstTowerAngleString.Length - secondTowerAngleString.Length);
            builder.Append(secondTowerAngleString);

            //Print players shooting velocities
            string firstTowerVelocityString = string.Format("Velocity: {0}", firstTowerVelocity);
            string secondTowerVelocityString = string.Format("Velocity: {0}", secondTowerVelocity);
            builder.Append(firstTowerVelocityString);
            builder.Append(' ', terrainWidth - firstTowerVelocityString.Length - secondTowerVelocityString.Length);
            builder.Append(secondTowerVelocityString);
            //Draw line

            builder.Append('=', terrainWidth);

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(builder.ToString());
        }

        static void KeyPress(ConsoleKeyInfo keyPressed)
        {

            if (keyPressed.Key == secondAngleUpKey || keyPressed.Key == secondAngleDownKey ||
                keyPressed.Key == firstAngleUpKey || keyPressed.Key == firstAngleDownKey)
            {
                ModifyAngle(keyPressed);
            }

            if (keyPressed.Key == secondVelocityDownKey || keyPressed.Key == secondVelocityUpKey ||
                keyPressed.Key == firstVelocityUpKey || keyPressed.Key == firstVelocityDownKey)
            {
                ModifyVelocity(keyPressed);
            }

            if ((keyPressed.Key == firstShootKey && activePlayer == true) || (keyPressed.Key == secondShootKey && activePlayer == false))
            {
                throw new Exception();
            }
        }
    }
}
