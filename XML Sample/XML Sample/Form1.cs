//Youtube Tutorials:
//Part 1: https://www.youtube.com/watch?v=DPY1v7sDK9w
//Part 2: https://www.youtube.com/watch?v=Wa9KbLDtxCI
//Part 3: https://www.youtube.com/watch?v=nPg7CMJ6q60
//Part 4: https://www.youtube.com/watch?v=iSUxvnppFJA
//Part 5: https://www.youtube.com/watch?v=EAvlXSAHRvk
//Part 6: https://www.youtube.com/watch?v=KhYDIgF3OtM

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace XML_Sample
{
    public partial class Form1 : Form
    {

        //For Edit XML
        XmlDocument exDoc;
        string ePath;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML |*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(ofd.FileName);
                MessageBox.Show(xDoc.SelectSingleNode("EAttendance/Person/FirstName").InnerText); //FirstName

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML |*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(ofd.FileName);
               foreach (XmlNode node in xDoc.SelectNodes("EAttendance/Person"))
               {
                   MessageBox.Show(node.SelectSingleNode("FirstName").InnerText);
               }

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) //Edit XML
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML |*.xml";
           if (ofd.ShowDialog() == DialogResult.OK)
            {
                ePath = ofd.FileName;
                exDoc = new XmlDocument();
                exDoc.Load(ePath);
              
                textBox1.Text = exDoc.SelectSingleNode("EAttendance/Person/FirstName").InnerText;
                textBox2.Text = exDoc.SelectSingleNode("EAttendance/Person/LastName").InnerText;
                numericUpDown1.Value = Convert.ToInt32(exDoc.SelectSingleNode("EAttendance/Person/Age").InnerText);
                numericUpDown2.Value = Convert.ToInt32(exDoc.SelectSingleNode("EAttendance/Person/Grade").InnerText);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            exDoc.SelectSingleNode("EAttendance/Person/FirstName").InnerText = textBox1.Text;
            exDoc.SelectSingleNode("EAttendance/Person/LastName").InnerText = textBox2.Text;
            exDoc.SelectSingleNode("EAttendance/Person/Age").InnerText = numericUpDown1.Value.ToString();
            exDoc.SelectSingleNode("EAttendance/Person/Grade").InnerText = numericUpDown2.Value.ToString();
            exDoc.Save(ePath);
        }

        private void button5_Click(object sender, EventArgs e)
        {
         

           // XmlDocument xDoc = new XmlDocument();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true; //Omitting <?xml version="1.0" encoding="utf-8"?>
            settings.IndentChars = "\t";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "XML |*.xml";
            if (sfd.ShowDialog() == DialogResult.OK)
            {

                using (XmlWriter writer = XmlWriter.Create(sfd.FileName, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("EAttendance");//<EAttendance>
                writer.WriteStartElement("Person");//<Person>
                writer.WriteStartElement("FirstName"); //<Name>
                writer.WriteString(textBox3.Text);
                writer.WriteEndElement();//</Name>
                writer.WriteStartElement("LastName"); //<LastName>
                writer.WriteString(textBox4.Text);
                writer.WriteEndElement();//</LastName>
                writer.WriteStartElement("Age");//<Age>
                writer.WriteString(numericUpDown3.Value.ToString());
                writer.WriteEndElement();//</Age>
                writer.WriteStartElement("Grade");//<Grade>
                writer.WriteString(numericUpDown4.Value.ToString());
                writer.WriteEndElement();//</Grade>
                writer.WriteEndElement();//</Person>
                writer.WriteEndElement();//</EAttendance>
                writer.WriteEndDocument();
            }

                

            }
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML |*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                doc.Load(ofd.FileName);
                XmlNode person = doc.CreateElement("Person");
                XmlNode firstname = doc.CreateElement("FirstName");//FirstName
                firstname.InnerText = textBox5.Text;
                person.AppendChild(firstname);
                XmlNode lastname = doc.CreateElement("LastName");//LastName
                lastname.InnerText = textBox6.Text;
                person.AppendChild(lastname);
                XmlNode age = doc.CreateElement("Age");//Age
                age.InnerText = numericUpDown5.Value.ToString();
                person.AppendChild(age);
                XmlNode grade = doc.CreateElement("Grade");//Grade
                grade.InnerText = numericUpDown6.Value.ToString();
                person.AppendChild(grade);
                doc.DocumentElement.AppendChild(person);
                doc.Save(ofd.FileName);

            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML |*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                xDoc.Load(ofd.FileName);
                foreach (XmlNode xNode in xDoc.SelectNodes("EAttendance/Person"))
                {
                    if (xNode.SelectSingleNode("FirstName").InnerText == textBox7.Text)
                    {
                       xNode.ParentNode.RemoveChild(xNode);
                    }

                    xDoc.Save(ofd.FileName);

                }
            }

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            //exDoc.Load(ePath);
            xDoc.LoadXml(textBox8.Text);

        }

        private void button9_Click(object sender, EventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML |*.xml";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                xDoc.Load(ofd.FileName);
                foreach (XmlNode xNode in xDoc.SelectNodes("EAttendance/Person"))
                {
                    if (xNode.SelectSingleNode("FirstName").InnerText == textBox7.Text)
                    {
                        MessageBox.Show(xNode.InnerXml);                        
                    }
                                      
                }
            }
        }
    }
}
