using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerFitting
{
    [Serializable]
    public class Board : ComputerPart
    {
        public String socket;
        
       public String maxPci;
        
        public String maxSocket;
        
        public String maxMemory;
        public int identity;
        public Board()
        {
            identity = 6;
        }
    }
}
