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
        private static MediaPlayer playMusic = new MediaPlayer();
        Button[,] userButtons = new Button[10, 10];
        int counterLevel1 = 0;
        int counterLevel2 = 0;
        int counterLevel3 = 0;
        bool possibleToCheck = true;
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
   

        public void computerPlay() { 
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

        public Image creatXImage()
        {
            Image xImage = new Image
            {
                Source = new BitmapImage(new Uri("ship-hit.png", UriKind.Relative)),
            };
        }//end creat image
        public String assignRandom(int counter)
        {
            Random randomNum = new Random();
            //generate a random row
            int col = randomNum.Next(0, 10); //num between 0-9
            int row = randomNum.Next(0, 10); //num between 0-9
            //store those to make sure that we dont user them again
            String point = row + "," + col;

            for(int index = 0; index < counter; index++)
            {
                while (point.Equals(array[index]))
                {
                     col = randomNum.Next(0, 10); //num between 0-9
                     row = randomNum.Next(0, 10); //num between 0-9
                    point = row + "," + col;
                    index=-1;//what if returns the same thing again ask sonya (so you restart) 

                }//end while 
            }//end for 
             ///only when u r out u know those are new points and u add them in to the array
            array[counterLevel1] = point;
            return point;
            }//end method      

        public void computerPlayLevel1()
    {
            counterLevel1++;
            String point = assignRandom(counterLevel1);
            int row = int.Parse(point.Substring(0,point.IndexOf(","))); //0,4 //you want all from 0 until , not included
            int col = int.Parse(point.Substring(point.IndexOf(",")+1)); //so you will have everything from ,+1 meaning the rest of the num

            Button guess = userButtons[row, col];

                if (!(Image)(guess.Content).BaseUri.equals("ImgShip"){ ///compare name or ui 
                    compTurn = false; ///inside the method
                    guess.Content = creatXImage();// or flips ?!?!?! ask sonya
                }

                else
                {///comp found it
                        //image of the ship becomes visible?//what if its the whole ship? 
                        //I would put a fire effect+boom
                        //u have it right sonya
                        // a method that will check if the current button ship, were all of the buttons found ?
                        guess.Content = creatShipImage();
                compTurn = true;//inside the method ?! ask Sonya 
                        checkIfFound();//description above
                }//end else
            }//end method 





















       public void computerPlayLevel2()
        {
            int row;
            int col;
            if (possibleToCheck) {
                String point = assignRandom(counterLevel2);
                row = int.Parse(point.Substring(0, point.IndexOf(","))); //0,4 //you want all from 0 until , not included
                col = int.Parse(point.Substring(point.IndexOf(",") + 1)); //so you will have everything from ,+1 meaning the rest of the num

                Button guess = userButtons[row, col];
                //the starter point 

            }//end starter point


                //compare it
                if (!(Image)(guess.Content).BaseUri.equals("ImgShip"){ ///compare name or ui 
                    compTurn = false; ///inside the method
                    guess.Content = creatXImage();// or flips ?!?!?! ask sonya
                
                
                }
                else
                {///comp found it
                    //image of the ship becomes visible?//what if its the whole ship? 
                    //I would put a fire effect+boom
                    //u have it right sonya
                    // a method that will check if the current button ship, were all of the buttons found ?
                    guess.Content = creatShipImage();
                    compTurn = true;//inside the method ?! ask Sonya 
                    checkIfFound();//description above
                }//end else
          

                //rows check 1 above
                if (row + 1 <= userButtons[row])//check if length, check if not later when deskcheck
                {
                    guess = userButtons[row + 1, col];
                    (!(Image)(guess.Content).BaseUri.equals("ImgShip"){ ///compare name or ui 
                        compTurn = false; ///inside the method
                        guess.Content = creatXImage();// or flips ?!?!?! ask sonya
                    }
                    else
                {///comp found it
                        //image of the ship becomes visible?//what if its the whole ship? 
                        //I would put a fire effect+boom
                        //u have it right sonya
                        // a method that will check if the current button ship, were all of the buttons found ?
                        guess.Content = creatShipImage();
                        compTurn = true;//inside the method ?! ask Sonya 
                        checkIfFound();//description above
                    }//end else
                }


                if (calledLevel2 == 2)
                    calledLevel2++;

                {
                    if (row - 1 >= 0)
                    {
                        guess = userButtons[row - 1, col];
                        (!(Image)(guess.Content).BaseUri.equals("ImgShip"){ ///compare name or ui 
                            compTurn = false; ///inside the method
                            guess.Content = creatXImage();// or flips ?!?!?! ask sonya
                        }
                        else
                {///comp found it
                            //image of the ship becomes visible?//what if its the whole ship? 
                            //I would put a fire effect+boom
                            //u have it right sonya
                            // a method that will check if the current button ship, were all of the buttons found ?
                            guess.Content = creatShipImage();
                            compTurn = true;//inside the method ?! ask Sonya 
                            checkIfFound();//description above
                        }//end else
                    }

        }//end computerPlayLevel2






























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
