using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Pizza.Models;
using Pizza.Services;
using Pizza.ViewModels;
using Unity;

namespace Pizza
{
    class MainWindowViewModel : BindableBase
    {
        private AddEditCustomerViewModel _addEditCustomerVewModel;
        private CustomerListViewModel _customerListViewModel;
        private OrderPerpViewModel _orderPrepViewModel;
        private OrderViewModer _orderViewModel;

        private ICustomerRepository _customerRepository = new CustomerRepository();

        public MainWindowViewModel()
        {
            NavigationCommand = new RelayCommand<string>(OnNavigation);
            //_customerListViewModel = new CustomerListViewModel(new CustomerRepository()) ;
            //_addEditCustomerVewModel = new AddEditCustomerViewModel(new CustomerRepository()) ; 
            _customerListViewModel = RepoContainer.Container.Resolve<CustomerListViewModel>();  
            _addEditCustomerVewModel = RepoContainer.Container.Resolve<AddEditCustomerViewModel>();

            _orderViewModel= RepoContainer.Container.Resolve<OrderViewModer>();

            _customerListViewModel.AddCustomerRequested +=NavigationToAddCustomer;
            _customerListViewModel.EditCustomerRequested += NavigationToEditCustomer;
            //_customerListViewModel.PlaceOrderRequested += NavigateToOrder;
            _customerListViewModel.ObservOrderCustomerRequested += NavigateToObservOrderCustomer;
           
        }
        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public RelayCommand<string> NavigationCommand { get; private set; }

        //открывать список клиентов
        private void OnNavigation(string dest)
        {
            switch (dest)
            {
                case "orderPrep":
                    CurrentViewModel = _orderPrepViewModel; break;
                case "customers":
                default:
                       CurrentViewModel = _customerListViewModel; break;
            }
        }
        
        //открывать окно для редактирования клиента
        private void NavigationToEditCustomer(Customer customer)
        {
            _addEditCustomerVewModel.IsEditeMode = true; 
            _addEditCustomerVewModel.SetCustomer(customer);
            CurrentViewModel = _addEditCustomerVewModel;

        }

        private void NavigateToObservOrderCustomer(Customer? customer) 
        {
            
            _orderViewModel.LoadOrderCustomers();
            _orderViewModel.SelectedCustomer = customer;


            CurrentViewModel = _orderViewModel;
        }

        //открывать окно для добавления клиента
        //private void NavigationToAddCustomer(Customer cust)----------------------------
        private void NavigationToAddCustomer()
        {
            _addEditCustomerVewModel.IsEditeMode = false;
            _addEditCustomerVewModel.SetCustomer(new Customer
            {
                Id = Guid.NewGuid(),
            });
            CurrentViewModel = _addEditCustomerVewModel;
            
        }

        //окно для оформления заказа
        //private void NavigateToOrder(Customer? customer)
        //{

        //    
        //    CurrentViewModel = _orderViewModel;
        //}
    }
}
