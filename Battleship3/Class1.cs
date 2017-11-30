﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship3
{
    class Class1
    {
        ﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Battleship2
    {
        /// <summary>
        /// Interaction logic for MainWindow.xaml
        /// </summary>
        public partial class MainWindow : Window
        {
            Button[,] userButtons = new Button[10, 10];

            // Create ship objects
            Ship a = new Ship(2, false);
            Ship b = new Ship(3, false);
            Ship c = new Ship(4, false);
            Ship d = new Ship(5, true);
            Ship e = new Ship(6, true);
            List<Ship> ships = new List<Ship>();

            private Boolean gameStarted = false;

            //Generate time
            DispatcherTimer timer;

            //Holds time
            int time;

            //Keeps the initial time
            int initialTime;

            //Indicate to active board
            Grid playedBoard;

            public MainWindow()
            {
                InitializeComponent();
                initialSetUps();
            }



            private void initialSetUps()
            {
                Time_Lbl.Content = "8";
                // fill children elements(button) of computer grid with noship/or water
                for (int i = 0; i < 100; i++)
                {
                    ((Button)computerGrid.Children[i]).Content = "noShip";
                }

                randomSetupComputerPlayer();
            }
            // create ships and place them randomly inside computer player grid
            private void randomSetupComputerPlayer()
            {
                Random random = new Random();
                int[] shipSizes = new int[] { 2, 3, 4, 5, 6 };
                string[] shipNames = new string[] { "a", "b", "c", "d", "e" };
                int size, index;
                string shipName;
                bool isHorizontal;
                bool alreadyTakenIndex = true;

                for (int i = 0; i < shipSizes.Length; i++)
                {
                    //Set size and ship type
                    size = shipSizes[i];
                    shipName = shipNames[i];
                    alreadyTakenIndex = true;

                    if (random.Next(0, 2) == 0)
                        isHorizontal = true;
                    else
                        isHorizontal = false;

                    //Set ships
                    if (isHorizontal)
                    {
                        index = random.Next(0, 100);
                        while (alreadyTakenIndex == true)
                        {
                            alreadyTakenIndex = false;
                            // check if the place available for the ship size which is randomly chosen
                            while ((index + size - 1) % 10 < size - 1)
                            {
                                index = random.Next(0, 100);
                            }
                            // check if these place is already filled or they are free .
                            for (int j = 0; j < size; j++)
                            {
                                if (index + j > 99 || !((Button)computerGrid.Children[index + j]).Content.Equals("noShip"))
                                {
                                    index = random.Next(0, 100);
                                    alreadyTakenIndex = true;
                                    break;
                                }
                            }
                        }
                        // now we can place the ship 
                        for (int j = 0; j < size; j++)
                        {
                            ((Button)computerGrid.Children[index + j]).Content = shipName;
                        }
                    }
                    else
                    {
                        index = random.Next(0, 100);
                        while (alreadyTakenIndex == true)
                        {
                            alreadyTakenIndex = false;

                            while (index / 10 + size * 10 > 100)
                            {
                                index = random.Next(0, 100);
                            }

                            for (int j = 0; j < size * 10; j += 10)
                            {
                                if (index + j > 99 || !((Button)computerGrid.Children[index + j]).Content.Equals("noShip"))
                                {
                                    index = random.Next(0, 100);
                                    alreadyTakenIndex = true;
                                    break;
                                }
                            }
                        }
                        for (int j = 0; j < size * 10; j += 10)
                        {
                            ((Button)computerGrid.Children[index + j]).Content = shipName;
                        }
                    }

                }


            }// End RandomComputerSetup

            private void userSetUps()
            {
                // fill children elements(button) of computer grid with noship/or water
                for (int i = 0; i < 100; i++)
                {
                    ((Button)userGrid.Children[i]).Content = "noShip";
                }

                // Total number of ships
                int numShips = 0;
                // Create 2D array of Buttons
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        userButtons[i, j] = (Button)userGrid.Children[numShips];

                        numShips++;
                    }
                }

                ships.Add(a);
                ships.Add(b);
                ships.Add(c);
                ships.Add(d);
                ships.Add(e);



            }// End userSetUps method

            //@author Farzaneh
            //@version November 26, 2017

            private void startBtn_Click(object sender, RoutedEventArgs e)
            {
                //Generate a random number to choose the player who starts the game.
                Random random = new Random();
                int turn = random.Next(0, 10) % 2;

                // If random number is even the computer will start.
                if (turn == 0)
                {
                    playedBoard = userGrid;
                    Disable_Board(computerGrid);
                }
                else
                {
                    playedBoard = computerGrid;
                    Disable_Board(userGrid);
                }

                if (int.TryParse(Time_Lbl.Content.ToString(), out time))
                {
                    initialTime = time;
                }
                startBtn.IsEnabled = false;
                Time_plus_Btn.IsEnabled = false;
                Time_minus_Btn.IsEnabled = false;
                Level_MinusBtn.IsEnabled = false;
                Level_PlusBtn.IsEnabled = false;
                Load_Btn.IsEnabled = false;
                timer = new DispatcherTimer();
                timer.Tick += Timer_Tick;
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Start();
            }

            //Manage time interval for player. Countdown from setting time, if player doesn't play in this interval it will change the turn.
            //@author Farzaneh
            //@version November 26, 2017
            private void Timer_Tick(Object sender, EventArgs e)
            {
                Time_Lbl.Content = Convert.ToString(--time);

                if (time == 0)
                {
                    Reset_Timer();
                    Change_Player();
                }
            }//end Timer_Tick

            //Reset timer to its initial time
            //@author Farzaneh
            //@version November 26, 2017
            private void Reset_Timer()
            {
                time = initialTime + 1;
                timer.Start();
            }// end resetTimer


            //@author Farzaneh
            //@Version November 25, 2017
            private void Change_Player()
            {
                if (playedBoard.Equals(userGrid))
                {
                    playedBoard = computerGrid;
                    Enabl_eBoard(computerGrid);
                    Disable_Board(userGrid);
                }
                else
                {
                    playedBoard = userGrid;
                    Enabl_eBoard(userGrid);
                    Disable_Board(computerGrid);
                }
            }//end Change_Player


            //@author Frazaneh
            //@version November 25, 2017
            private void Enabl_eBoard(Grid grid)
            {
                foreach (Button btn in grid.Children)
                {
                    btn.IsEnabled = true;
                }
            }


            //@author Farzaneh
            //@version November25, 2017
            private void Disable_Board(Grid grid)
            {
                foreach (Button btn in grid.Children)
                {
                    btn.IsEnabled = false;
                }
            }


            //@author Farzaneh
            //@version November 26, 2017
            private void BtnComp_Click(object sender, RoutedEventArgs e)
            {

                Button btn = (Button)sender;
                if (!(bool)startBtn.IsEnabled)
                {
                    btn.IsEnabled = false;
                    if (!(btn.Content.ToString().Equals("noShip")))
                    {


                        Set_Image(btn, "ship-hit.png");
                        Reset_Timer();
                    }
                    else
                    {
                        Set_Image(btn, "ship-missed.png");
                        Reset_Timer();
                        Change_Player();
                    }
                }
            }//end BtnCom_Click

            //Set proper image for the clicked button
            //@author Farzaneh
            //@version November 26, 2017
            private void Set_Image(Button btn, String imgName)
            {

                string imagePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\" + imgName;
                btn.Content = new Image
                {
                    Source = new BitmapImage(new Uri(@imagePath, UriKind.RelativeOrAbsolute)),
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.Fill
                };
            }// end set_Image
        }// End MainWindow class

        public class Ship
        {
            int size;
            Boolean isHorizontal;

            public Ship(int size, Boolean isHorizontal)
            {
                this.size = size;
                this.isHorizontal = isHorizontal;
            }

            public int GetSize()
            {
                return this.size;
            }

            public Boolean GetDirection()
            {
                return this.isHorizontal;
            }
        }// End Ship class
    }

}
}