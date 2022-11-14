using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ahed_project.MasterData.ProjectClasses
{
    public class ProjectInfoGet : INotifyPropertyChanged
    {
        private int _project_id ;
        private int? _revision ;
        private string _name ;
        private string _customer ;
        private string _contact ;
        private string _customer_reference ;
        private string _description ;
        private string _units ;
        private int? _number_of_decimals ;
        private string _category ;
        private string _keywords ;
        private string _comments;
        private string _createdAt;
        private string _updatedAt;
        public int project_id
        {
            get => _project_id;
            set
            {
                _project_id = value;
                OnPropertyChanged("project_id");
            }
        }
        public int? revision
        {
            get => _revision;
            set
            {
                _revision = value;
                OnPropertyChanged("revision");
            }
        }
        public string name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }
        public string customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged("customer");
            }
        }
        public string contact
        {
            get => _contact;
            set
            {
                _contact = value;
                OnPropertyChanged("contact");
            }
        }
        public string customer_reference
        {
            get => _customer_reference;
            set
            {
                _customer_reference = value;
                OnPropertyChanged("customer_reference");
            }
        }
        public string description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }
        public string units
        {
            get => _units;
            set
            {
                _units = value;
                OnPropertyChanged("units");
            }
        }
        public int? number_of_decimals
        {
            get => _number_of_decimals;
            set
            {
                _number_of_decimals = value;
                OnPropertyChanged("number_of_decimals");
            }
        }
        public string category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged("category");
            }
        }
        public string keywords
        {
            get => _keywords;
            set
            {
                _keywords = value;
                OnPropertyChanged("keywords");
            }
        }
        public string comments
        {
            get => _comments;
            set
            {
                _comments = value;
                OnPropertyChanged("comments");
            }
        }
        public string createdAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                OnPropertyChanged("createdAt");
            }
        }
        public string updatedAt
        {
            get => _updatedAt;
            set
            {
                _updatedAt = value;
                OnPropertyChanged("updatedAt");
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
