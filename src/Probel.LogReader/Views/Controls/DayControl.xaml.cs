using System;
using System.Windows;
using System.Windows.Controls;

namespace Probel.LogReader.Views.Controls
{
    /// <summary>
    /// Interaction logic for DayControl.xaml
    /// </summary>
    public partial class DayControl : UserControl
    {
        #region Fields

        public static readonly DependencyProperty RepositoryProperty = DependencyProperty.Register(
            "Repository",
            typeof(string),
            typeof(DayControl),
            new PropertyMetadata(null, OnRepositoryChanged));

        private static void OnRepositoryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DayControl tb)
            {
                tb._repository.Text = e.NewValue as string;
            }
        }

        public static readonly DependencyProperty DayProperty = DependencyProperty.Register(
            "Day",
            typeof(string),
            typeof(DayControl),
            new PropertyMetadata(null, OnDayChanged));

        private static void OnDayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DayControl tb)
            {
                tb._date.Text = e.NewValue as string;
            }
        }

        #endregion Fields

        #region Constructors

        public DayControl()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string Repository
        {
            get => (string)GetValue(RepositoryProperty);
            set => SetValue(RepositoryProperty, value);
        }
        public string Day
        {
            get => (string)GetValue(DayProperty);
            set => SetValue(DayProperty, value);
        }

        #endregion Properties
    }
}