using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerFitting
{
    [Serializable]
    public class ComputerPart : object
    {
       public String name;
        public String price;
       public  String note;
       public List<String> notCompatible;
      
    }
  
}
