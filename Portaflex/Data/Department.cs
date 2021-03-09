using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Portaflex.Data
{
    [Serializable()]
    public class Department : AbstractDepartment
    {
        public event EventHandler InternalDirChanged;

        public Department() : base()
        {
            SubDepartments = new BindingList<SubDepartment>();
            Password = "";
            Locked = true;
            SubDepartments.ListChanged +=new ListChangedEventHandler(Subs_ListChanged);
        }

        public Department(Total t)
            : base()
        {
            SubDepartments = new BindingList<SubDepartment>();
            SubDepartments.ListChanged += new ListChangedEventHandler(Subs_ListChanged);
            Parent = t;
            addValues();
            Locked = true;
            //Parent.Budgets.ListChanged += new ListChangedEventHandler(Budgets_ListChanged);
        }        

        private Total parent;
        [XmlIgnore]
        public Total Parent
        {
            get { return parent; }
            set
            {
                parent = value;
                if (parent != null)
                {
                    addValues(); 
                    parent.Budgets.ListChanged += new ListChangedEventHandler(Budgets_ListChanged);
                }
            }
        }

        private void Subs_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                SubDepartment sub = SubDepartments[e.NewIndex];
                if (Parent != null)
                {
                    int valuesToAdd = Parent.Budgets.Count - sub.Values.Count;
                    for (int i = 0; i < valuesToAdd; i++)
                    {
                        sub.Values.Add(0);
                    }
                }
                sub.Values.ListChanged += new ListChangedEventHandler(
                    delegate(object s, ListChangedEventArgs lcea)
                    {
                        if (lcea.ListChangedType == ListChangedType.ItemChanged)
                        {
                            DepartmentChangeEventArgs ea = new DepartmentChangeEventArgs(DepartmentChangeTypes.Value);
                            ea.AffectedValueIndex = lcea.NewIndex;
                            //ea.AffectedSub = (SubDepartment)sender;
                            OnChange(ea);
                        }
                    }
                );
            }
        }

        /// <summary>
        /// Pozivat pro inicializaci oddeleni, kdyz uz existuje rodic s ucty. Pro kazdy ucet se vlozi do oddeleni i pododdeleni hodnota nula.
        /// </summary>
        private void addValues()
        {
            if (Parent != null)
            {
                for (int i = 0; i < Parent.Budgets.Count - Values.Count; i++)
                {
                    Values.Add(0);
                    foreach (SubDepartment sub in SubDepartments)
                        sub.Values.Add(0);
                }
                /*
                foreach (Budget b in Parent.Budgets)
                {
                    Values.Add(0);
                    foreach (SubDepartment sub in SubDepartments)
                        sub.Values.Add(0);
                }*/
            }
        }

        private void Budgets_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                int index = e.NewIndex;
                Values.Insert(index, 0);
                foreach (SubDepartment sub in SubDepartments)
                    sub.Values.Insert(index, 0);
                if (InternalDir != null)
                    InternalDir.Values.Insert(index, 0);
            }
            else if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                Values.RemoveAt(e.NewIndex);
                foreach (SubDepartment sub in SubDepartments)
                    sub.Values.RemoveAt(e.NewIndex);
                if (InternalDir != null)
                    InternalDir.Values.RemoveAt(e.NewIndex);
            }
        }

        public double TotalSum(int row)
        {
            if(row < 0)
                return 0;
            double sum = Values.Count > row ? Values[row] : 0;
            foreach (SubDepartment s in SubDepartments)
                sum += s.Values[row];
            if (InternalDir != null && InternalDir.Values.Count > row)
                sum += InternalDir.Values[row];
            return sum;
        }
        private Directing dir;
        public Directing InternalDir
        {
            get { return dir; }
            set
            {
                dir = value;
                if (dir != null && parent != null && parent.Budgets != null)
                {
                    if (dir.Values.Count < parent.Budgets.Count)
                    {
                        dir.Values.Clear();
                        foreach (Budget b in parent.Budgets)
                            dir.Values.Add(0);
                    }
                    dir.Values.ListChanged += new ListChangedEventHandler
                    (
                        delegate(object sender, ListChangedEventArgs e)
                        {
                            DepartmentChangeEventArgs ea = new DepartmentChangeEventArgs(DepartmentChangeTypes.Value);
                            ea.AffectedValueIndex = e.NewIndex;
                            OnChange(ea);
                        }
                    );
                }
                else
                {
                    DepartmentChangeEventArgs ea = new DepartmentChangeEventArgs(DepartmentChangeTypes.Dir);
                    OnChange(ea);
                }
                if (InternalDirChanged != null)
                    InternalDirChanged(this, new EventArgs());
            }
        }
        public BindingList<SubDepartment> SubDepartments { get; set; }
    }
}
