using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.WebUI;

namespace Farba.View
{
    public sealed partial class ContrastCheckerPage : Page
    {
        static Random random = new Random();

        private readonly Brush passBrush = new SolidColorBrush(Color.FromArgb(255, 0, 200, 0));
        private readonly Brush failBrush = new SolidColorBrush(Color.FromArgb(255, 200, 0, 0));

        public ContrastCheckerPage()
        {
            this.InitializeComponent();

            ForegroundHexInput.Text = string.Format("#{0:X2}{1:X2}{2:X2}", random.Next(256), random.Next(256), random.Next(256));
            BackgroundHexInput.Text = string.Format("#{0:X2}{1:X2}{2:X2}", random.Next(256), random.Next(256), random.Next(256));

            UpdateContrast();
        }

        private void ColorInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateContrast();
        }

        private void UpdateContrast()
        {
            if (TryParseColor(ForegroundHexInput.Text, out Color foregroundColor) &&
                TryParseColor(BackgroundHexInput.Text, out Color backgroundColor))
            {
                PreviewBackground.Background = new SolidColorBrush(backgroundColor);
                foreach (var previewText in PreviewContentBox.Children.OfType<TextBlock>())
                { 
                    previewText.Foreground = new SolidColorBrush(foregroundColor);
                }

                double foregroundLuminane = RelativeLuminance(foregroundColor);
                double backgroundLuminance = RelativeLuminance(backgroundColor);

                double ratio = foregroundLuminane > backgroundLuminance
                    ? ((foregroundLuminane + 0.05) / (backgroundLuminance + 0.05))
                    : ((backgroundLuminance + 0.05) / (foregroundLuminane + 0.05));

                ContrastRationOutput.Text = ratio.ToString("N2");

                WcagAaLargeStatus.Text = ratio >= 3.0 ? "PASS" : "FAIL";
                WcagAaSmallStatus.Text = ratio >= 4.5 ? "PASS" : "FAIL";
                WcagAaaLargeStatus.Text = ratio >= 4.5 ? "PASS" : "FAIL";
                WcagAaaSmallStatus.Text = ratio >= 7.0 ? "PASS" : "FAIL";

                WcagAaLargeStatus.Foreground = ratio >= 3.0 ? passBrush : failBrush;
                WcagAaSmallStatus.Foreground = ratio >= 4.5 ? passBrush : failBrush;
                WcagAaaLargeStatus.Foreground = ratio >= 4.5 ? passBrush : failBrush;
                WcagAaaSmallStatus.Foreground = ratio >= 7.0 ? passBrush : failBrush;
            }
        }

        private static bool TryParseColor(string hex, out Color color)
        {
            color = Colors.Transparent;
            if (hex.StartsWith('#'))
            {
                hex = hex[1..];
            }

            if (hex.Length == 6 && int.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out int intColor))
            {
                color = Color.FromArgb(255,
                    (byte)((intColor >> 16) & 0xFF),
                    (byte)((intColor >> 8) & 0xFF),
                    (byte)(intColor & 0xFF));
                return true;
            }
            return false;
        }

        private static double RelativeLuminance(Color color)
        {
            static double Adjust(double v)
            {
                v /= 255.0;
                return v <= 0.03928 
                    ? v / 12.92 
                    : Math.Pow((v + 0.055) / 1.055, 2.4);
            }

            double r = Adjust(color.R);
            double g = Adjust(color.G);
            double b = Adjust(color.B);

            return r * 0.2126 + g * 0.7152 + b * 0.0722;
        }
    }
}
