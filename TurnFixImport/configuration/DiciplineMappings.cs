namespace TurnFixImport.configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class DiciplineMappings : ConfigurationElementCollection
    {
        public Dicipline this[int index]
        {
            get
            {
                return base.BaseGet(index) as Dicipline;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public new Dicipline this[string responseString]
        {
            get { return (Dicipline)BaseGet(responseString); }
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }

        protected override System.Configuration.ConfigurationElement CreateNewElement()
        {
            return new Dicipline();
        }

        protected override object GetElementKey(System.Configuration.ConfigurationElement element)
        {
            var e = (Dicipline)element;

            var key = e.Name + (e.Male ? "_m" : string.Empty) + (e.Female ? "_f" : string.Empty);

            return key;
        }
    }    
}
