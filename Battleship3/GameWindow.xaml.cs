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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Battleship3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        //general
        private static MediaPlayer playMusic = new MediaPlayer();
        Button[,] userButtons = new Button[10, 10];

/// <summary>
/// levels , counter that places points within the array 
/// </summary>
        int counterPointsFound = 0; //should have counter for every method?


        Boolean possibleToCheckTop=false; //by default
        Boolean possibleToCheckButtom=false; //by default


        String[] array = new String[100]; //you will end up filing it until 100

        //the user will start first
        bool userTurn = true;
        bool compTurn = false;
        public GameWindow()
        {
            InitializeComponent();
            initialSetUps();
            Music.PlayBackgroundMusic();
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

            // Create ship objects
            Ship a = new Ship(2, false);
            Ship b = new Ship(3, false);
            Ship c = new Ship(4, false);
            Ship d = new Ship(5, true);
            Ship e = new Ship(6, true);



        }// End userSetUps method

        public void computerPlay()
        {
            //will invoke the method according to the level seleced
            if (level1.IsChecked == true)
            {
                computerPlayLevel1();
            }




            if (level2.IsChecked == true)
            {
                computerPlayLevel2();
            }




            if (level3.IsChecked == true)
            {
                computerPlayLevel3();




            }




        }// end computerPlay




        public Boolean seeIfRepeated(String point, Boolean result, int counter)
        {
            for (int index = 0; index < counter; index++)
            {
                if (point.Equals(array[index]))
                {
                    result = false;
                }

                else
                {
                    result = true;
                }
            }//end for loop


            return result;
        }//end method








        public String assignRandom(int counter)
        {
            Random randomNum = new Random();
            //generate a random row
            int col = randomNum.Next(0, 10); //num between 0-9
            int row = randomNum.Next(0, 10); //num between 0-9
            //store those to make sure that we dont user them again
            String point = row + "," + col;
           Boolean result = true;//assuming                                          //bool vs Bool difference


            //the actual result 
            result = seeIfRepeated(point, result, counter);


            while (!result)
            {
                col = randomNum.Next(0, 10); //num between 0-9
                row = randomNum.Next(0, 10); //num between 0-9
                point = row + "," + col;
                result = seeIfRepeated(point, result, counter);
            }//end while 

            ///only when u r out u know those are new points and u add  them in to the array
            array[counter] = point;
            return point;
        }//end assign random  




        public void computerPlayLevel1()
        {


            String point = assignRandom(counterPointsFound);
            int row = int.Parse(point.Substring(0, point.IndexOf(","))); //0,4 //you want all from 0 until , not included
            int col = int.Parse(point.Substring(point.IndexOf(",") + 1)); //so you will have everything from ,+1 meaning the rest of the num
            counterPointsFound++;


            Button guess = userButtons[row, col];


            if (!(guess.Content).ToString().Equals("ImgShip"))
            {
                Change_Player();
                Reset_Timer();
            }




            else
            {///comp found it
                //image of the ship becomes visible?
                //what if its the whole ship? //I would put a fire effect+boom
                guess.IsEnabled = false;
                /////////I think calling disabelling the board should happened out side of this method
                //because if the computer finds the image, it could play again,
                //we must call it from the outside 


            }//end else
        }//end level1

      



        public void computerPlayLevel2()
    {
//assign a random colom to search at
        String point = assignRandom(counterPointsFound);
        int  row = int.Parse(point.Substring(0, point.IndexOf(","))); //0,4 //you want all from 0 until , not included
        int  col = int.Parse(point.Substring(point.IndexOf(",") + 1)); //so you will have everything from ,+1 meaning the rest of the num
        counterPointsFound++;

            //check if buttom 
            if (row + 1 < userButtons.Length)
            {
                possibleToCheckButtom = true;

            }
            else
            {
                possibleToCheckButtom = false;

            }
            
            //check if top
            if (row - 1 >= 0)
            {
                possibleToCheckTop = true;

            }

            else
            {
                possibleToCheckTop = false;
            }

            Button guess = userButtons[row, col];

        //test intially
        if (!(guess.Content).ToString().Equals("ImgShip"))
        {
            Change_Player();
            Reset_Timer();

                //get out of this method 
        }

        else
        {///comp found it
            //still my turn
            //reset timer anyways(?)


                //check either top or buttom 
            if (possibleToCheckTop)
            {
                //must add this point to the array, dont ever wanna check it again
                guess = userButtons[row-1][col];
               counterPointsFound++;//another point found
                 point = row - 1 + "," + col;
                array[counterPointsFound]=point;//since u are checking a new point a new point, make sure that random will not generate it 

                if (!(guess.Content).ToString().Equals("ImgShip"))
                {
                   
                    Change_Player();
                    Reset_Timer();
                    //supposed to get out of the method
                    //image to x
                }

                else
                {
                    if (possibleToCheckButtom)
                    {
                        guess = userButtons[row+1][col];
                        counterPointsFound++;//another point found
                        point = row + 1 + "," + col;
                        array[counterPointsFound] = point;//since u added a new point


                        if (!(guess.Content).ToString().Equals("ImgShip"))
                        {
                            Change_Player();
                            Reset_Timer();
                            //change image to X
                            //get out of the loop
                        }

                        else
                        {
                            //you found the whole ship 
                            //has to do with ships lenght if its 2 so would win before
                        }
                    }
                }//if cant check buttom not gonna enter here 
            }//end possible to checkTop

            else
            {
                //check buttom only cuz cant search on
                guess = userButtons[row + 1][col];
                counterPointsFound++;//another point found
                 point = row + 1 + "," + col;
                array[counterPointsFound] = point;//since u added a new point


                if (!(guess.Content).ToString().Equals("ImgShip"))
                {
                    Change_Player();
                    Reset_Timer();
                    //change image to X
                }
                else
                {
                    //you found it 
                    //still change_Player() since there is no more searching done
                    

                }

            }

        }//end else

    }//end computerPlayLevel2




        //note:
        //else block , change image to x
        //get out of the loop and change the player bord via variable .. compare with girls
        //if is in the last else has found
        //or second else                         possibility of finding a ship.must check the length.



















public void computerPlayLevel3()
{
    computerPlayLevel2();
    //than logic of both sides -friday
}



























        public void compterPlayLevel3()
        {

        }//end computerPlayLevel3


        public void decideTimer()
    {
            /**
             * to do 
             **/
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
