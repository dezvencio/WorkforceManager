using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace DataLibrary
{
    public class DataAccess
    {

        public static List<Employee> LoadEmployees()
        {
            List<Employee> loadedEmps = new List<Employee>();

            if (!File.Exists("Employees.xml"))
            {
                System.IO.File.Create("Employees.xml").Close();
            }

            else
            if (new FileInfo("Employees.xml").Length != 0)
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(List<Employee>));
                using (System.IO.StreamReader empsFile = new System.IO.StreamReader("Employees.xml"))
                { 
                    loadedEmps = (List<Employee>)reader.Deserialize(empsFile);
                    empsFile.Close();
                }
            }
            return loadedEmps;
        }

       

    }
}
