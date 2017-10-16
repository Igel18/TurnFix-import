namespace TurnFixImport.import.mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SexMapper
    {
        public static int Map(int value)
        {
            switch (value)
            {
                case 1:
                    return 1;
                case 2:
                    return 0;
                default:
                    throw new ArgumentException("Sex of participant could not be determined!");
            }
             
        }
    }
}
