using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Hexapawn
{
    class MainClass
    {
        public static int[] playerNum = new int[9];
        public static UI ui = new UI();
        public static AI ai = new AI();
        public static Process process = new Process();
        static void Main(string[] args)
        {
            //initialize values
            playerNum = new int[9] { 1, 1, 1, 0, 0, 0, 2, 2, 2 };
            int winsUser=0, winsAI=0;
            int mechanicsConfirm;
            Console.SetWindowSize(56, 40);
            if (ui.Welcome_Prompt())
            {
                mechanicsConfirm = ui.Mechanics_Prompt();
                while (mechanicsConfirm == -1)

                {
                    mechanicsConfirm = ui.Mechanics_Prompt();
                }
                if (mechanicsConfirm == 1)
                {
                    int returnCode;
                    try
                    {
                        using (StreamReader sr = new StreamReader("data.txt"))
                        {
                            string line;
                            int lineNum = 1;
                            while ((line = sr.ReadLine()) != null)
                            {
                                if(lineNum % 2 == 0)
                                {
                                    if(line == "True")
                                    {
                                        ai.AI_Decision_isGood.AddLast(true);
                                    }
                                    else
                                    {
                                        ai.AI_Decision_isGood.AddLast(false);
                                    }
                                    lineNum++;
                                }
                                else
                                {
                                    ai.AI_Memory.AddLast(line);
                                    lineNum++;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The file could not be read:");
                        Console.WriteLine(e.Message);
                    }
                    while (true)
                    {
                        returnCode = process.Start();
                        playerNum = new int[9] { 1, 1, 1, 0, 0, 0, 2, 2, 2 };
                        if(returnCode == 1)
                        {
                            winsAI++;
                        }
                        else if(returnCode == 2)
                        {
                            winsUser++;
                            ai.SetBad();
                        }
                        Console.WriteLine(returnCode==1?"AI Wins!!":"You Win, Congratulations");
                        Console.WriteLine("\nCurrent ScoreBoard\n\tYou: " + winsUser.ToString() + "\n\tAI: " + winsAI.ToString());
                        Console.WriteLine("\nRound Done! Thats a lovely game. \nThe AI learned from that\nPress any key to continue and \'x\' to exit");
                        string[] s = new string[ai.AI_Memory.Count];
                        bool[] x = new bool[ai.AI_Decision_isGood.Count];
                        s = ai.AI_Memory.ToArray();
                        x = ai.AI_Decision_isGood.ToArray();
                        if (Console.ReadKey().KeyChar == 'x') {
                            break;
                        } 
                    }

                }
            }
            new MainClass().exit();
            Console.ReadKey();
        }
        void exit()
        {
            string[] AIM = new string[ai.AI_Memory.Count];
            bool[] AID = new bool[ai.AI_Decision_isGood.Count];
            ai.AI_Decision_isGood.CopyTo(AID, 0);
            ai.AI_Memory.CopyTo(AIM, 0);
            using (StreamWriter sw = new StreamWriter("data.txt", true))
            {
                for(int a = 0; a < ai.AI_Memory.Count; a++)
                {
                    sw.WriteLine(AIM[a],true);
                    sw.WriteLine(AID[a].ToString(),true);
                }
            }
            Console.Clear();
            Console.WriteLine("Thanks for playing!!\nGlad you didn't start an AI rebellion. See You Soon!");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
    class UI:MainClass
    { 
        public UI()
        {
     
        }
        public void DisplayBoard()
        {
            DisplayBoard(playerNum);
        }
        public void DisplayBoard(int[] playerNum)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.WriteLine("     A     B     C  ");
            Console.WriteLine("    _________________");
            Console.WriteLine("   |     |     |     |");
            Console.Write("3  | ");
            for (int c = 1; c <= playerNum.Length; c++)
            {
                SetPosition(playerNum[c - 1]);
                if (c % 3 != 0 || c == playerNum.Length)
                {
                    Console.Write(" | ");
                }
                else
                {
                    Console.WriteLine(" |");
                    Console.WriteLine("   |_____|_____|_____|");
                    Console.WriteLine("   |     |     |     |");
                    Console.Write(3 - (c / 3) + "  | ");
                }
            }
            Console.WriteLine("\n   |_____|_____|_____|");

        }
        void SetPosition(int player)
        {
            Console.ForegroundColor = player == 1 ? ConsoleColor.Red : player == 2 ? ConsoleColor.Blue : ConsoleColor.Black;
            Console.Write("███");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public bool Welcome_Prompt()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.Cyan;
            //Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("\t                                         \t");
            Console.WriteLine("\t  _____________________________________  \t");
            Console.WriteLine("\t //                                   \\\\\t");
            Console.WriteLine("\t||               THIS IS               ||\t");
            Console.WriteLine("\t||            H E X A P A W N          ||\t");
            Console.WriteLine("\t||         The Game That Learns        ||\t");
            Console.WriteLine("\t||                                     ||\t");
            Console.WriteLine("\t||      Created & Developed by: 3em    ||\t");
            Console.WriteLine("\t||                                     ||\t");
            Console.WriteLine("\t \\\\===================================//\t");
            Console.WriteLine("\t\t\t\t\t\t\t");
            Console.WriteLine("********************************************************");
            Console.WriteLine("                 [Welcome To Hexapawn]                  ");
            Console.WriteLine("    This is a game of wit where you will be battling    ");
            Console.WriteLine(" against a computer capable of learning every move you  ");
            Console.WriteLine("       make, making it potentially undefeatable and     ");
            Console.WriteLine("          unstoppable as the game progresses by.        ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("     By playing this game, you agree to never blame     ");
            Console.WriteLine("  me when AIs takes over our race and eventually lead   ");
            Console.WriteLine("       our fate to the destruction of our very own      ");
            Console.WriteLine("                  civilization                          ");
            Console.WriteLine("                                                        ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("            Proceed with caution. Please!!!             ");
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine("********************************************************");
            Console.WriteLine("    With that in mind, would you still want to play?    ");
            Console.WriteLine("type \'y\' if you want to proceed and \'n\' if not          ");
            Console.Write(">> ");
            if (Console.ReadLine() == "y")
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
                return true;
            }
            else
            {
                return false;
            }
        }
        public int Mechanics_Prompt()
        {
            Console.Clear();
            int[] a = new int[9];
            Console.WriteLine("\t\t   How to play?");
            Console.WriteLine("The Rules and mechanics are simple and if you played");
            Console.WriteLine("chess before then this wouldn't be a pronblem. ");
            Console.WriteLine("\n  1. Your opponent will be the computer and you will");
            Console.WriteLine("     be the first to play (why? because I said so).");
            Console.WriteLine("  2. Your pieces are the blue ones. The moves of each");
            Console.WriteLine("     piece is only forward (Ex. piece A1 moves to A2)");
            Array.Copy(playerNum, a, 9);
            a[6] = 0; a[3] = 2;
            DisplayBoard(a);
            Console.WriteLine("  3. You can only move diagonally when capturing an");
            Console.WriteLine("     opponent's piece. (Ex. piece A1 captures B2)");
            Array.Copy(playerNum, a, 9);
            a[1] = 0; a[4] = 1;
            DisplayBoard(a);
            Console.WriteLine("  4. You can't move forward if there is already an");
            Console.WriteLine("     existing piece infront. You can't also move");
            Console.WriteLine("     backwards (Ex. above: B1 can't move to B2)");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("\t\t     How to win?");
            Console.WriteLine("  1. If one of your piece reach the base of the opponent");
            Console.WriteLine("(Ex. C3)");
            a[0] = 0; a[2] = 2; a[3] = 1; a[7] = 0;
            DisplayBoard(a);
            Array.Copy(playerNum, a, 9);
            Console.WriteLine("  2. If you've ran out the moves of the opponent");
            a[1] = 0; a[3] = 2; a[4] = 1; a[5] = 2; a[6] = 0; a[8] = 0;
            DisplayBoard(a);
            Array.Copy(playerNum, a, 9);
            Console.WriteLine("  3. If you captured all of the opponent's piece");
            Array.Clear(a, 0, 9);
            a[4] = 2; a[7] = 2; a[8] = 2;
            DisplayBoard(a);
            Console.WriteLine("Proceed to Game? (Type \'y\' if yes \'p\' to read");
            Console.Write("previous instructions and \'n\' to exit): ");
            string s = Console.ReadLine();
            if (s == "y")
            {
                return 1;
            }
            else if (s == "p")
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
    class Process : MainClass
    {
        public string[] positionString = new string[9] { "a3", "b3", "c3", "a2", "b2", "c2", "a1", "b1", "c1" };
        public void Program()
        {
            Start();
        }
        public int Start()
        {
            Console.Clear();
            string sourceGridNum, destinationGridNum;
            ui.DisplayBoard();
            int exitCode;
            Console.WriteLine("You will be the first to play");
            while (true)
            {
                Console.WriteLine("Enter the grid number of the piece you want");
                Console.Write("to move (e.g. A1, B2, C3): ");
                Stopwatch stopwatch = new Stopwatch();
                while ((sourceGridNum = ValidateGridNumber(ValidateInput())) == "0")
                {
                    continue;
                }
                Console.WriteLine("\nChoose the coordinate to where you want to");
                Console.Write("move that piece: ");

                while ((destinationGridNum = ValidateGridNumber(ValidateInput(), sourceGridNum.ToLower())) == "0")
                {
                    continue;
                }
                playerNum[Array.IndexOf(positionString, sourceGridNum)] = 0;
                playerNum[Array.IndexOf(positionString, destinationGridNum)] = 2;
                if(isRoundDone(2) != 0)
                { 
                    exitCode = isRoundDone(2);
                    break;
                }
                stopwatch.Start();
                Random rand = new Random();
                int counter = 0;
                ui.DisplayBoard();
                Console.Write("AI is Thinking");
                while (true)
                {
                    if (stopwatch.Elapsed.Seconds == rand.Next(4) + 2)
                    {
                        break;
                    }
                    if (stopwatch.Elapsed.Milliseconds % 100 == 0)
                    {
                        counter++;
                    }
                    switch (counter % 4)
                    {
                        case 0: Console.Write(".   "); break;
                        case 1: Console.Write("..  "); break;
                        case 2: Console.Write("... "); break;
                        case 3: Console.Write("...."); break;
                    }
                    Console.CursorVisible = false;
                    Console.SetCursorPos­ition(Console.Cursor­Left - 4, Console.CursorTop);
                }
                Console.CursorVisible = true;
                stopwatch.Stop();
                string ai_init_Move = ai.InitiateMove();
                Console.Clear();
                if (ai_init_Move != "None")
                {
                    string ai_Move = ai.InitiateMove().Split(' ')[1];
                    playerNum[Array.IndexOf(positionString, ai_Move.Split('-')[0])] = 0;
                    playerNum[Array.IndexOf(positionString, ai_Move.Split('-')[1])] = 1;
                    Console.WriteLine("AI decided to move his piece " + ai_Move.Split('-')[0] + " to "+ ai_Move.Split('-')[1]);
                    ui.DisplayBoard();
                    exitCode = isRoundDone(1);
                }
                else
                {
                    exitCode = 2;
                    break;
                }
                if (exitCode != 0)
                {
                    return exitCode;
                }
            }
            return exitCode;
        }
        private int ValidateInput()
        {
            string input = Console.ReadLine();
            int index = Array.IndexOf(positionString, input.ToLower());
            while (index == -1)
            {
                Console.Write("Invalid coordinate, Enter the coordinate agan: ");
                input = Console.ReadLine();
                index = Array.IndexOf(positionString, input.ToLower());
            }
            return index;
        }
        private string ValidateGridNumber(int gridNum)
        {
            if (playerNum[gridNum] != 2)
            {
                Console.WriteLine("Invalid Input!\nSelected Piece is not yours or empty\nInput Again: ");
                return "0";
            }
            else
            {
                return positionString[gridNum];
            }
        }
        private string ValidateGridNumber(int gridNum, string sourceGrid)
        {
            bool isAdjacentVar = isAdjacent(gridNum, sourceGrid);
            bool isFrontVar = isFront(gridNum, sourceGrid);
            if (isAdjacentVar || isFrontVar)
            {
                if (isAdjacentVar)
                {
                    if (playerNum[gridNum] == 1)
                    {
                        return positionString[gridNum];
                    }
                    else
                    {
                        Console.WriteLine("Invalid Capture or Move\nTry again: ");
                        return "0";
                    }
                }
                else
                {
                    if (playerNum[gridNum] == 0)
                    {
                        return positionString[gridNum];
                    }
                    else
                    {
                        Console.WriteLine("Selected Grid not Empty\n Try Again:");
                        return "0";
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid move\nTry again: ");
                return "0";
            }
        }
        private bool isAdjacent(int distGrid, string sourceGrid)
        {
            int index = Array.IndexOf(positionString, sourceGrid);
            int adj1 = -1, adj2 = -1;
            switch (index)
            {
                case 3:
                    adj1 = 1;
                    break;
                case 4:
                    adj1 = 0;
                    adj2 = 2;
                    break;
                case 5:
                    adj1 = 1;
                    break;
                case 6:
                    adj1 = 4;
                    break;
                case 7:
                    adj1 = 3;
                    adj2 = 5;
                    break;
                case 8:
                    adj1 = 4;
                    break;
            }
            if (distGrid == adj1 || distGrid == adj2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool isFront(int distGrid, string sourceGrid)
        {
            int index = Array.IndexOf(positionString, sourceGrid);
            return index - 3 == distGrid ? true : false;
        }
        private string LeftAdjacent(int index)
        {
            string result = "";
            if (index == 2 || index == 5 || index == 1|| index == 4)
            {
                if (playerNum[index + 2] == 2)
                {
                    result += positionString[index] + "-" + positionString[index + 2];
                }
            }
            return result;
        }
        private string RightAdjacent(int index)
        {
            string result = "";
            if (index == 0 || index == 3 || index == 1 || index == 4)
            {
                if (playerNum[index + 4] == 2)
                {
                    result += positionString[index] + "-" + positionString[index + 4];
                }
            }
            return result;
        }
        private string infront(int index)
        {
            string result = "";
            if (playerNum[index + 3] == 0) {
                result = positionString[index] + "-" + positionString[index + 3];
            }
            return result;
        }
        private bool UserHaveMoves()
        {
            for (int j = 3; j < 9; j++)
            {
                if (playerNum[j] == 2)
                {
                    if (playerNum[j - 3] == 0)
                    {
                        return true;
                    }
                    else if (j == 8 || j == 5 || j == 4 || j == 7)
                    {
                        if(playerNum[j-4] == 1)
                        {
                            return true;
                        }
                    }
                    else if (j == 6 || j == 3 || j == 4 || j == 7)
                    {
                        if (playerNum[j - 2] == 1)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
            return false;
        }
        public LinkedList<string> fetchAvailableMoves()
        {
            LinkedList<string> list = new LinkedList<string>();
            for(int index = 0; index < 5; index++)
            {
                if (playerNum[index] == 1)
                {
                    if (infront(index) != "")
                    {
                        list.AddLast(infront(index));
                    }
                    if (LeftAdjacent(index) != "")
                    {
                        list.AddLast(LeftAdjacent(index));
                    }
                    if (RightAdjacent(index) != "")
                    {
                        list.AddLast(RightAdjacent(index));
                    }
                }
            }
            return list;
        }
        public int isRoundDone(int player)
        {
            int exitcode = 0;
            int[] playerNumCopy = new int[9];
            Array.Copy(playerNum, playerNumCopy, 9);
            bool player2wins = (playerNumCopy[0] == 2 || playerNumCopy[1] == 2 || playerNumCopy[2] == 2 || Array.IndexOf(playerNumCopy, 1) == -1 || (fetchAvailableMoves().Count < 1));
            bool player1wins = (playerNumCopy[6] == 1 || playerNumCopy[7] == 1 || playerNumCopy[8] == 1 || Array.IndexOf(playerNumCopy, 2) == -1 || UserHaveMoves() == false);
            if (player == 1)
            {
                if (player1wins == true)
                {
                    exitcode = 1;
                    Console.Clear();
                    ui.DisplayBoard();
                }
            }
            else
            {
                if (player2wins == true)
                {
                    exitcode = 2;
                    Console.Clear();
                    ui.DisplayBoard();
                }
            }
            return exitcode;
        }
    }
    class AI:Process
    { 
        public LinkedList<string> AI_Memory = new LinkedList<string>();
        public LinkedList<bool> AI_Decision_isGood = new LinkedList<bool>();
        public string MemoFragment = "";

        public string selectRandomMove()
        {
            LinkedList<string> listOfMoves = new LinkedList<string>();
            listOfMoves = fetchAvailableMoves();
            string[] ArrayOfMoves = new string[listOfMoves.Count];
            ArrayOfMoves = listOfMoves.ToArray();
            Random random = new Random();
            try
            {
                return ArrayOfMoves[random.Next(ArrayOfMoves.Length)];
            }
            catch(Exception e)
            {
                return "None";
            }
        }
        public string InitiateMove()
        {
            int[] playerNumCopy = new int[9];
            Array.Copy(playerNum, playerNumCopy, 9);
            string board = "";
            string MemFragment="";
            foreach (int j in playerNumCopy)
            {
                board += j.ToString();
            }
            bool isDone = false;
            while (true)
            {
                string randmove = selectRandomMove();
                if(randmove == "None")
                {
                    isDone = true;
                    break;
                }
                MemFragment = board + " " + randmove;
                if (AI_Memory.Contains(MemFragment))
                {
                    int index = 0;
                    foreach(string s in AI_Memory)
                    {
                        if(s == MemFragment)
                        { 
                            break;
                        }
                        index++;
                    }
                    int x = 0;
                    bool isGood = false;
                    foreach(bool s in AI_Decision_isGood)
                    {
                        isGood = s;
                        if(x == index)
                        {
                            break;
                        }
                        x++;
                    }
                    if (isGood)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    AI_Memory.AddLast(MemFragment);
                    AI_Decision_isGood.AddLast(true);
                    break;
                }
            }
            if (isDone)
            {
                return "None";
            }
            else
            {
                MemoFragment = MemFragment;
                return MemFragment;
            }
        }
        public void SetBad()
        {
            int indM = 0;
            foreach (string s in ai.AI_Memory)
            {
                if (s == ai.MemoFragment)
                {
                    Console.WriteLine("Found " + s);
                    break;
                }
                indM++;
            }
            bool[] D = new bool[AI_Decision_isG­ood.Count];
            AI_Decision_isGood.C­opyTo(D, 0);
            try
            {
                D[indM] = false;
            }
            catch (Exception e)
            {
                AI_Decision_isGoo­d.Last.Value = false;
            }

            AI_Decision_isGood.C­lear();
            foreach (bool x in D)
            {
                AI_Decision_isGood.A­ddLast(x);
            }
        }
    }
}   