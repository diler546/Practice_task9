using System;
using System.Text.RegularExpressions;

namespace Cheesboard
{
    class Program
    {
        static string input = "";
        static string firstFigureCoordinates = "";
        static string secondFigureCoordinates = "";
        static string thirdFigureCoordinates = "";
        static string nameWhiteFigure = "";
        static string nameBlackFigure = "";

        static bool IsValidChessCoordinateFormat()
        {
            string patter = @"^(ладья|конь|слон|ферзь|король)\s[a-h]+[1-8]\s(ладья|конь|слон|ферзь|король)\s[a-h]+[1-8]\s[a-h]+[1-8]$";
            return Regex.IsMatch(input, patter);
        }

        static void EnteringString()
        {
            Console.WriteLine("Введите название белой фигуры(ладья, конь, слон, ферзь, король), её координаты x1y1, также название черной фигуры(ладья, конь, слон, ферзь, король), её координаты x2y2 и клетки на которую чёрной фигуре переместится(Пример:ферзь d3 слон e1 d8): ");
            input = Console.ReadLine().ToLower();
        }

        static void SplittingAString()
        {
            try
            {
                string[] parts = input.Split(' ');
                nameWhiteFigure = parts[0];
                firstFigureCoordinates = parts[1];
                nameBlackFigure = parts[2];
                secondFigureCoordinates = parts[3];
                thirdFigureCoordinates = parts[4];
            }
            catch
            {
                Console.WriteLine("Не правельный ввод");
            }

        }

        static void ConvertChessCoordinatesToArray(string str, out int[] shape)
        {
            shape = new int[2];
            switch (str[0])
            {
                case 'a': shape[0] = 0; break;
                case 'b': shape[0] = 1; break;
                case 'c': shape[0] = 2; break;
                case 'd': shape[0] = 3; break;
                case 'e': shape[0] = 4; break;
                case 'f': shape[0] = 5; break;
                case 'g': shape[0] = 6; break;
                case 'h': shape[0] = 7; break;
            }

            shape[1] = Convert.ToInt32(str[1].ToString()) - 1;
        }

        static bool IsQueenTargetingFigure(string coordinatesFigure, string coordinatesCell)
        {
            int[] queenPosition = new int[2];

            int[] CellPosition = new int[2];

            ConvertChessCoordinatesToArray(coordinatesFigure, out queenPosition);

            ConvertChessCoordinatesToArray(coordinatesCell, out CellPosition);

            int diagonal1Constant = queenPosition[1] - queenPosition[0];
            int diagonal2Constant = queenPosition[1] + queenPosition[0];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if ((i - j) == diagonal1Constant || (i + j) == diagonal2Constant)
                    {
                        if (i == CellPosition[1] && j == CellPosition[0])
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        static bool IsKnightTargetingFigure(string coordinatesFigure, string coordinatesCell)
        {
            int[] knightPosition = new int[2];

            int[] CellPosition = new int[2];

            ConvertChessCoordinatesToArray(coordinatesFigure, out knightPosition);

            ConvertChessCoordinatesToArray(coordinatesCell, out CellPosition);

            int[] xMoves = { 2, 1, -1, -2, -2, -1, 1, 2 };
            int[] yMoves = { 1, 2, 2, 1, -1, -2, -2, -1 };

            for (int i = 0; i < 8; i++)
            {
                int potentialPositionX = knightPosition[1] + xMoves[i];
                int potentialPositionY = knightPosition[0] + yMoves[i];

                if (potentialPositionX == CellPosition[1] && potentialPositionY == CellPosition[0])
                {
                    return true;
                }
            }
            return false;
        }

        static bool IsKingTargetingFigure(string coordinatesFigure, string coordinatesCell)
        {
            int[] kingPosition = new int[2];

            int[] CellPosition = new int[2];

            ConvertChessCoordinatesToArray(coordinatesFigure, out kingPosition);

            ConvertChessCoordinatesToArray(coordinatesCell, out CellPosition);

            for (int i = kingPosition[1] - 1; i <= kingPosition[1] + 1; i++)
            {
                for (int j = kingPosition[0] - 1; j <= kingPosition[0] + 1; j++)
                {
                    if (i == CellPosition[1] && j == CellPosition[0])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static bool ChoosingFigure(string figure, string coordinatesFigure, string coordinatesCell)
        {
            switch (figure)
            {
                case "ладья": if (firstFigureCoordinates[0] == thirdFigureCoordinates[0] || firstFigureCoordinates[1] == thirdFigureCoordinates[1]) return true; break;
                case "конь": if (IsKnightTargetingFigure(coordinatesFigure, coordinatesCell)) return true; break;
                case "слон": if (IsQueenTargetingFigure(coordinatesFigure, coordinatesCell)) return true; break;
                case "ферзь": if (IsQueenTargetingFigure(coordinatesFigure, coordinatesCell) || firstFigureCoordinates[0] == thirdFigureCoordinates[0] || firstFigureCoordinates[1] == thirdFigureCoordinates[1]) return true; break;
                case "король": if (IsKingTargetingFigure(coordinatesFigure, coordinatesCell)) return true; break;
            }
            return false;
        }

        static void CanFigureMove()
        {
            if (ChoosingFigure(nameWhiteFigure, firstFigureCoordinates, thirdFigureCoordinates) && ChoosingFigure(nameBlackFigure, secondFigureCoordinates, thirdFigureCoordinates))
            {
                Console.WriteLine($"{nameWhiteFigure} не сможет добратся до клетки {thirdFigureCoordinates} по причине попадания под атаку {nameBlackFigure}");
            }
            else if (ChoosingFigure(nameWhiteFigure, firstFigureCoordinates, thirdFigureCoordinates))
            {
                Console.WriteLine($"{nameWhiteFigure} сможет добратся до клетки {thirdFigureCoordinates} не попав под атаку {nameBlackFigure}");
            }
            else if (ChoosingFigure(nameWhiteFigure, firstFigureCoordinates, secondFigureCoordinates))
            {
                Console.WriteLine($"{nameWhiteFigure} сможет добратся до клетки {thirdFigureCoordinates} не попав под атаку {nameBlackFigure}");
            }
            else
            {
                Console.WriteLine($"{nameWhiteFigure} не сможет добратся до клетки {thirdFigureCoordinates} по причине не возможности туда передвинуться");
            }
        }

        static bool CheckFigureMoveValidity()
        {
            if (!(IsValidChessCoordinateFormat()))
            {
                Console.WriteLine("Вы вели строку неправильного формата \n" +
                                  "Формат:a1 a2");
                return true;
            }
            else if (firstFigureCoordinates == secondFigureCoordinates && firstFigureCoordinates == thirdFigureCoordinates && secondFigureCoordinates == thirdFigureCoordinates)
            {
                Console.WriteLine("Вы вели одинаковые позиции фигур \n" +
                                  "Введите разные позиции фигур");
                return true;
            }
            return false;
        }

        static void Main()
        {
            do
            {
                EnteringString();
                SplittingAString();

            } while (CheckFigureMoveValidity());

            CanFigureMove();

        }
    }
}