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
        protected string level;
        protected string desc;

        public SkillNodeInfo()
        {
            type = null;
            name = null;
            limit = "0";
            need = "0";
            level = "0";
            desc = null;
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

        public string Level
        {
            get { return level; }
            set { level = value; }
        }

        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }
    }
}
