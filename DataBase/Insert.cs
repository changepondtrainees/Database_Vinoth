using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
namespace DataBase
{
    public class Insert
    {
        public class InsertColumn
        {
            public InsertColumn() { }
            public string DataType;
            public int Range;
            public string values;
        }
        string FileName = "";
        int InsertIteration = 0;
        Dictionary<string, string> NameValue = null;
        string[] Values;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public void QueryValidator(string Query, int Insert)
        {
            NameValue = new Dictionary<string, string>();
            InsertIteration = Insert;
            string InsertCommand = Query.Substring(0, Query.IndexOf('('));
            InsertCommand = InsertCommand.Trim();
            FileName = InsertCommand.Substring(InsertCommand.LastIndexOf(" "));
            Match InsertCheck = Regex.Match(InsertCommand, @"insert into ([\w]+)");
            if (InsertCheck.Success)
            {
                ValuesSplit(Query, InsertCommand);
            }
            else
            {
                Console.WriteLine("\tPlease check your syntax");
            }
            NameValue = null;
        }

        /// <summary>        
        /// 
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="CreateCommand"></param>
        public void ValuesSplit(string Query, string InsertCommand)
        {
            string ValuesString = Query.Substring(InsertCommand.Length + 1, Query.LastIndexOf(')') - InsertCommand.Length - 1);
            Match ValueCheck = Regex.Match(ValuesString, @"([\w,])+$");
            Values = ValuesString.Split(',');
            if (ValueCheck.Success)
            {
                ParseXml(FileName);
            }
            else
            {
                Console.WriteLine("\tPlease check your syntax");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CreateCommand"></param>
        public void ParseXml(string XmlFile)
        {
            XElement Table = null;
            XElement xElementname = null;
            InsertColumn oInsertColumn = null;
            int Range;
            int i = 0;
            string AttrType = "";
            try
            {
                if (File.Exists(XmlFile + ".xml"))
                {
                    Table = XElement.Load(@"D:\DataBase\DataBase\bin\Debug\" + XmlFile + ".xml");
                }
                else
                {
                    Console.WriteLine("There is No Table To Insert");
                    return;
                }
            }
            catch (FileNotFoundException E)
            {
                Console.WriteLine(E.ToString());
            }
            if (Table.Elements().Count() == Values.Length)
            {
                foreach (XElement child in Table.Elements())
                {
                    oInsertColumn = new InsertColumn();
                    xElementname = child;
                    AttrType = (string)xElementname.Attribute("Type");
                    Range = Convert.ToInt32(AttrType.Substring(AttrType.IndexOf("(") + 1, AttrType.IndexOf(")") - AttrType.IndexOf("(") - 1));

                    oInsertColumn.DataType = AttrType.Substring(0, AttrType.IndexOf("("));
                    oInsertColumn.Range = Range;
                    oInsertColumn.values = Values[i++];
                    MatchValues(oInsertColumn, xElementname);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AttrTypeRange"></param>
        public void MatchValues(InsertColumn oInsertColumn, XElement XElementName)
        {
            Match InsertCheck;
            if (oInsertColumn.DataType.Equals("varchar2"))
            {
                InsertCheck = Regex.Match(oInsertColumn.values, @"([\w])");
                if (InsertCheck.Success)
                {
                    if (oInsertColumn.values.Length <= oInsertColumn.Range)
                    {
                        NameValue.Add((XElementName.Name).ToString(), oInsertColumn.values);
                        StoreValues(NameValue, InsertIteration);
                    }
                    else
                    {
                        NameValue = null;
                        Console.WriteLine("DataType Mismatch");
                        return;
                    }
                }
            }
            else if (oInsertColumn.DataType.Equals("number"))
            {
                InsertCheck = Regex.Match(oInsertColumn.values, @"([\d])");
                if (InsertCheck.Success)
                {
                    if (oInsertColumn.values.Length <= oInsertColumn.Range)
                    {
                        NameValue.Add((XElementName.Name).ToString(), oInsertColumn.values);
                        StoreValues(NameValue, InsertIteration);
                    }
                    else
                    {
                        NameValue = null;
                        Console.WriteLine("DataType Mismatch");
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Enter A Valid DataType");
                return;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="NameValue"></param>
        /// <param name="i"></param>
        public void StoreValues(Dictionary<string, string> NameValue, int i)
        {
            ////if (File.Exists(FileName + "Insert_Values" + ".xml"))
            ////{
            ////    XDocument fromWeb = XDocument.Load(@"D:\DataBase\DataBase\bin\Debug\" + FileName + "Insert_Values" + ".xml");                               
                           
            ////}
            ////else
            ////{               
            //    XmlWriter writer = XmlWriter.Create(FileName + "Insert_Values" + ".xml");
            //    XElement TableName = new XElement("table");
            //    TableName.Add(new XAttribute("Name", FileName));
            //    foreach (var Item in NameValue)
            //    {
            //        TableName.Add(new XElement(Item.Key, Item.Value));
            //    }  
            //    TableName.Save(writer);
            ////}

            using (XmlWriter writer = XmlWriter.Create(FileName + i + ".xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("table");
                writer.WriteAttributeString("Name", FileName);
                foreach (var Item in NameValue)
                {
                    writer.WriteStartElement(Item.Key);
                    writer.WriteString(Item.Value);
                    writer.WriteEndElement();
                }
                writer.WriteEndDocument();
            }
        }
    }
     
}
