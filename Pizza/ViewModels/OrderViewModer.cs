using Pizza.Models;
using Pizza.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pizza.ViewModels
{
    class OrderViewModer : BindableBase
    {
        private readonly IOrderRepository _repository;


        public OrderViewModer(IOrderRepository repository)
        {
            _repository = repository;

            Orders = new ObservableCollection<Order>();
            LoadOrderCustomers();
        }

        public ObservableCollection<Order>? Order;

        public async Task LoadOrderCustomers()
        {
            if (SelectedCustomer == null)
            {
                throw new InvalidOperationException("Selected null!");
            }

            var orders = await _repository.GetOrdersByCustomerAsync(SelectedCustomer.Id);
            Orders.Clear();
            foreach (var order in orders.OrderBy(o => o.OrderDate))
            {
                Orders.Add(order);
            }
        }

        private Customer _selectedCustomer;

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                SetProperty(ref _selectedCustomer, value);
            }
        }

        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set => SetProperty(ref _orders, value);
        }
        public RelayCommand CloseCommand { get; }

        public event Action? Done;


    }
}
