using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Farba.View;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Farba
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.ExtendsContentIntoTitleBar = true;
            SetTitleBar(TitleBar);
            AppWindow.Resize(new Windows.Graphics.SizeInt32(400, 600));

            //rootFrame.Navigate(typeof(ColorSelectorPage));
            NavView.SelectionChanged += OnNavigationChanged;
        }

        private void OnNavigationChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs e)
        {
            if (e.SelectedItem is NavigationViewItem item)
            {
                switch (item.Tag)
                {
                    case "ColorSelectorPage":
                        rootFrame.Navigate(typeof(ColorSelectorPage));
                        break;
                    case "PaletteGeneratorPage":
                        rootFrame.Navigate(typeof(PaletteGeneratorPage));
                        break;
                    case "ContrastCheckerPage":
                        rootFrame.Navigate(typeof(ContrastCheckerPage));
                        break;
                }
            }
        }
    }
}
