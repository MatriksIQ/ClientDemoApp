using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientApiAppDemo.Annotations;
using ClientApiAppDemo.Models;
using Matriks.Api;
using Matriks.Api.RequestModels;
using Matriks.Api.ResposeModels;
using Matriks.API.Shared;
using Matriks.ApiClient;
using Matriks.ApiClient.TcpConnection;
using Matriks.Utility;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace ClientApiAppDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public ObservableCollection<Accounts> Accounts
        {
            get => _accounts;
            set
            {
                _accounts = value;
                OnPropertyChanged("Accounts");
            } 
        }

        public Accounts SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                if (SelectedAccount != null)
                {
                    FilterPositions();
                    FilterOrders();
                }
                    
                OnPropertyChanged();
            } 
        }

        

        public ObservableCollection<PositionResponseModel> FilteredPositions
        {
            get => _filteredPositions;
            set
            {
                _filteredPositions = value;

                OnPropertyChanged();
            } 
        }

        public ObservableCollection<OrderRequest> FileteredOrders
        {
            get => _fileteredOrders;
            set
            {
                _fileteredOrders = value;
                OnPropertyChanged();
            } 
        }

        public OrderRequest SelectedOrderApiModel
        {
            get => _orderApiModel;
            set
            {
                _orderApiModel = value;
                if (value != null)
                    this.Symbol = value.Symbol;
                OnPropertyChanged();
            } 
        }

        private void FilterPositions()
        {
            if (SelectedAccount == null)
                return;
            FilteredPositions =new ObservableCollection<PositionResponseModel>(AllPositionResponseModels
                    .Where(x => x.AccountId == SelectedAccount.AccountId && x.BrokageId == SelectedAccount.BrokageId && x.ExchangeId == SelectedAccount.ExchangeId));
                
        }

        private void FilterOrders()
        {
            if (SelectedAccount == null)
                return;
            FileteredOrders = new ObservableCollection<OrderRequest>(AllOrderApiModels.Where(x => x.AccountId == SelectedAccount.AccountId ));
                
        }
        public List<PositionResponseModel> AllPositionResponseModels;

        public List<OrderRequest> AllOrderApiModels;

        public string Symbol
        {
            get => _symbol;
            set
            {
                _symbol = value;
                OnPropertyChanged();
            } 
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            } 
        }

        public decimal Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                OnPropertyChanged();
            } 
        }

        private ICommand OnRefreshAccountInfoCommand { get; set; }

        private TcpClientService _tcpClientService;
        private TcpCallbackService _tcpCallbackService;
        private ObservableCollection<Accounts> _accounts;
        private Accounts _selectedAccount;
        private ObservableCollection<PositionResponseModel> _filteredPositions;
        private ObservableCollection<OrderRequest> _fileteredOrders;
        private string _symbol;
        private decimal _price;
        private decimal _volume;
        private OrderRequest _orderApiModel;

        private Timer _keepAliveTimer;
        public MainWindow()
        {
            _tcpCallbackService = new TcpCallbackService();
            _tcpClientService = new TcpClientService(_tcpCallbackService, "localhost", 18890);
            InitializeComponent();
            DataContext = this;
            this.Accounts = new ObservableCollection<Accounts>();
            AllPositionResponseModels = new List<PositionResponseModel>();
            AllOrderApiModels = new List<OrderRequest>();
            FileteredOrders = new ObservableCollection<OrderRequest>();
            OnRefreshAccountInfoCommand = new RoutedCommand();
            _tcpClientService.InitializeTcpConnection();
            RegisterEvents();
            FilteredPositions = new ObservableCollection<PositionResponseModel>();
        }

        private void RegisterEvents()
        {
            _tcpCallbackService.ListAccountsResponseEvent += TcpCallbackServiceOnListAccountsResponseEvent;
            _tcpCallbackService.ListPositionsResponseEvent += TcpCallbackServiceOnListPositionsResponseEvent;
            _tcpCallbackService.ListOrdersResponseEvent += TcpCallbackServiceOnListOrdersResponseEvent;
            _tcpCallbackService.OrderChangedEvent += TcpCallbackServiceOnOrderChangedEvent;
            _tcpCallbackService.PositionChangedEvent += TcpCallbackServiceOnPositionChangedEvent;
            _tcpCallbackService.TradeUserLoginEvent += TcpCallbackServiceOnTradeUserLoginEvent;
            _tcpCallbackService.TraderUserLogoutEvent += TcpCallbackServiceOnTraderUserLogoutEvent;
            _tcpCallbackService.KeepAliveResponseEvent += TcpCallbackServiceOnKeepAliveResponseEvent;
            _keepAliveTimer = new Timer();
            _keepAliveTimer.Interval = 1000 * 30;
            _keepAliveTimer.Elapsed += KeepAliveTimerOnElapsed;
            _keepAliveTimer.Start();
        }

        private void TcpCallbackServiceOnKeepAliveResponseEvent(object sender, KeepAlive e)
        {
        }

        private void KeepAliveTimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            _tcpClientService.SendKeepAlive();
        }

        private void TcpCallbackServiceOnTraderUserLogoutEvent(object sender, TradeUserLogoutModel e)
        {
            if (!Accounts.Any(x => x.AccountId == e.AccountId && x.BrokageId == e.BrokageId))
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.Accounts.Remove(Accounts.FirstOrDefault(x => x.AccountId == e.AccountId && x.BrokageId == e.BrokageId));
                    this.AllPositionResponseModels.RemoveAll(x =>
                        x.BrokageId == e.BrokageId && x.AccountId == e.AccountId);
                    this.AllOrderApiModels.RemoveAll(x => x.AccountId == e.AccountId && x.BrokageId == e.BrokageId);
                    if (SelectedAccount.AccountId == e.AccountId && SelectedAccount.BrokageId == e.BrokageId)
                        SelectedAccount = null;

                    FilterOrders();
                    FilterPositions();

                });
        }

        private void TcpCallbackServiceOnTradeUserLoginEvent(object sender, TradeUserLoginModel e)
        {
            _tcpClientService.RequestAccounts();
            //    var account = new Accounts();
            //    account.BrokageId = e.BrokageId;
            //    account.AccountId = e.AccountId;
            //    account.ExchangeId = e.ExchangeId;
            //    account.DisplayName = e.BrokageName + " " + e.AccountId;
            //    if(!Accounts.Any(x => x.AccountId == account.AccountId && x.BrokageId == account.BrokageId))
            //        Application.Current.Dispatcher.Invoke(() => { this.Accounts.Add(account); });
            //    _tcpClientService.RequestPositions(account.BrokageId, account.AccountId, account.ExchangeId);
            //_tcpClientService.RequestWaitingOrders(account.AccountId, account.BrokageId,account.ExchangeId);

        }

        private void TcpCallbackServiceOnPositionChangedEvent(object sender, PositionResponseModel e)
        {
            if (!AllPositionResponseModels.Any(x =>
                x.Symbol == e.Symbol && x.AccountId == e.AccountId && x.BrokageId == e.BrokageId && e.ExchangeId== x.ExchangeId))
            {
                AllPositionResponseModels.Add(e);
            }
            else
            {
                AllPositionResponseModels.Remove(AllPositionResponseModels.FirstOrDefault(x =>
                    x.Symbol == e.Symbol && x.AccountId == e.AccountId && x.BrokageId == e.BrokageId && x.ExchangeId == e.ExchangeId));
                AllPositionResponseModels.Add(e);
            }
            FilterPositions();  
        }

        private void TcpCallbackServiceOnOrderChangedEvent(object sender, OrderRequest e)
        {
            if (!AllOrderApiModels.Any(x => x.OrderId == e.OrderId))
            {
                AllOrderApiModels.Add(e);
            }
            else
            {
                AllOrderApiModels.Remove(AllOrderApiModels.FirstOrDefault(x => x.OrderId == e.OrderId));
                AllOrderApiModels.Add(e);
            }

            FilterOrders();
        }

        private void TcpCallbackServiceOnListOrdersResponseEvent(object sender, ListOrdersApiResponseModel e)
        {
            foreach (var eOrderApiModel in e.OrderApiModels)
            {
                AllOrderApiModels.Add(eOrderApiModel);
            }

        }

        private void TcpCallbackServiceOnListPositionsResponseEvent(object sender, ListPositionResponseModel e)
        {
            AllPositionResponseModels.AddRange(e.PositionResponseList);
        }

        private void TcpCallbackServiceOnListAccountsResponseEvent(object sender, List<BrokageAccounts> e)
        {
            foreach (var brokerAccountse in e)
            {
                foreach (var accountId in brokerAccountse.AccountIdList)
                {
                    var account = new Accounts();
                    account.BrokageId = brokerAccountse.BrokageId;
                    account.AccountId = accountId.AccountId;
                    account.ExchangeId = accountId.ExchangeId;
                    account.DisplayName =  brokerAccountse.BrokageName + " " + accountId.AccountId + " " +account.ExchangeId;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if(!Accounts.Any(x => x.AccountId == account.AccountId && x.BrokageId == account.BrokageId && x.ExchangeId == account.ExchangeId))
                            this.Accounts.Add(account);
                    });
                    if (Accounts.Any(x =>
                        x.AccountId == account.AccountId && x.BrokageId == account.BrokageId &&
                        x.ExchangeId == account.ExchangeId))
                    {
                        _tcpClientService.RequestPositions(account.BrokageId, account.AccountId, account.ExchangeId);
                        _tcpClientService.RequestWaitingOrders(account.AccountId, account.BrokageId, account.ExchangeId);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            OrderRequest orderApiModel =  new OrderRequest();
            orderApiModel.Symbol = Symbol;
            orderApiModel.AccountId = SelectedAccount.AccountId;
            orderApiModel.BrokageId = SelectedAccount.BrokageId;
            orderApiModel.Price = Price;
            orderApiModel.Quantity = Volume;
            orderApiModel.OrderSide = 0;
            orderApiModel.OrderType = '2';
            orderApiModel.TransactionType = '1';
            orderApiModel.ApiCommands = (int) ApiCommands.NewOrder;
            _tcpClientService.SendNewOrder(orderApiModel);
            


        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OrderView orderView = new OrderView();
                orderView.Symbol = SelectedOrderApiModel.Symbol;
                orderView.SelectedAccount = SelectedOrderApiModel.AccountId;
                orderView.SelectedBrokage = SelectedOrderApiModel.BrokageId;
                orderView.IsEdit = true;
                orderView.Price = SelectedOrderApiModel.Price;
                orderView.Volume = SelectedOrderApiModel.LeavesQty;
                orderView.TcpClientService = _tcpClientService;
                if (SelectedOrderApiModel.OrderSide == 0)
                    orderView.IsBuy = true;
                else
                    orderView.IsBuy = false;

                orderView.OrderId = SelectedOrderApiModel.OrderId;
                orderView.OrderId2 = SelectedOrderApiModel.OrderId2;
                orderView.ClientOrderId = SelectedOrderApiModel.ClientOrderId;
                orderView.Show();
            }
            catch (Exception)
            {

            }

        }

        private void MenuItem_OnClickCancel(object sender, RoutedEventArgs e)
        {
            OrderRequest orderApiModel = new OrderRequest();
            orderApiModel.Symbol = Symbol;
            orderApiModel.AccountId = SelectedOrderApiModel.AccountId;
            orderApiModel.BrokageId = SelectedOrderApiModel.BrokageId;
            orderApiModel.Price = SelectedOrderApiModel.Price;
            orderApiModel.Quantity = SelectedOrderApiModel.OrderQty;
            orderApiModel.LeavesQty = SelectedOrderApiModel.LeavesQty;
            orderApiModel.OrderId2 = SelectedOrderApiModel.OrderId2;
            orderApiModel.OrderSide = SelectedOrderApiModel.OrderSide;
            orderApiModel.OrderId = SelectedOrderApiModel.OrderId;
            orderApiModel.OrderId2 = SelectedOrderApiModel.OrderId2;
            

            orderApiModel.OrderType = '2';
            orderApiModel.TimeInForce = SelectedOrderApiModel.TimeInForce;



            orderApiModel.TransactionType = '1';

            _tcpClientService.SendCancelOrder(orderApiModel);
        }

        private void MenuItem_OnClickAddOrder(object sender, RoutedEventArgs e)
        {
            try
            {
                OrderView orderView = new OrderView();

                orderView.SelectedAccount = SelectedAccount.AccountId;
                orderView.SelectedBrokage = SelectedAccount.BrokageId;
                orderView.IsEdit = false;
                if (SelectedOrderApiModel != null)
                {
                    orderView.Symbol = SelectedOrderApiModel.Symbol;
                    orderView.Price = SelectedOrderApiModel.Price;
                    orderView.Volume = SelectedOrderApiModel.LeavesQty;
                    orderView.OrderId = SelectedOrderApiModel.OrderId;
                    orderView.OrderId2 = SelectedOrderApiModel.OrderId2;
                    if (SelectedOrderApiModel.OrderSide == 0)
                        orderView.IsBuy = true;
                    else
                        orderView.IsBuy = false;

                }
                orderView.TcpClientService = _tcpClientService;


                orderView.Show();
            }
            catch (Exception)
            {

            }
        }

        private void DuzenleClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                OrderView orderView = new OrderView();
                orderView.Symbol = SelectedOrderApiModel.Symbol;
                orderView.SelectedAccount = SelectedOrderApiModel.AccountId;
                orderView.SelectedBrokage = SelectedOrderApiModel.BrokageId;
                orderView.IsEdit = true;
                orderView.Price = SelectedOrderApiModel.Price;
                orderView.Volume = SelectedOrderApiModel.LeavesQty;
                orderView.TcpClientService = _tcpClientService;
                if (SelectedOrderApiModel.OrderSide == 0)
                    orderView.IsBuy = true;
                else
                    orderView.IsBuy = false;

                orderView.OrderId = SelectedOrderApiModel.OrderId;
                orderView.OrderId2 = SelectedOrderApiModel.OrderId2;
                orderView.ClientOrderId = SelectedOrderApiModel.ClientOrderId;
                orderView.Show();
            }
            catch (Exception)
            {

            }
        }

        private void SilClicked(object sender, RoutedEventArgs e)
        {
            OrderRequest orderApiModel = new OrderRequest();
            orderApiModel.Symbol = Symbol;
            orderApiModel.AccountId = SelectedOrderApiModel.AccountId;
            orderApiModel.BrokageId = SelectedOrderApiModel.BrokageId;
            orderApiModel.Price = SelectedOrderApiModel.Price;
            orderApiModel.Quantity = SelectedOrderApiModel.OrderQty;
            orderApiModel.LeavesQty = SelectedOrderApiModel.LeavesQty;
            orderApiModel.OrderId2 = SelectedOrderApiModel.OrderId2;
            orderApiModel.OrderSide = SelectedOrderApiModel.OrderSide;
            orderApiModel.OrderId = SelectedOrderApiModel.OrderId;
            orderApiModel.OrderId2 = SelectedOrderApiModel.OrderId2;


            orderApiModel.OrderType = '2';
            orderApiModel.TimeInForce = SelectedOrderApiModel.TimeInForce;



            orderApiModel.TransactionType = '1';

            _tcpClientService.SendCancelOrder(orderApiModel);
        }
    }
}
