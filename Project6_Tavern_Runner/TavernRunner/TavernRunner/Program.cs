using System;
using System.Collections;
using System.Text;
using System.Threading;
using GHIElectronics.TinyCLR.Devices.Display;
using System.Drawing;
using GHIElectronics.TinyCLR.Devices.Gpio;

namespace TavernRunner
{
    class Program
    {

        static Graphics screen;
        static GpioPin led = GpioController.GetDefault().OpenPin(
        GHIElectronics.TinyCLR.Pins.G400D.GpioPin.PC18); 


        static bool ledValue = false;
        static int locationX = 430;
        static int locY = 0;

        //
        static int playSpace = 110;
        static int blockwidth = 80;

        static int randomHeight1 = 60;
        static int randomHeight2 = 80;
        static int randomHeight3 = 100;
        static int randomHeight4 = 120;
        static int randomHeight5 = 90;
        static int randomHeight6 = 75;

        static int randomHeight7 = 45;
        static int randomHeight8 = 20;
        static int randomHeight9 = 50;
        static int randomHeight10 = 80;
        static int randomHeight11 = 110;
        static int randomHeight12 = 125;

        static int startBlock1UpX = 0;
        static int startBlock1UpY = 0;
        static int startBlock1DownX = startBlock1UpX;
        static int startBlock1DownY = startBlock1UpY + randomHeight1 + playSpace;

        static int startBlock2UpX = startBlock1UpX + blockwidth;
        static int startBlock2UpY = 0;
        static int startBlock2DownX = startBlock2UpX;
        static int startBlock2DownY = startBlock2UpY + randomHeight2 + playSpace;

        static int startBlock3UpX = startBlock2UpX + blockwidth;
        static int startBlock3UpY = 0;
        static int startBlock3DownX = startBlock3UpX;
        static int startBlock3DownY = startBlock3UpY + randomHeight3 + playSpace;

        static int startBlock4UpX = startBlock3UpX + blockwidth;
        static int startBlock4UpY = 0;
        static int startBlock4DownX = startBlock4UpX;
        static int startBlock4DownY = startBlock4UpY + randomHeight4 + playSpace;

        static int startBlock5UpX = startBlock4UpX + blockwidth;
        static int startBlock5UpY = 0;
        static int startBlock5DownX = startBlock5UpX;
        static int startBlock5DownY = startBlock5UpY + randomHeight5 + playSpace;

        static int startBlock6UpX = startBlock5UpX + blockwidth;
        static int startBlock6UpY = 0;
        static int startBlock6DownX = startBlock6UpX;
        static int startBlock6DownY = startBlock6UpY + randomHeight6 + playSpace;

        static int score = 0;
        static int highscore = 0;
        static int highscore2 = 0;
        static int highscore3 = 0;
        static int highscore4 = 0;
        static int win = 0;

        static void Main()
        {
            var displayController = DisplayController.GetDefault();
            var font = Resource1.GetFont(Resource1.FontResources.Ariel72);

            // Enter the proper display configurationsok
            displayController.SetConfiguration(new ParallelDisplayControllerSettings
            {
                Width = 480,
                Height = 272,
                DataFormat = DisplayDataFormat.Rgb565,
                PixelClockRate = 20000000,
                PixelPolarity = false,
                DataEnablePolarity = true,
                DataEnableIsFixed = false,
                HorizontalFrontPorch = 2,
                HorizontalBackPorch = 2,
                HorizontalSyncPulseWidth = 41,
                HorizontalSyncPolarity = false,
                VerticalFrontPorch = 2,
                VerticalBackPorch = 2,
                VerticalSyncPulseWidth = 10,
                VerticalSyncPolarity = false,
            });

            displayController.Enable();

            screen = Graphics.FromHdc(displayController.Hdc);
            var GreenPen = new Pen(Color.Green);
            // Start Drawing (to memroy)
            screen.Clear(Color.Black);         

            //screen.DrawEllipse(GreenPen, 10, 10, 20, 10);
            screen.DrawString("Kunal", font, new SolidBrush(Color.Yellow), 20, 20);
            screen.DrawString("Mukherjee", font, new SolidBrush(Color.Yellow), 20, 100);
            screen.DrawString("EE 356", font, new SolidBrush(Color.Blue), 20, 180);

            // Flush the memory to the display. This is a very fast operation.
            screen.Flush();
            GpioPin padUp = GpioController.GetDefault().OpenPin(
            GHIElectronics.TinyCLR.Pins.G400D.GpioPin.PA24);
            GpioPin padDown = GpioController.GetDefault().OpenPin(
            GHIElectronics.TinyCLR.Pins.G400D.GpioPin.PA4);
            GpioPin padRight = GpioController.GetDefault().OpenPin(
            GHIElectronics.TinyCLR.Pins.G400D.GpioPin.PD9);
            GpioPin padLeft = GpioController.GetDefault().OpenPin(
            GHIElectronics.TinyCLR.Pins.G400D.GpioPin.PD7);

            int locationY = 272 / 2;       

            while (padUp.Read() != 0)
            {
                Thread.Sleep(100);
            }

            while (true)
            {
                if(win == 0)
                {
                    if (padUp.Read() == 0)
                    {
                        if (locationY > (0 + 5))
                            locationY -= 2;
                    }
                    else if (padDown.Read() == 0)
                    {                        
                        if (locationY < (272 - 20))
                            locationY += 2;
                    }

                    Thread.Sleep(33); //33
                    UpdateScreen(locationY);
                    score++;
               }
               else
               {
                    showScore();                    
                    score = 1;
                    win = 0;
                    locationY = 272 / 2;
                    initialRandom();                   
                    updateCavern();

                    while (padLeft.Read() != 0)
                    {
                        Thread.Sleep(100);
                    }
                    
                    UpdateScreen(locationY);
                }
                

            }
        }


        static void UpdateScreen(int locationY)
        {
            var blueBrush = new SolidBrush(Color.Blue);            
            var greenBrush = new SolidBrush(Color.Green);
            var font = Resource1.GetFont(Resource1.FontResources.Ariel32);

            var redBrush = new SolidBrush(Color.Red);
            var purpleBrush = new SolidBrush(Color.Purple);
            var tealBrush = new SolidBrush(Color.Teal);

            screen.Clear(Color.Black);

            //  
            updateCavern();

            screen.FillRectangle(redBrush, startBlock1UpX, startBlock1UpY, blockwidth, randomHeight1);
            screen.FillRectangle(redBrush, startBlock1DownX, startBlock1DownY, blockwidth, 272 - playSpace - randomHeight1);

            screen.FillRectangle(purpleBrush, startBlock2UpX, startBlock2UpY, blockwidth, randomHeight2);
            screen.FillRectangle(purpleBrush, startBlock2DownX, startBlock2DownY, blockwidth, 272 - playSpace - randomHeight2);

            screen.FillRectangle(tealBrush, startBlock3UpX, startBlock3UpY, blockwidth, randomHeight3);
            screen.FillRectangle(tealBrush, startBlock3DownX, startBlock3DownY, blockwidth, 272 - playSpace - randomHeight3);

            screen.FillRectangle(redBrush, startBlock4UpX, startBlock4UpY, blockwidth, randomHeight4);
            screen.FillRectangle(redBrush, startBlock4DownX, startBlock4DownY, blockwidth, 272 - playSpace - randomHeight4);

            screen.FillRectangle(purpleBrush, startBlock5UpX, startBlock5UpY, blockwidth, randomHeight5);
            screen.FillRectangle(purpleBrush, startBlock5DownX, startBlock5DownY, blockwidth, 272 - playSpace - randomHeight5);

            screen.FillRectangle(tealBrush, startBlock6UpX, startBlock6UpY, blockwidth, randomHeight6);
            screen.FillRectangle(tealBrush, startBlock6DownX, startBlock6UpY + randomHeight6 + playSpace, blockwidth, 272 - playSpace - randomHeight6);

            //*


            screen.FillRectangle(redBrush, startBlock1UpX - 480, startBlock1UpY, blockwidth, randomHeight7);
            screen.FillRectangle(redBrush, startBlock1DownX - 480, startBlock1UpY + randomHeight7 + playSpace, blockwidth, 272 - playSpace - randomHeight7 );

            screen.FillRectangle(purpleBrush, startBlock2UpX - 480, startBlock2UpY, blockwidth, randomHeight8);
            screen.FillRectangle(purpleBrush, startBlock2DownX - 480, startBlock2UpY + randomHeight8 + playSpace, blockwidth, 272 - playSpace - randomHeight8);

            screen.FillRectangle(tealBrush, startBlock3UpX - 480, startBlock3UpY, blockwidth, randomHeight9);
            screen.FillRectangle(tealBrush, startBlock3DownX - 480, startBlock3UpY + randomHeight9 + playSpace, blockwidth, 272 - playSpace - randomHeight9);

            screen.FillRectangle(redBrush, startBlock4UpX - 480, startBlock4UpY, blockwidth, randomHeight10);
            screen.FillRectangle(redBrush, startBlock4DownX - 480, startBlock4UpY + randomHeight10 + playSpace, blockwidth, 272 - playSpace - randomHeight10);

            screen.FillRectangle(purpleBrush, startBlock5UpX - 480, startBlock5UpY, blockwidth, randomHeight11);
            screen.FillRectangle(purpleBrush, startBlock5DownX - 480, startBlock5UpY + randomHeight11 + playSpace, blockwidth, 272 - playSpace - randomHeight11);

            screen.FillRectangle(tealBrush, startBlock6UpX - 480, startBlock6UpY, blockwidth, randomHeight12);
            screen.FillRectangle(tealBrush, startBlock6DownX - 480, startBlock6UpY + randomHeight12 + playSpace, blockwidth, 272 - playSpace - randomHeight12);

            //

            locY = locationY;
            //screen.FillRectangle(blueBrush, locationX, locationY, 15, 15);

            screen.FillRectangle(blueBrush, locationX, locationY, 25, 5);
            screen.FillRectangle(blueBrush, locationX + 5, locationY - 5, 5, 15);
            screen.FillRectangle(blueBrush, locationX + 15, locationY - 2, 5, 9);

            screen.FillRectangle(greenBrush, 200, 200, 150, 32);
            screen.DrawString("Score " + score.ToString(), font, new SolidBrush(Color.Yellow), 200, 200);
            screen.Flush();
            isWin();

            if (!ledValue)
            {
                led.Write(GpioPinValue.High);
                ledValue = true;
            }
            else
            {
                led.Write(GpioPinValue.Low);
                ledValue = false;
            }

        }
        
        static void updateCavern()
        {

            if (startBlock1UpX >  480)
            {
                startBlock1UpX = 0;
                updateRandomHeight();
            }
                

            startBlock1UpX += 5;

            if (score > 50)
                startBlock1UpX += 6;

            startBlock1UpY = 0;
            startBlock1DownX = startBlock1UpX;
            startBlock1DownY = startBlock1UpY + randomHeight1 + playSpace;

            startBlock2UpX = startBlock1UpX + blockwidth;
            startBlock2UpY = 0;
            startBlock2DownX = startBlock2UpX;
            startBlock2DownY = startBlock2UpY + randomHeight2 + playSpace;

            startBlock3UpX = startBlock2UpX + blockwidth;
            startBlock3UpY = 0;
            startBlock3DownX = startBlock3UpX;
            startBlock3DownY = startBlock3UpY + randomHeight3 + playSpace;

            startBlock4UpX = startBlock3UpX + blockwidth;
            startBlock4UpY = 0;
            startBlock4DownX = startBlock4UpX;
            startBlock4DownY = startBlock4UpY + randomHeight4 + playSpace;

            startBlock5UpX = startBlock4UpX + blockwidth;
            startBlock5UpY = 0;
            startBlock5DownX = startBlock5UpX;
            startBlock5DownY = startBlock5UpY + randomHeight5 + playSpace;

            startBlock6UpX = startBlock5UpX + blockwidth;
            startBlock6UpY = 0;
            startBlock6DownX = startBlock6UpX;
            startBlock6DownY = startBlock6UpY + randomHeight6 + playSpace;

            if (score % 25 == 0 && (score != 0) && (playSpace > 30))
             {
                 playSpace -= 5;
             }
            else
            {
                playSpace = 110;
            }
            

        }

        static void updateRandomHeight()
        {
            randomHeight1 = randomHeight7;
            randomHeight2 = randomHeight8;
            randomHeight3 = randomHeight9;
            randomHeight4 = randomHeight10;
            randomHeight5 = randomHeight11;
            randomHeight6 = randomHeight12;

            if(score % 2 == 0)
            {
                randomHeight7 = 30;
                randomHeight8 = 60;
                randomHeight9 = 80;
                randomHeight10 = 100;
                randomHeight11 = 70;
                randomHeight12 = 65;
            }
            if (score % 5 == 0)
            {
                randomHeight7 = 75;
                randomHeight8 = 115;
                randomHeight9 = 145;
                randomHeight10 = 150;
                randomHeight11 = 120;
                randomHeight12 = 80;
            }

        }

        static void initialRandom()
        {
            randomHeight1 = 60;
            randomHeight2 = 80;
            randomHeight3 = 100;
            randomHeight4 = 120;
            randomHeight5 = 90;
            randomHeight6 = 75;
        }

        static void isWin()
        {
            if ((startBlock1UpX + blockwidth > locationX) && (startBlock1UpX < locationX))
            {
                if ((locY - 5 < startBlock1UpY + randomHeight1) || (locY + 10 > startBlock1DownY))
                {
                    win = 1;
                }
            }
            else if ((startBlock2UpX + blockwidth > locationX) && (startBlock2UpX < locationX))
            {
                if ((locY - 5 < startBlock2UpY + randomHeight2) || (locY + 10 > startBlock2DownY))
                {
                    win = 1;
                }
            }
            else if ((startBlock3UpX + blockwidth > locationX) && (startBlock3UpX < locationX))
            {
                if ((locY - 5 < startBlock3UpY + randomHeight3) || (locY + 10 > startBlock3DownY))
                {
                    win = 1;
                }
            }
            else if ((startBlock4UpX + blockwidth > locationX) && (startBlock4UpX < locationX))
            {
                if ((locY - 5 < startBlock4UpY + randomHeight4) || (locY + 10 > startBlock4DownY))
                {
                    win = 1;
                }
            }
            else if ((startBlock5UpX + blockwidth > locationX) && (startBlock5UpX < locationX))
            {
                if ((locY - 5 < startBlock5UpY + randomHeight5) || (locY + 10 > startBlock5DownY))
                {
                    win = 1;
                }
            }
            else if ((startBlock6UpX + blockwidth > locationX) && (startBlock6UpX < locationX))
            {
                if ((locY - 5 < startBlock6UpY + randomHeight6) || (locY + 10 > startBlock6DownY))
                {
                    win = 1;
                }
            }
            else
            {
                win = 0;
            }
        }

        static void showScore()
        {
            var blueBrush = new SolidBrush(Color.Blue);
            var redBrush = new SolidBrush(Color.Red);
            var greenBrush = new SolidBrush(Color.Green);
            var font = Resource1.GetFont(Resource1.FontResources.Ariel32);

            screen.Clear(Color.Black);
            //
            screen.FillRectangle(blueBrush, 20, 20, 400, 230);
            screen.FillRectangle(redBrush, 30, 30, 380, 200);

            if (score >= highscore)
            {
                screen.DrawString("Score: " + score.ToString(), font, new SolidBrush(Color.Yellow), 35, 35);
                screen.DrawString("Congratulations. You have the", font, new SolidBrush(Color.Yellow), 35, 35 + 38);
                screen.DrawString("Highest Score: " + score.ToString(), font, new SolidBrush(Color.Yellow), 35, 35 + 38 + 38);
                highscore = score;
            }
            else
            {
                screen.DrawString("Your Score: " + score.ToString(), font, new SolidBrush(Color.Yellow), 35, 35);
                screen.DrawString("Highest Score: " + highscore.ToString(), font, new SolidBrush(Color.Yellow), 35, 35 + 38);
                screen.DrawString("2nd Highest Score: " + highscore2.ToString(), font, new SolidBrush(Color.Yellow), 35, 35 + 38 + 38);
                screen.DrawString("3rd Highest Score: " + highscore3.ToString(), font, new SolidBrush(Color.Yellow), 35, 35 + 38 + 38 + 38);
                screen.DrawString("4th Highest Score: " + highscore4.ToString(), font, new SolidBrush(Color.Yellow), 35, 35 + 38 + 38 + 38 + 38);

                if (score > highscore2)
                {
                    highscore4 = highscore3;
                    highscore3 = highscore2;
                    highscore2 = score;
                }
                else
                {
                    if (score > highscore3)
                    {
                        highscore4 = highscore3;
                        highscore3 = score;
                    }
                    else
                    {
                        if (score > highscore4)
                        {
                            highscore4 = score;
                        }
                    }
                }
            }

                screen.Flush();

                if (!ledValue)
                {
                    led.Write(GpioPinValue.High);
                    ledValue = true;
                }
                else
                {
                    led.Write(GpioPinValue.Low);
                    ledValue = false;
                }

            }
        }

    
}
