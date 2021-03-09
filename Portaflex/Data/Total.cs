using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Portaflex.Data
{
    [Serializable()]
    public class Total
    {
        public BindingList<Department> Departments { get; set; }
        
        private Directing dir;
        public Directing Dir
        {
            get { return dir; }
            set
            {
                dir = value;
                if (dir != null)
                {
                    if (dir.Values.Count < Budgets.Count)
                    {
                        dir.Values.Clear();
                        foreach (Budget b in Budgets)
                            dir.Values.Add(0);
                    }
                }
                if (DirChanged != null)
                    DirChanged(this, new EventArgs());
            }
        }

        public BindingList<Budget> Budgets { get; set; }
        public event BudgetChangedHandler BudgetChanged;
        public event EventHandler DirChanged;

        public Total()
        {
            Departments = new BindingList<Department>();            
            Budgets = new BindingList<Budget>();
            Departments.ListChanged += new ListChangedEventHandler(Departments_ListChanged);
            Budgets.ListChanged += new ListChangedEventHandler(Budgets_ListChanged);
        }

        private void Departments_ListChanged(object sender, ListChangedEventArgs e)
        {
                                  
        }

        private void Budgets_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                Budget b = Budgets[e.NewIndex];
                b.BudgetChanged += new BudgetChangedHandler(OnBudgetChanged);
                if (Dir != null && Budgets.Count > Dir.Values.Count)
                    Dir.Values.Insert(e.NewIndex, 0);
            }
            else if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                if (Dir != null)
                    Dir.Values.RemoveAt(e.NewIndex);
            }
        } 
 /*
        public void addNewBudget(string name, string id)
        {
            Budget b = new Budget(name, id);
            b.BudgetChanged += new BudgetChangedHandler(budgetChanged);
            Budgets.Add(b);
            budgetChanged(b,new BudgetEventArgs("created",Budgets.IndexOf(b)));
            if(Dir != null)
                Dir.Values.Add(0);
            foreach (Department d in Departments)
            {
                d.addValue(0);
            }
        }
*/
        private void OnBudgetChanged(object sender, BudgetEventArgs e)
        {
            e.Index = Budgets.IndexOf((Budget)sender);
            if(BudgetChanged != null)
                BudgetChanged(sender, e);
        }

        public bool ContainsBudget(string id, string name)
        {
            foreach (Budget b in Budgets)
            {
                if (b.ID == id && b.Name == name)
                    return true;                
            }
            return false;
        }

        public int BudgetIndex(string id, string name)
        {
            foreach (Budget b in Budgets)
            {
                if (b.ID == id && b.Name == name)
                    return Budgets.IndexOf(b);
            }
            return -1;
        }
    }

    public static class DeepCloner
    {
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
