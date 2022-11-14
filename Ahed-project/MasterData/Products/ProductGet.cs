using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ahed_project.MasterData.Products
{
    /// <summary>
    /// Продукт
    /// </summary>
    public class ProductGet : INotifyPropertyChanged
    {
        private string _name;
        private int _product_id;
        private List<ProductProperties> _product_properties;
        private DateTime _createdAt;
        private DateTime _updatedAt;

        /// <summary>
        /// Наименование
        /// </summary>
        public string name 
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(name));
            }
        }
        /// <summary>
        /// Айдишник
        /// </summary>
        public int product_id
        {
            get => _product_id;
            set
            {
                _product_id = value;
                OnPropertyChanged(nameof(product_id));
            }
        }
        public List<ProductProperties> product_properties
        {
            get => _product_properties;
            set
            {
                _product_properties = value;
                OnPropertyChanged(nameof(product_properties));
            }
        }
        public DateTime createdAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged(nameof(createdAt));
            }
        }
        public DateTime updatedAt
        {
            get => _updatedAt;
            set
            {
                _updatedAt = value;
                OnPropertyChanged(nameof(updatedAt));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
