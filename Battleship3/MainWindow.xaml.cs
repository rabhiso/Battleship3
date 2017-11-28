using System;
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
using System.Collections;

namespace Battleship2
{
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

        public MainWindow()
        {
            InitializeComponent();
            initialSetUps();
            userSetUps();
        }

        private void initialSetUps()
        {
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

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            
            if (!(gameStarted))
            {
                Boolean isPlaced = false;
                
                Button btnClicked = (Button)sender;
                isPlaced = PlaceShip(ships[0], btnClicked);
                    
                if(isPlaced)
                {
                    ships.RemoveAt(0);
                    if(ships.Count == 0)
                    {
                        MessageBox.Show("All ships placed! Click the start button!");
                    }
                }

            } // end if (!gameStarted)
        }// end BtnUser_Click event handler

        private Boolean PlaceShip(Ship ship, Button btnClicked)
        {
            Boolean isPlaced = false;
            // Check if this button is "free" (no content)
            if ( !(btnClicked.Content.Equals("noShip")) )
            {
                MessageBox.Show("The ship does not fit there!");
            }

            // Check the position of the button (returns the i and j position of the button in the 2D array)
            int i = 0;
            int j = 0;
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            for (int index1 = 0; index1 < 10; index1++)
            {
                for (int index2 = 0; index2 < 10; index2++)
                {
                    if (userButtons[index1, index2].Name.Equals(btnClicked.Name))
                    {
                        i = index1;
                        j = index2;
                    }
                }
            }

            // MessageBox.Show("i is: " + i.ToString() + "j is: " +  j.ToString() + "btnclickd name: " + btnClicked.Name + "userbutton at 55" + userButtons[5, 5].Name);

            // Check orientation of the ship
            Boolean isHorizontal = ship.GetDirection();
            Boolean canPlace = true;
           
            if (isHorizontal)
            {
                //Check if there is enough space on the graph
                if ( (j + ship.GetSize() - 1) <= 9)
                {
                    // Check if following buttons are available
                    for (int index = j + 1; index <= j + ship.GetSize() - 1; index++)
                    {
                        if ( !(userButtons[i, index].Content.Equals("noShip")) )
                        {
                            canPlace = false;
                            break;
                        }
                    }

                    if (canPlace)
                    {
                        for (int index = j; index <= j + ship.GetSize() - 1; index++)
                        {
                            userButtons[i, index].Content = "d";
                            
                        }
                        isPlaced = true;
                        return isPlaced;
                    }

                }// If enough space on graph
                else
                {
                    return isPlaced = false;
                }
            }
            else // IF vertical
            {
                //Check if there is enough space on the graph
                if ((i + ship.GetSize() - 1) <= 9)
                {
                    // Check if following buttons are available
                    for (int index = i + 1; index <= i + ship.GetSize() - 1; index++)
                    {
                        if (!(userButtons[index, j].Content.Equals("noShip")))
                        {
                            canPlace = false;
                            break;
                        }
                    }

                    if (canPlace)
                    {
                        for (int index = i; index <= i + ship.GetSize() - 1; index++)
                        {
                            userButtons[index, j].Content = "a";
                            
                        }
                        isPlaced = true;
                        return isPlaced;
                    }

                }// If enough space on graph
                else
                {
                    return isPlaced = false;
                }
            }

            return isPlaced;
        }// end PlaceShip method

        private void BtnComp_Click(object sender, RoutedEventArgs e)
        {
            gameStarted = true;
        }
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
