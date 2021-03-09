using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Portaflex.Data
{
    public delegate void DepartmentChangedHandler(AbstractDepartment d, DepartmentChangeEventArgs e);
    [Serializable()]
    public abstract class AbstractDepartment
    {
        public event DepartmentChangedHandler DepartmentChanged;
        private string name;
        private decimal proc;
        public string Name
        {
            get { return name; }
            set
            {
                string oldname = name;
                name = value;
                if (oldname != name)
                    OnChange(new DepartmentChangeEventArgs(DepartmentChangeTypes.Name));
            }
        }
        public decimal Proc
        {
            get { return proc; }
            set
            {
                decimal oldproc = proc;
                proc = value;
                if (oldproc != proc)
                    OnChange(new DepartmentChangeEventArgs(DepartmentChangeTypes.Proc));
            }
        }
        //public List<Budget> Budgets { get; set; }
        public BindingList<double> Values { get; set; }
        

        public AbstractDepartment()
        {
            //Budgets = new List<Budget>();
            Values = new BindingList<double>();
            Locked = true;
            
        }

        private void Budgets_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                Values.Insert(e.NewIndex, 0);
            }
            else if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                Values.RemoveAt(e.NewIndex);
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                Locked = value != "";
                password = value;
            }
        }
        private string password;
        private bool locked;
        [XmlIgnore]
        public bool Locked
        {
            get { return locked; }
            set
            {
                if (value != locked)
                {
                    locked = value;
                    OnChange(new DepartmentChangeEventArgs(DepartmentChangeTypes.Value));
                }
            }
        }

        protected void OnChange(DepartmentChangeEventArgs e)
        {
            if (DepartmentChanged != null)
                DepartmentChanged(this, e);
        }
    }

    public enum DepartmentChangeTypes
    {
        Name,
        Proc,
        Value,
        Dir,
        Lock,
        IntProc
    };

    public class DepartmentChangeEventArgs : EventArgs
    {
        public DepartmentChangeTypes Type { get; set; }
        public SubDepartment AffectedSub { get; set; }
        public int AffectedValueIndex { get; set; }
        public DepartmentChangeEventArgs(DepartmentChangeTypes type)
        {
            Type = type;
        }
    }
}
