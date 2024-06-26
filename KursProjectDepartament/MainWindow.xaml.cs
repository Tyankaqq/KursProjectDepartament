﻿using KursProjectDepartament.Model;
using KursProjectDepartament.View;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KursProjectDepartament
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Frame frame;
        public MainWindow()
        {
            InitializeComponent();
            frame = FrameContainer;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameContainer.Navigate(new HumanPage());
            
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            FrameContainer.Navigate(new AddEditHuman(new Employee()));
        }

        private void OpenAddEditHumanButton_Click(object sender, RoutedEventArgs e)
        {
            AddEditHuman addEditPage = new AddEditHuman(new Employee());
            FrameContainer.Navigate(addEditPage);
        }

    }
}