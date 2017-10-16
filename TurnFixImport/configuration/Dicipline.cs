namespace TurnFixImport.configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Dicipline : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
        }

        [ConfigurationProperty("turnfixid", IsRequired = false)]
        public string TurnFixId
        {
            get
            {
                return this["turnfixid"] as string;
            }
        }

        [ConfigurationProperty("gymnetid", IsRequired = true)]
        public int GymNetId
        {
            get
            {
                return (int)this["gymnetid"];
            }
        }

        [ConfigurationProperty("m", IsRequired = true)]
        public bool Male
        {
            get
            {
                return (bool)this["m"];
            }
        }

        [ConfigurationProperty("f", IsRequired = true)]
        public bool Female
        {
            get
            {
                return (bool)this["f"];
            }
        }
    }
}
