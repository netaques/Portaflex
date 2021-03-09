using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Portaflex.Data
{
    [Serializable()]
    public class Directing : AbstractDepartment
    {
        public Directing() : base()
        {
            Password = "";
        }
    }
}
