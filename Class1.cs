using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class GameTool
    {
        // Dessine un tableau 2D "Array" de taille "ArraySize"
        // l'algorithme gère les espaces entre les éléments tant que les nombres du tableau ne dépasse pas 4 chiffres.
        public static void Draw2DArray(in int[,] Array, in int ArraySize)
        {
            string offset;

            for (int i = 0; i < ArraySize; i++)
            {
                for(int j = 0; j < ArraySize; j++)
                {
                    if(Array[i, j] < 10000 && Array[i, j] > 999)
                    {
                        offset = "";
                    }
                    else if(Array[i, j] < 1000 && Array[i, j] > 99)
                    {
                        offset = " ";
                    }
                    else if(Array[i, j] < 100 && Array[i, j] > 9)
                    {
                        offset = "  ";
                    }
                    else 
                    {
                        offset = "   ";
                    }

                    if(j == ArraySize -1)
                    {
                        Console.Write(Array[i, j] + offset + "\n\n");
                    }
                    else
                    {
                        Console.Write(Array[i, j] + offset);
                    }
                }
            }
        }

        // Initialise un tableau 2D "Array" de taille "ArraySize" avec la valeur "Value".
        public static int[,] InitializeArray(int[,] Array, in int ArraySize, in int Value)
        {
            for (int i = 0; i < ArraySize; i++)
            {
                for (int j = 0; j < ArraySize; j++)
                {
                    Array[i, j] = Value;
                }
            }

            return Array;
        }

        // Vérifie si le tableau est rempli et conditionne l'arrêt du jeu.
        public static bool LostCondition(in int ArraySize, in int[,] BoardGame)
        {
            bool lost = false;

            int isFilled = 0;

            /*Lis le tableau une fois pour vérifier le nombre de cases remplies. Si le compteur de cases remplies est de la taille du plateau 
            de jeu alors le jeu est terminé.*/
            for (int i = 0; i < ArraySize; i++)
            {
                for (int j = 0; j < ArraySize; j++)
                {
                    if (BoardGame[i, j] != 0)
                    {
                        isFilled++;
                    }
                }
            }

            if (isFilled == ArraySize * ArraySize)
            {
                lost = true;
            }

            return lost;
        }

        /* Rempli un tableau avec un seul 4 et plusieurs 2 dans lequel une fonction sélectionnera aléatoirement un 2 ou un 4 à placer sur le plateau de jeu
        à une position aléatoire. */ 
        public static int[] PercentOf4(in int Percent)
        {
            int ArraySize = 100 / Percent;
            int[] tab2Or4 = new int[ArraySize];

            tab2Or4[0] = 4;

            for(int i = 1; i < ArraySize;i++)
            {
                tab2Or4[i] = 2;
            }

            return tab2Or4;
        }

        // Choisi une position aléatoire du plateau de jeu. Si cette position contient un zéro, alors un 2 ou un 4 apparaîtra de manière aléatoire.
        public static int[,] Random2Or4(int[,] BoardGame, in int BoardSize, in int[] tabOfChance)
        {
            Random rand2Or4 = new Random();
            Random randPos1 = new Random();
            Random randPos2 = new Random();

            bool draw = true;
            int pos1 = 0;
            int pos2 = 0;

            while (draw)
            {
                // Choisi deux indices de positions aléatoires tant que l'on a pas trouvé de position contenant un 0.
                pos1 = randPos1.Next(BoardSize);
                pos2 = randPos2.Next(BoardSize);

                // Si une position aléatoire du plateau contient un 0, alors la fonction place de manière aléatoire un 2 ou un 4.
                if (BoardGame[pos1, pos2] == 0)
                {
                    BoardGame[pos1, pos2] = tabOfChance[rand2Or4.Next(tabOfChance.Length)];
                    draw = false;
                }
            }

            return BoardGame;
        }
    }

    public class Game
    {
        public String Name = "";

        public virtual void Execute()
        {
            Console.WriteLine($"Bienvenue dans le jeu {Name} ! Appuyez sur une touche pour commencer.");
            Console.ReadKey();
            Console.Clear();
        }
    }

    public class deuxMilQuarHuit : Game
    {
        public override void Execute()
        {
            base.Execute();
            bool error = true;

            int chance = 10;
            int size = 4;
            int[,] BoardGame = new int[size, size];

            // Il y a 10% de chances qu'un 4 apparaîsse sur le plateau de jeu.
            int[] tab2Or4 = GameTool.PercentOf4(chance);

            int k = 0;

            // On considère que sur le plateau de jeu, les i sont les lignes et les j les colonnes.

            GameTool.InitializeArray(BoardGame, size, 0);
            GameTool.Random2Or4(BoardGame, size, tab2Or4);

            while (!GameTool.LostCondition(size, BoardGame))
            {
                Console.Clear();
                GameTool.Draw2DArray(BoardGame, size);


                error = true;
                k = 0;
                while (error)
                {
                    Console.WriteLine("\nVeuillez appuyer sur une flèche directionnelle.\n");

                    ConsoleKeyInfo cki = Console.ReadKey();
                    switch (cki.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            error = false;

                            /*D'abord on ramène tous les nombres en partant de la droite vers la gauche du plateau de jeu, une colonne avant la dernière.
                            On le fait plusieurs fois afin d'éviter qu'un nombre soit perdu au milieu du plateau de jeu.*/

                            while(k < size)
                            {
                                for(int j = size - 2; j >= 0; j--)
                                {
                                    for (int i = 0; i < size; i++)
                                    {
                                        if(BoardGame[i, j] == 0 && BoardGame[i, j + 1] > 0)
                                        {
                                            BoardGame[i, j] = BoardGame[i, j + 1];
                                            BoardGame[i, j + 1] = 0;
                                        }
                                    }
                                }
                                k++;
                            }

                            // Ensuite, on additionne tous les nombres par ligne depuis la gauche du plateau de jeu.
                            // |---------------|
                            // | A OPTIMISER ! |
                            // |---------------|

                            for (int i = 0; i < size; i++)
                            {
                                if(BoardGame[i, 0] == BoardGame[i, 1])
                                {
                                    BoardGame[i, 0] = BoardGame[i, 0] * 2;
                                    BoardGame[i, 1] = BoardGame[i, 2];
                                    BoardGame[i, 2] = BoardGame[i, 3];
                                    BoardGame[i, 3] = 0;
                                }
                                if(BoardGame[i, 1] == BoardGame[i, 2])
                                {
                                    BoardGame[i, 1] = BoardGame[i, 1] * 2;
                                    BoardGame[i, 2] = BoardGame[i, 3];
                                    BoardGame[i, 3] = 0;
                                }
                                if(BoardGame[i, 2] == BoardGame[i, 3])
                                {
                                    BoardGame[i, 2] = BoardGame[i, 2] * 2;
                                    BoardGame[i, 3] = 0;
                                }
                            }

                            break;

                        case ConsoleKey.RightArrow:
                            error = false;

                            /*D'abord on ramène tous les nombres en partant de la gauche vers la droite du plateau de jeu, une colonne après la première.
                            On le fait plusieurs fois afin d'éviter qu'un nombre soit perdu au milieu du plateau de jeu.*/

                            while (k < size)
                            {
                                for (int j = 1; j < size; j++)
                                {
                                    for (int i = 0; i < size; i++)
                                    {
                                        if (BoardGame[i, j] == 0 && BoardGame[i, j - 1] > 0)
                                        {
                                            BoardGame[i, j] = BoardGame[i, j - 1];
                                            BoardGame[i, j - 1] = 0;
                                        }
                                    }
                                }
                                k++;
                            }

                            // Ensuite, on additionne tous les nombres par ligne depuis la gauche du plateau de jeu.
                            // |---------------|
                            // | A OPTIMISER ! |
                            // |---------------|

                            for (int i = 0; i < size; i++)
                            {
                                if (BoardGame[i, 3] == BoardGame[i, 2])
                                {
                                    BoardGame[i, 3] = BoardGame[i, 3] * 2;
                                    BoardGame[i, 2] = BoardGame[i, 1];
                                    BoardGame[i, 1] = BoardGame[i, 0];
                                    BoardGame[i, 0] = 0;
                                }
                                if (BoardGame[i, 2] == BoardGame[i, 1])
                                {
                                    BoardGame[i, 2] = BoardGame[i, 2] * 2;
                                    BoardGame[i, 1] = BoardGame[i, 0];
                                    BoardGame[i, 0] = 0;
                                }
                                if (BoardGame[i, 1] == BoardGame[i, 0])
                                {
                                    BoardGame[i, 1] = BoardGame[i, 1] * 2;
                                    BoardGame[i, 0] = 0;
                                }
                            }

                            break;

                        case ConsoleKey.UpArrow:
                            error = false;

                            /*D'abord on remonte tous les nombres en partant du bas du plateau de jeu, une ligne au dessus de la dernière.
                            On le fait plusieurs fois afin d'éviter qu'un nombre soit perdu au milieu du plateau de jeu.*/

                            while (k < size)
                            {
                                for (int i = size - 2; i >= 0; i--)
                                {
                                    for (int j = 0; j < size; j++)
                                    {
                                        if (BoardGame[i, j] == 0 && BoardGame[i + 1, j] > 0)
                                        {
                                            BoardGame[i, j] = BoardGame[i + 1, j];
                                            BoardGame[i + 1, j] = 0;
                                        }
                                    }
                                }
                                k++;
                            }

                            // Ensuite, on additionne tous les nombres par colonne depuis le haut du plateau de jeu.
                            // |---------------|
                            // | A OPTIMISER ! |
                            // |---------------|

                            for(int j = 0; j < size; j++)
                            {
                                if(BoardGame[0, j] == BoardGame[1, j])
                                {
                                    BoardGame[0, j] = BoardGame[0, j] * 2;
                                    BoardGame[1, j] = BoardGame[2, j];
                                    BoardGame[2, j] = BoardGame[3, j];
                                    BoardGame[3, j] = 0;
                                }

                                if(BoardGame[1, j] == BoardGame[2, j])
                                {
                                    BoardGame[1, j] = BoardGame[1, j] * 2;
                                    BoardGame[2, j] = BoardGame[3, j];
                                    BoardGame[3, j] = 0;
                                }

                                if(BoardGame[2, j] == BoardGame [3, j])
                                {
                                    BoardGame[2, j] = BoardGame[2, j] * 2;
                                    BoardGame[3, j] = 0;
                                }
                            }

                            break;

                        case ConsoleKey.DownArrow:
                            error = false;

                            /*D'abord on descends tous les nombres en partant du haut du plateau de jeu une ligne en dessous de la première.
                            On le fait plusieurs fois afin d'éviter qu'un nombre soit perdu au milieu du plateau de jeu.*/

                            while (k < size)
                            {
                                for(int i = 1; i < size; i++)
                                {
                                    for(int j = 0; j < size; j++)
                                    {
                                        if(BoardGame[i, j] == 0 && BoardGame[i -1, j] > 0)
                                        {
                                            BoardGame[i, j] = BoardGame[i - 1, j];
                                            BoardGame[i - 1, j] = 0;
                                        }
                                    }
                                }
                                k++;
                            }

                            // Ensuite, on additionne tous les nombres par colonne depuis le haut du plateau de jeu.
                            // |---------------|
                            // | A OPTIMISER ! |
                            // |---------------|

                            for (int j = 0; j < size; j++)
                            {
                                if (BoardGame[3, j] == BoardGame[2, j])
                                {
                                    BoardGame[3, j] = BoardGame[3, j] * 2;
                                    BoardGame[2, j] = BoardGame[1, j];
                                    BoardGame[1, j] = BoardGame[0, j];
                                    BoardGame[0, j] = 0;
                                }

                                if (BoardGame[2, j] == BoardGame[1, j])
                                {
                                    BoardGame[2, j] = BoardGame[2, j] * 2;
                                    BoardGame[1, j] = BoardGame[0, j];
                                    BoardGame[0, j] = 0;
                                }

                                if (BoardGame[1, j] == BoardGame[0, j])
                                {
                                    BoardGame[1, j] = BoardGame[1, j] * 2;
                                    BoardGame[0, j] = 0;
                                }
                            }

                            break;

                        default:
                            Console.WriteLine("\nErreur !");
                            break;
                    }
                }
                GameTool.Random2Or4(BoardGame, size, tab2Or4);
            }

            Console.Clear();
            Console.WriteLine("Perdu !");
            Console.ReadKey();
        }
    }
}