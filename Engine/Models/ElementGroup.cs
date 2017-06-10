using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class ElementGroup : IMeasureElement
    {
        public IList<IMeasureElement> GroupElements { get; set; } = new List<IMeasureElement>();

        public ElementGroup(IList<IMeasureElement> elements)
        {
            foreach (var element in elements)
            {
                GroupElements.Add(element);
            }
        }

        public ElementGroup(params IMeasureElement[] elements)
        {
            foreach (var element in elements)
            {
                GroupElements.Add(element);
            }
        }

        public static bool operator ==(ElementGroup a, ElementGroup b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            return a.GroupElements.SequenceEqual(b.GroupElements);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
            {
                return false;
            }
            
            var p = obj as ElementGroup;
            if ((System.Object)p == null)
            {
                return false;
            }

            return GroupElements.SequenceEqual(p.GroupElements);
        }


        public static bool operator !=(ElementGroup a, ElementGroup b)
        {
            return !(a == b);
        }
    }
}
