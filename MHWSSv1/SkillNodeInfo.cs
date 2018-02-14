using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHWSSv1
{
    public class SkillNodeInfo
    {
        protected string type;
        protected string name;
        protected string limit;
        protected string need;

        public SkillNodeInfo()
        {
            type = null;
            name = null;
            limit = "0";
            need = "0";
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        public string Need
        {
            get { return need; }
            set { need = value; }
        }
    }
}
