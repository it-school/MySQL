using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQL
{
    class StatusList : List<string>
    {
        public StatusList()
        {
            this.Add("active");
            this.Add("retired");
            this.Add("sacked");
        }
    }
}
