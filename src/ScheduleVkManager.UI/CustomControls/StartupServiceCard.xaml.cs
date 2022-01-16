using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScheduleVkManager.UI.CustomControls
{
    public partial class StartupServiceCard : UserControl
    {
        public static readonly DependencyProperty TextDependencyProperty = DependencyProperty.Register(
                                                                                "Text", typeof(string),
                                                                                typeof(MainWindow));

        public string Text
        {
            get => (string)GetValue(TextDependencyProperty);
            set => SetValue(TextDependencyProperty, value);
        }

        public StartupServiceCard()
        {
            InitializeComponent();
        }
    }
}
