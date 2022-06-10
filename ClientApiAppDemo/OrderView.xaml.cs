using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ClientApiAppDemo.Annotations;
using Matriks.Api;
using Matriks.API.Shared;
using Matriks.ApiClient;
using Newtonsoft.Json;

namespace ClientApiAppDemo
{

    public enum OperationType
    {
        Add,
        Edit
    }
    /// <summary>
    /// Interaction logic for OrderView.xaml
    /// </summary>
    public partial class OrderView : Window,INotifyPropertyChanged
    {
        public OperationType OpType { get; set; }

        public Visibility IsEditVisibility
        {
            get => _isEditVisibility;
            set
            {
                _isEditVisibility = value; 
                OnPropertyChanged();
            } 
        }

        public Visibility SymbolBoxVisibility
        {
            get => _symbolBoxVisibility;
            set
            {
                _symbolBoxVisibility = value;
                OnPropertyChanged();
                OnPropertyChanged("SymbolLabelVisiblirVisibility");
            } 
        }

        public Visibility SymbolLabelVisiblirVisibility
        {
            get
            {
                 if (SymbolBoxVisibility == Visibility.Visible) 
                    return Visibility.Collapsed; 
                return  Visibility.Visible;
            }
            
        }

        public bool IsEdit
        {
            get => _isEdit;
            set
            {
                _isEdit = value;
                if (IsEdit)
                {
                    SymbolBoxVisibility = Visibility.Collapsed; 
                    IsEditVisibility = Visibility.Collapsed;
                }

                else
                {
                    IsEditVisibility = Visibility.Visible;
                    SymbolBoxVisibility = Visibility.Visible;
                }
                    
                OnPropertyChanged();
            } 
        }

        public TcpClientService TcpClientService { get; set; }

        private string _selectedAccount;
        private string _symbol;
        private decimal _volume;
        private decimal _price;
        private string _selectedOrderType;
        private string _selectedValidityType;
        private bool _isBuy;
        private string _orderId;
        private bool _isEdit;
        private Visibility _isEditVisibility;
        private Visibility _symbolBoxVisibility;
        private Visibility _symbolLabelVisiblirVisibility;

        public string OrderId
        {
            get => _orderId;
            set
            {
                _orderId = value;
                OnPropertyChanged();
            }
        }

        public string OrderId2 { get; set; }

        public string SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged();
            } 
        }

        public string SelectedBrokage { get; set; }

        public string Symbol
        {
            get => _symbol;
            set
            {
                _symbol = value;
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

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            } 
        }

        public string SelectedOrderType
        {
            get => _selectedOrderType;
            set
            {
                _selectedOrderType = value;
                OnPropertyChanged();
            } 
        }

        public string SelectedValidityType
        {
            get => _selectedValidityType;
            set
            {
                _selectedValidityType = value;
                OnPropertyChanged();
            }
        }

        public bool IsBuy
        {
            get => _isBuy;
            set
            {
                _isBuy = value;
                OnPropertyChanged();
            } 
        }

        public string ClientOrderId { get; set; }

        public OrderView()
        {
            InitializeComponent();
            DataContext = this;
        }

        private static Random _rng = new Random();
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            OrderRequest orderApiModel = new OrderRequest();
            orderApiModel.Symbol = Symbol;
            orderApiModel.AccountId = SelectedAccount;
            orderApiModel.BrokageId = SelectedBrokage;
            orderApiModel.Price = Price;
            orderApiModel.Quantity = Volume;
            orderApiModel.LeavesQty = Volume;
            orderApiModel.OrderId2 = OrderId2;
            orderApiModel.OrderQty = Volume;
            if (IsBuy)
                orderApiModel.OrderSide = 1;
            else
                orderApiModel.OrderSide = 2;

            orderApiModel.OrderType = '2';

            if (OrderTypeCombo.SelectionBoxItem.ToString() == "LMT")
            {
                orderApiModel.OrderType = '2';

            }
            else if (OrderTypeCombo.SelectionBoxItem.ToString() == "PYS")
            {

                orderApiModel.OrderType = '1';
            }
            else if (OrderTypeCombo.SelectionBoxItem.ToString() == "PLM")
            {
                orderApiModel.OrderType = 'R';
                
            }

            if (ValidityTypeCombo.SelectionBoxItem.ToString() == "GUN")
            {
                orderApiModel.TimeInForce = '0';

            }
            else if (ValidityTypeCombo.SelectionBoxItem.ToString() == "KIE")
            {

                orderApiModel.TimeInForce = '3';
            }
            else if (ValidityTypeCombo.SelectionBoxItem.ToString() == "TAR")
            {
                orderApiModel.TimeInForce = '6';
            }


            orderApiModel.TransactionType = '1';
            orderApiModel.OrderId = OrderId;
            if (IsEdit)
            {
                orderApiModel.ClientOrderId = ClientOrderId;
                TcpClientService.SendEditOrder(orderApiModel);
                this.Close();
            }

            else
            {
                orderApiModel.ClientOrderId = GetTimeStamp();
                orderApiModel.OrdStatus = '0';
                TcpClientService.SendNewOrder(orderApiModel);
            }
        }
        public static string GetTimeStamp()
        {
            var randomNumber = _rng.Next(1000000);
            return (DateTime.Now.Ticks + randomNumber).ToString("00000000000000000000");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
