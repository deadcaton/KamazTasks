using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Excel = Microsoft.Office.Interop.Excel;

namespace Task2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Excel.Application excel = new Excel.Application();

            // Открытие рабочей книги только для чтения.
            Excel.Workbook workbook = excel.Workbooks.Open(
                @"C:/Users/ir_ya/Dropbox/Github/KamazTasks/Task2/data/ViewerMessages.xlsx",
                Type.Missing, true, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);

            // Получение первого рабочего листа.
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1];

            // Получение значений столбцов от A до B.
            int iLastRow = sheet.Cells[sheet.Rows.Count, "A"].End[Excel.XlDirection.xlUp].Row;
            object[,] exData = (object[,])sheet.Range["A1:B" + iLastRow].Value;


            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"C:/Users/ir_ya/Dropbox/Github/KamazTasks/Task2/data/ViewerMessages.xml");

            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlNode xnode in xRoot)
            {
                string ruVar = "";
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    // если узел - company
                    if (childnode.Name == "variant")
                    {
                        switch (childnode.Attributes.GetNamedItem("language").Value)
                        {
                            case "en_US":
                                for (int a = 1; exData.Length > a; a++)
                                {
                                    if (childnode.InnerText == exData[a, 1].ToString())
                                    {
                                        ruVar = exData[a, 2].ToString();
                                        break;
                                    }
                                }
                                break;

                            case "ru_RU":
                                childnode.InnerText = ruVar;
                                break;
                        }
                    }
                }
            }

            xDoc.Save(@"C:/Users/ir_ya/Dropbox/Github/KamaTasks/Task2/data/ViewerMessagesDone.xml");


            // Закрытие книги без сохранения изменений.
            workbook.Close(false, Type.Missing, Type.Missing);

            // Закрытие сервера Excel.
            excel.Quit();
        }
    }
}
