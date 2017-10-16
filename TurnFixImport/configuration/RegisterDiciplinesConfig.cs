namespace TurnFixImport.configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RegisterDiciplinesConfig : ConfigurationSection
    {

        public static RegisterDiciplinesConfig GetConfig()
        {
            return (RegisterDiciplinesConfig)System.Configuration.ConfigurationManager.GetSection("RegisterDiciplines") ?? new RegisterDiciplinesConfig();
        }

        [System.Configuration.ConfigurationProperty("DiciplineMappings")]
        [ConfigurationCollection(typeof(DiciplineMappings), AddItemName = "Dicipline")]
        public DiciplineMappings DiciplineMappings
        {
            get
            {
                object o = this["DiciplineMappings"];
                return o as DiciplineMappings;
            }
        }
    }
}
