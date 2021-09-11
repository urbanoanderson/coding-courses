using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.LinkModels
{
    public class Link
    {
        public Link()
        {
            // Only for XML serialization purposes
        }

        public Link(string href, string rel, string method)
        {
            this.Href = href;
            this.Rel = rel;
            this.Method = method;
        }

        public string Href { get; set; }

        public string Rel { get; set; }

        public string Method { get; set; }
    }
}
