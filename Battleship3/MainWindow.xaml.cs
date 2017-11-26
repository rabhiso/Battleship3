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

namespace Battleship2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MediaPlayer playMusic = new MediaPlayer();
        Button[,] userButtons = new Button[10, 10];

        public MainWindow()
        {
            InitializeComponent();
            initialSetUps();
            PlayBackgroundMusic();
            MoveTo(dolphinoimage, 200, 7);
        }

       
            public static void MoveTo(this Image target, double newX, double newY)
            {
                var top = Canvas.GetTop(target);
                var left = Canvas.GetLeft(target);
                TranslateTransform trans = new TranslateTransform(); //SHTO eta?
                target.RenderTransform = trans;
                DoubleAnimation anim1 = new DoubleAnimation(top, newY - top, TimeSpan.FromSeconds(10));
                DoubleAnimation anim2 = new DoubleAnimation(left, newX - left, TimeSpan.FromSeconds(10));
                trans.BeginAnimation(TranslateTransform.XProperty, anim1);
                trans.BeginAnimation(TranslateTransform.YProperty, anim2);
            }
            public static void PlayBackgroundMusic()
        { 
            Uri backgroundMusic = new Uri("backgroundMusic.mp3", UriKind.Relative);
            var player = new MediaPlayer();
            player.Open(backgroundMusic);
            player.Play();
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
