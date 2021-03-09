using System;
using System.Xml.Serialization;
using System.Xml;

namespace Portaflex.Data
{
        class XMLSerializer
        {
            private Type[] extra;

            /// <summary>
            /// Konstruktor, nastavi atribut fullFilePath.
            /// </summary>
            /// <param name="path">Cesta pro umisteni XML souboru.</param>
            public XMLSerializer(string path)
            {
                this.fullFilePath = path;
                extra = new Type[] { typeof(Budget), typeof(Department), typeof(Directing), typeof(SubDepartment), typeof(Total), typeof(AbstractDepartment) };
            }

            public XMLSerializer() : this("") { }

            private const string XML_PATH = @"scenario.xml";
            private string fullFilePath;

            /// <summary>
            /// Nacte z XML souboru data a ulozi je to tridy Osoby.
            /// </summary>
            /// <returns>Nactena data v tride Total</returns>
            public Total LoadTotal()
            {
                var s = new XmlSerializer(typeof(Total), extra);
                XmlTextReader xmlReader = null;
                try
                {
                    
                    xmlReader = new XmlTextReader(this.fullFilePath);
                    var total = (Total)s.Deserialize(xmlReader);
                    updateParentReference(total);
                    xmlReader.Close();
                    return total;
                }
                catch
                {
                    if (xmlReader != null)
                        xmlReader.Close();
                    return new Total();
                }
            }

            /// <summary>
            /// Nastavi vsem oddelenim predaneho rodice
            /// </summary>
            /// <param name="total"></param>
            private void updateParentReference(Total total)
            {
                foreach (var d in total.Departments)
                    d.Parent = total;
            }

            /// <summary>
            /// Ulozi data z parametru osoby do XML souboru.
            /// </summary>
            /// <param name="total">Trida Total urcena k serializaci.</param>

            public void SaveTotal(Total total)
            {
                var s = new XmlSerializer(typeof(Total), extra);
                var xmlWriter = new XmlTextWriter(this.fullFilePath, System.Text.Encoding.UTF8);

                s.Serialize(xmlWriter, total);
                xmlWriter.Close();
            }
        }
}
