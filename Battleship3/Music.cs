using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
namespace Battleship3
{
   public static class Music
    {
        private static MediaPlayer playMusic = new MediaPlayer();
        private static MediaPlayer playLose = new MediaPlayer();
        private static MediaPlayer playWin = new MediaPlayer();


        public static void PlayBackgroundMusic()
        {
            playMusic.Open(new Uri(Path.Combine(Environment.CurrentDirectory, "backgroundMusic.mp3")));
            playMusic.Play();
        }

       
        public static void PlayLose()
        {
            playLose.Open(new Uri(Path.Combine(Environment.CurrentDirectory, "Sad_Violin_-_MLG_Sound_Effects_HD[Mp3Converter.net]")));
            playLose.Play();
        }

        public static void PlayWin()
        {
            playWin.Open(new Uri(Path.Combine(Environment.CurrentDirectory,"Heavenly_Music_Vanoss_Sound_Effect[Mp3Converter.net]")));
            playWin.Play();
        }
    }
}
