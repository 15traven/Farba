using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Media;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI;

namespace Farba.View
{
    public sealed partial class ColorSelectorPage : Page
    {
        public ColorSelectorPage()
        {
            this.InitializeComponent();

            byte r = (byte)Random.Shared.Next(0, 255);
            byte g = (byte)Random.Shared.Next(0, 255);
            byte b = (byte)Random.Shared.Next(0, 255);

            UpdateUI(r, g, b);
            UpdateColorPreview(r, g, b);
        }

        private void UpdateColorPreview(byte r, byte g, byte b)
        {
            ColorPreview.Background = new SolidColorBrush(Color.FromArgb(255, r, g, b));
            HexInput.Text = $"#{r:X2}{g:X2}{b:X2}";
        }

        private void CopyHex(object sender, RoutedEventArgs e)
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(HexInput.Text);
            Clipboard.SetContent(dataPackage);
        }

        private void HexInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (HexInput.Text.Length == 7 && HexInput.Text.StartsWith("#"))
            {
                if (byte.TryParse(HexInput.Text.AsSpan(1, 2), System.Globalization.NumberStyles.HexNumber, null, out byte r) &&
                    byte.TryParse(HexInput.Text.AsSpan(3, 2), System.Globalization.NumberStyles.HexNumber, null, out byte g) &&
                    byte.TryParse(HexInput.Text.AsSpan(5, 2), System.Globalization.NumberStyles.HexNumber, null, out byte b))
                {
                    UpdateUI(r, g, b);
                    UpdateColorPreview(r, g, b);
                }
            }
        }

        private void UpdateUI(byte r, byte g, byte b)
        {
            RedSlider.Value = r;
            GreenSlider.Value = g;
            BlueSlider.Value = b;

            RedInput.Text = r.ToString();
            GreenInput.Text = g.ToString();
            BlueInput.Text = b.ToString();
        }

        private void RgbSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            byte r = (byte)RedSlider.Value;
            byte g = (byte)GreenSlider.Value;
            byte b = (byte)BlueSlider.Value;

            UpdateUI(r, g, b);
            UpdateColorPreview(r, g, b);
        }

        private void RgbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (byte.TryParse(RedInput.Text, out byte r) &&
                byte.TryParse(GreenInput.Text, out byte g) &&
                byte.TryParse(BlueInput.Text, out byte b))
            {
                UpdateUI(r, g, b);
                UpdateColorPreview(r, g, b);
            }
        }
    }
}
