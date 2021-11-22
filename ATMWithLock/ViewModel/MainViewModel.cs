using ATMWithLock.Command;
using ATMWithLock.Model;
using ATMWithLock.Repository;
using ATMWithLock.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ATMWithLock.ViewModel
{
    public class MainViewModel : BaseViewModel
    {

        public RelayCommand InsertBtnCommand { get; set; }
        public RelayCommand LoadBtnCommand { get; set; }
        public RelayCommand TransferMoneyBtnCommand { get; set; }
        public FakeRepo _repo { get; set; }
        public List<Card> Cards { get; set; }
        public Card Card { get; set; }
        public MainWindow MainWindow { get; set; }

        public static object obj = new object();
        DispatcherTimer dispatcher = new DispatcherTimer();
        public MainViewModel(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            dispatcher.Interval = TimeSpan.FromSeconds(2);
            dispatcher.Tick += Dispatcher_Tick;
          
            Cards = new List<Card>();
            _repo = new FakeRepo();
            Cards = _repo.GetAll();
            InsertBtnCommand = new RelayCommand((sender) =>
            {
                mainWindow.cardNumberTxtbx.Visibility = Visibility.Visible;
                mainWindow.cardNumberLbl.Visibility = Visibility.Visible;
                mainWindow.loadBtn.Visibility = Visibility.Visible;
            });
            LoadBtnCommand = new RelayCommand((sender) =>
            {
                foreach (var item in Cards)
                {
                    if (item.CardNumber == mainWindow.cardNumberTxtbx.Text)
                    {
                        mainWindow.fullNameLbl.Content = item.Username;
                        mainWindow.balanceLbl.Content = item.Balance;
                        Card = item;
                    }
                }
            });
            TransferMoneyBtnCommand = new RelayCommand((sender) =>
            {
                lock (obj)
                {
                    dispatcher.Start();
                    if (decimal.Parse(Card.Balance.ToString()) >= decimal.Parse(mainWindow.moneyCountTxtbx.Text))
                    {
                        Thread.Sleep(5000);
                        Card.Balance = (decimal.Parse(Card.Balance.ToString()) - decimal.Parse(mainWindow.moneyCountTxtbx.Text)).ToString();
                        mainWindow.balanceLbl.Content = Card.Balance;
                    }
                    else
                    {
                        MessageBox.Show("Transfer Declined");
                    }
                }
            });

        }

        private void Dispatcher_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Decimal.Parse(MainWindow.moneyCountTxtbx.Text) >= 0)
                {
                    MainWindow.moneyCountTxtbx.Text = (Decimal.Parse(MainWindow.moneyCountTxtbx.Text) - 10).ToString() ;
                    MainWindow.moneyAnimationLbl.Content = MainWindow.moneyCountTxtbx.Text;
                }
                else
                {
                    dispatcher.Stop();
                }

            }
            catch (Exception)
            {

            }
        }
    }
}
