using System;

namespace Portaflex.Data
{
    public delegate void BudgetChangedHandler(object sender, BudgetEventArgs e);

    [Serializable()]
    public class Budget
    {
        public event BudgetChangedHandler BudgetChanged;

        private string name;
        private string id;
        private bool income;

        public Budget() : this("nespecifikováno","nespecifikováno"){ }

        public Budget(string name, string ID, bool sum = false, bool income = false)
        {
            this.Name = name;                                                                                                                
            this.ID = ID;
            this.Sum = sum;
            this.income = income;
        }

        public bool Sum
        {
            get;
            set;
        }

        public bool Income
        {
            get { return income; }
            set
            {
                var oldincome = income;
                income = value;
                if (oldincome != income)
                    OnChange(new BudgetEventArgs("income", 0));
            }
        }

        public string Name
        {
            get {return name;}
            set
            {
                var oldname = name;
                name = value;
                if(oldname != name)
                    OnChange(new BudgetEventArgs("name",0));
            }
        }

        public string ID
        {
            get {return id;}
            set
            {
                var oldid = id;
                id = value;
                if(oldid != id)
                    OnChange(new BudgetEventArgs("id",0));
            }
        }

        private void OnChange(BudgetEventArgs e)
        {
            if(BudgetChanged != null)
                BudgetChanged(this,e);
        }
    }

    public class BudgetEventArgs : EventArgs
    {
        public string Type { get; set; }
        public int Index { get; set; }
        public BudgetEventArgs(string type, int index)
        {
            this.Type = type;
            this.Index = index;
        }
    }
}
