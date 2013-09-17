using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace DataBase
{
    class Program
    {  
        static void Main()
        {             

            StringBuilder QueryBuild = new StringBuilder();
            StringBuilder CompleteStringBuilder = new StringBuilder();
            string Query=string.Empty;
            while(true)
            {
            Query = Console.ReadLine();
            int Index = Query.LastIndexOf(";"); 
                if (Index != -1) { QueryBuild.Append(Query);
                    string CompleteQuery = Convert.ToString(QueryBuild);
                    CompleteQuery = CompleteQuery.Trim();
                    string[] QuerySplit = CompleteQuery.Split(' ');
                    for (int i = 0; i < QuerySplit.Length; i++)
                    { 
                        if (!QuerySplit[i].Equals(""))
                        {
            QuerySplit[i] = QuerySplit[i].Trim();
            QuerySplit[i] += " ";
            CompleteStringBuilder.Append(QuerySplit[i]); 
            }

                }
                if (CompleteQuery.Equals("exit"))
                {
                    break;
                }
                else if (CompleteQuery.StartsWith("create"))
                {
                    Create OCreate = new Create();
                    OCreate.QueryValidator(Convert.ToString(CompleteStringBuilder));
                }
            }
            else
            {                    
                QueryBuild.Append(Query+" ");                    
            }                
        }
    }
}
}


//            Create oCreate = new Create();
//            Insert oInsert = new Insert();
//            string FileName="";
//            int InsertIteration = 0;
//            Console.WriteLine("\tWelcome to vinoth DataBase");
//            while (true)
//            {
//                string Query = Console.ReadLine();
//                if (Query == "exit")
//                {
//                    break;
//                }
//                else if (Query.StartsWith("create"))
//                {
//                    FileName=oCreate.QueryValidator(Query);
//                }
//                else if(Query.StartsWith("insert"))
//                {
//                    oInsert.QueryValidator(Query, InsertIteration++);
//                }
//            }
//            Console.ReadKey();
//        }
//    }
//}
