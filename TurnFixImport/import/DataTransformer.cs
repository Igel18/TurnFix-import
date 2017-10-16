namespace TurnFixImport.import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class DataTransformer
    {
        private readonly string _path;

        public DataTransformer(string path)
        {
            _path = path;
        }

        public XElement TransformToXML()
        {
            var result = File.ReadAllText(_path, Encoding.UTF8);

            result = result.Replace("=\r\n", string.Empty);
            result = result.Replace("=0A=", string.Empty);
            result = result.Replace("=20", " ");

            var regex = new Regex("(=[0-9a-fA-F]{2}){1,2}");

            var matched = regex.Matches(result);

            foreach (var m in matched)
            {
                var tmp = m.ToString().Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                List<byte> bytes = new List<byte>();

                foreach (var entry in tmp)
                {
                    var res = Int32.Parse(entry, System.Globalization.NumberStyles.HexNumber);

                    bytes.Add(Convert.ToByte(res));
                }

                var surrogate = Encoding.UTF8.GetString(bytes.ToArray());

                result = result.Replace(m.ToString(), surrogate);
            }

            var doc = XDocument.Parse(result);

            return doc.Root;

            //File.WriteAllText("demo.xml", result);
        }
    }
}
