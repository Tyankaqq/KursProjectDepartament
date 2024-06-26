using KursProjectDepartament.Model;
using KursProjectDepartament.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KursProjectDepartament.ModelView
{
    internal class HumanViewModel : Baseclass
    {
        HumanDepartmentDbContext db = new HumanDepartmentDbContext();
        private AddEditHuman? HumanPage;
        public ObservableCollection<Employee>? StudentList { get; set; }

        private Employee? selecteEmployee;
        public Employee? SelectedEmployee
        {
            get { return selecteEmployee; }
            set
            {
                selecteEmployee = value;
                OnPropertyChanged("SelectedHuman");
            }
        }
        public HumanViewModel(AddEditHuman page)
        {
            this.HumanPage = page;
            db.Database.EnsureCreated();
            StudentList = db.Employees.Local.ToObservableCollection();
        }
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        AddEditHuman window = new AddEditHuman();
                    }));
            }
        }
    }
}

