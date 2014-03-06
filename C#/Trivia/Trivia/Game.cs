using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UglyTrivia
{
    using Trivia;

    public class Game
    {
        private const int WinningScore = 6;

        private List<Player> players = new List<Player>();

        // int[] places = new int[6];
        //int[] purses = new int[6];

        //bool[] inPenaltyBox = new bool[6];

        private LinkedList<string> popQuestions = new LinkedList<string>();

        private LinkedList<string> scienceQuestions = new LinkedList<string>();

        private LinkedList<string> sportsQuestions = new LinkedList<string>();

        private LinkedList<string> rockQuestions = new LinkedList<string>();

        private List<string> board = new List<string>(
            new [] {
                "Science",
                "Sports",
                "Rock",
                "Pop",
                "Science",
                "Sports",
                "Rock",
                "Pop",
                "Science",
                "Sports",
                "Rock",
                "Pop" }); 

        int currentPlayer ;
        bool isGettingOutOfPenaltyBox;

        public Game()
        {
            for (int i = 0; i < 50; i++)
            {
                popQuestions.AddLast("Pop Question " + i);
                scienceQuestions.AddLast(("Science Question " + i));
                sportsQuestions.AddLast(("Sports Question " + i));
                rockQuestions.AddLast(createRockQuestion(i));
            }
        }

        public String createRockQuestion(int index)
        {
            return "Rock Question " + index;
        }

        public bool isPlayable()
        {
            return (howManyPlayers() >= 2);
        }

        public bool add(String playerName)
        {


            players.Add(new Player(playerName));

            Console.WriteLine(playerName + " was added");
            Console.WriteLine("They are player number " + players.Count);
            return true;
        }

        public int howManyPlayers()
        {
            return players.Count;
        }

        public void roll(int roll)
        {
            Console.WriteLine(this.CurrentPlayer + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (CurrentPlayer.IsInPenalty)
            {
                if (CanLeavePenalty(roll))
                {
                    isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(CurrentPlayer.Name + " is getting out of the penalty box");
                    this.MoveCurrentPlayer(roll);

                    Console.WriteLine(CurrentPlayer.Location
                            + "'s new location is "
                            + CurrentPlayer.Location);
                    Console.WriteLine("The category is " + currentCategory());
                    askQuestion();
                }
                else
                {
                    Console.WriteLine(CurrentPlayer.Name + " is not getting out of the penalty box");
                    isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
    
                this.MoveCurrentPlayer(roll);
                Console.WriteLine(CurrentPlayer.Name
                        + "'s new location is "
                        + CurrentPlayer.Location);
                Console.WriteLine("The category is " + currentCategory());
                askQuestion();
            }

        }

        private void MoveCurrentPlayer(int roll)
        {
            this.CurrentPlayer.Location += roll;
            this.CurrentPlayer.Location %=  12;
        }

        private static bool CanLeavePenalty(int roll)
        {
            return roll % 2 != 0;
        }

        private Player CurrentPlayer
        {
            get
            {
                return this.players[this.currentPlayer];
            }
        }

        internal int CurrentPlayerLocation()
        {
            return CurrentPlayer.Location;
        }

        internal int CurrentPlayerScore()
        {
            return CurrentPlayer.Score;
        }

        private void askQuestion()
        {
            if (currentCategory() == "Pop")
            {
                Console.WriteLine(popQuestions.First());
                popQuestions.RemoveFirst();
            }
            if (currentCategory() == "Science")
            {
                Console.WriteLine(scienceQuestions.First());
                scienceQuestions.RemoveFirst();
            }
            if (currentCategory() == "Sports")
            {
                Console.WriteLine(sportsQuestions.First());
                sportsQuestions.RemoveFirst();
            }
            if (currentCategory() == "Rock")
            {
                Console.WriteLine(rockQuestions.First());
                rockQuestions.RemoveFirst();
            }
        }


        internal String currentCategory()
        {
            return board[CurrentPlayer.Location];
        }

        public bool wasCorrectlyAnswered()
        {
            if (CurrentPlayer.IsInPenalty)
            {
                if (isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    CurrentPlayer.Score++;
                    Console.WriteLine(CurrentPlayer
                            + " now has "
                            + CurrentPlayer.Score
                            + " Gold Coins.");

                    bool winner = didPlayerWin();
                    this.MoveToNextPlayer();

                    return winner;
                }
                else
                {
                    this.MoveToNextPlayer();
                    return true;
                }

            }
            else
            {

                Console.WriteLine("Answer was corrent!!!!");
                CurrentPlayer.Score++;
                Console.WriteLine(CurrentPlayer
                        + " now has "
                        + CurrentPlayer.Score
                        + " Gold Coins.");

                bool winner = didPlayerWin();
                this.MoveToNextPlayer();

                return winner;
            }
        }

        public bool wrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(CurrentPlayer + " was sent to the penalty box");
            CurrentPlayer.IsInPenalty = true;

            this.MoveToNextPlayer();
            return true;
        }

        
        private void MoveToNextPlayer()
        {
            this.currentPlayer++;
            if (this.currentPlayer == this.players.Count)
            {
                this.currentPlayer = 0;
            }
        }


        private bool didPlayerWin()
        {
            return !(CurrentPlayer.Score == WinningScore);
        }
    }

}
