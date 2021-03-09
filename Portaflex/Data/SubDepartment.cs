using System;

namespace Portaflex.Data
{
    [Serializable()]
    public class SubDepartment : AbstractDepartment
    {
        public SubDepartment()
            : base()
        {
            Hidden = false;
        }
        public bool Hidden { get; set; }
        private decimal intproc;
        public decimal IntProc
        {
            get { return intproc; }
            set
            {
                var oldproc = intproc;
                intproc = value;
                if (oldproc != intproc)
                    OnChange(new DepartmentChangeEventArgs(DepartmentChangeTypes.IntProc));
            }
        }
    }

    
}
