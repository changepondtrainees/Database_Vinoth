using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
namespace DataBase
{
    public class Create
    {       
        Dictionary<string,string> ColumnNameTypePair=new Dictionary<string,string>();
        string FileName = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public string QueryValidator(string Query)
        {
            string CreateCommand = Query.Substring(0, Query.IndexOf('('));          
            CreateCommand = CreateCommand.Trim();
            Match CreateCheck = Regex.Match(CreateCommand, @"create table ([\w]+)");
            if (CreateCheck.Success)
            {
                ColumnSplit(Query, CreateCommand);
                FileName=CreateXml(CreateCommand);
            }
            else
            {
                Console.WriteLine("\tPlease check your syntax");
            }
            return FileName;            
            }

        /// <summary>        
        /// 
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="CreateCommand"></param>
        public void ColumnSplit(string Query, string CreateCommand)
        {
            string Columns = Query.Substring(CreateCommand.Length+1, Query.LastIndexOf(')')-CreateCommand.Length+1);
            Match ColumnCheck = Regex.Match(Query.Substring(Query.IndexOf('(')), @"(([\(\w\s]+)([number(\d),varchar2(\d)]),?)+([\)])([';'])$");
            string[] ColumnNameType=Columns.Split(',');
            string[] ColumnArray;
            
            foreach (string Column in ColumnNameType)
            {
                if (ColumnCheck.Success)
                {
                    ColumnArray = Column.Split(' ');
                    ColumnNameTypePair.Add(ColumnArray[0], ColumnArray[1]);
                }
                else
                {
                    Console.WriteLine("\tPlease check your syntax");
                    break;
                }
            }
        }

        /// <summary>
        /// Creating The XML File for the Table
        /// </summary>
        /// <param name="CreateCommand"></param>
        public string CreateXml(string CreateCommand)
        {
            string filename = CreateCommand.Substring(CreateCommand.LastIndexOf(" "));           
            if (File.Exists(filename + ".xml"))
            {
                Console.WriteLine("\tTable already exists.");
            }
            else
            {
                using (XmlWriter writer = XmlWriter.Create(filename + ".xml"))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("table");
                    writer.WriteAttributeString("Name", filename);
                    foreach (var Item in ColumnNameTypePair)
                    {
                        writer.WriteStartElement(Item.Key);
                        writer.WriteAttributeString("Type", Item.Value);
                        writer.WriteString(" ");                        
                        writer.WriteEndElement();
                    }                                        
                    writer.WriteEndDocument();
                }
                Console.WriteLine("\tTable created succesfully");
                
            }
            return filename;
        }        
    }
}

 
