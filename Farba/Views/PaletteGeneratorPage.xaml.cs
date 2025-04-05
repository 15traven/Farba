using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

namespace Farba.View
{
    public sealed partial class PaletteGeneratorPage : Page
    {
        private readonly bool[] isLocked = new bool[5];

        public PaletteGeneratorPage()
        {
            this.InitializeComponent();
            GenerateRandomColors();
        }

        private void Page_KeyDown(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            GenerateRandomColors();
        }

        private void GenerateRandomColors()
        {
            if (!isLocked[0]) SetRandomColor(ColorRow1, 1);
            if (!isLocked[1]) SetRandomColor(ColorRow2, 2);
            if (!isLocked[2]) SetRandomColor(ColorRow3, 3);
            if (!isLocked[3]) SetRandomColor(ColorRow4, 4);
            if (!isLocked[4]) SetRandomColor(ColorRow5, 5);
        }

        private void SetRandomColor(Border row, int index)
        {
            var hexText = FindName($"HexText{index}") as TextBlock;
            var lockIcon = FindName($"LockIcon{index}") as FontIcon;

            byte r = (byte)Random.Shared.Next(256);
            byte g = (byte)Random.Shared.Next(256);
            byte b = (byte)Random.Shared.Next(256);

            row.Background = new SolidColorBrush(Color.FromArgb(255, r, g, b));

            string hex = $"#{r:X2}{g:X2}{b:X2}";
            hexText!.Text = hex;

            double brightness = System.Drawing.Color.FromArgb(255, r, g, b).GetBrightness();
            if (brightness > 0.5)
            {
                hexText.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                lockIcon!.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            } 
            else
            {
                hexText.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                lockIcon!.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }
        }

        private void ToggleLock(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;

            int index = int.Parse(button.Tag.ToString());
            isLocked[index] = !isLocked[index];

            FontIcon icon = (FontIcon)button.Content;
            icon.Glyph = isLocked[index] ? "\uE72E" : "\uE785";
        }

        private void CopyHex(object sender, RoutedEventArgs e)
        {
            if (sender is TextBlock hexTexBlock)
            {
                DataPackage dataPackage = new();
                dataPackage.SetText(hexTexBlock.Text);

                Clipboard.SetContent(dataPackage);
            }
        }
    }
}
