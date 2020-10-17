using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2EF2020
{
    public partial class Form1 : Form

    {
        EmployeeProjectDBEntities dbEntities = new EmployeeProjectDBEntities();
        public Form1()
        {
            InitializeComponent();
        }
        private void cleanTextBox()
        {
            textBoxId.Text = string.Empty;
            textBoxFn.Text = string.Empty;
            textBoxLn.Text = string.Empty;
            textBoxPnumber.Text = string.Empty;
            textBoxEmail.Text = string.Empty;
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            Employee emp = new Employee();
            //check for duplicated EmployeeId
            int id = Convert.ToInt32(textBoxId.Text.Trim());
            var foundEmp =dbEntities.Employees.Find(id); 
            if (foundEmp != null)
            {
                MessageBox.Show("Employee with this Id is there" , "Error", MessageBoxButtons.OK , MessageBoxIcon.Error);
                textBoxId.Clear();
                textBoxId.Focus();
            }
            else
            {
                emp.EmployeeId = Convert.ToInt32(textBoxId.Text);
                emp.Email = textBoxEmail.Text;
                emp.FirstName = textBoxFn.Text;
                emp.LastName = textBoxLn.Text;
                emp.PhoneNumber = textBoxPnumber.Text;
                dbEntities.Employees.Add(emp);
                dbEntities.SaveChanges();
                MessageBox.Show("Employee save successfully");
                cleanTextBox();
            }
            

        }
        private void PopulateList()
        {
             //var empList =  dbEntities.Employees.Select(x=>x).ToList();
            var empList = from emp in dbEntities.Employees
                          select emp;
            listViewEmployee.Items.Clear();
            foreach (var emp in empList)
            {
                ListViewItem item = new ListViewItem(Convert.ToString(emp.EmployeeId));
                item.SubItems.Add(emp.FirstName);
                item.SubItems.Add(emp.LastName);
                item.SubItems.Add(emp.PhoneNumber);
                item.SubItems.Add(emp.Email);
                listViewEmployee.Items.Add(item);
            }
            

        }

        private void tabPageEmployee_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateList();
            listViewEmployee.FullRowSelect = true;
        }

        private void listViewEmployee_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listViewEmployee.SelectedItems[0];
            textBoxId.Text = item.SubItems[0].Text;
            textBoxFn.Text = item.SubItems[1].Text;
            textBoxLn.Text = item.SubItems[2].Text;
            textBoxPnumber.Text = item.SubItems[3].Text;
            textBoxEmail.Text = item.SubItems[4].Text;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBoxId.Text.Trim());
            var updetedEmp = dbEntities.Employees.Find(id);
            if (updetedEmp != null)
            {
                updetedEmp.FirstName = textBoxFn.Text;
                updetedEmp.LastName = textBoxLn.Text;
                updetedEmp.Email = textBoxEmail.Text;
                updetedEmp.PhoneNumber = textBoxPnumber.Text;
                dbEntities.SaveChanges();
                MessageBox.Show("Update successfully");
                PopulateList();
                cleanTextBox();

            }
            else
            {
                MessageBox.Show("Employee with this Id not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int empId = Convert.ToInt32(textBoxId.Text.Trim());
            Employee emp = dbEntities.Employees.Find(empId);
            if (emp != null)
            {
                dbEntities.Employees.Remove(emp);
                dbEntities.SaveChanges();
                PopulateList();
                MessageBox.Show("Employees Deleted successfully");
                cleanTextBox();
            }
            else
            {
                MessageBox.Show("Error", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
                
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            int selectedOption = comboBoxSearch.SelectedIndex;
            switch (selectedOption)
            {
                case 0: //search by employee Id
                    int empId = Convert.ToInt32(textBoxSearch.Text.Trim());
                    Employee emp = dbEntities.Employees.Find(empId);
                    if(emp!= null)
                    {
                        textBoxId.Text = emp.EmployeeId.ToString();
                        textBoxFn.Text = emp.FirstName;
                        textBoxLn.Text = emp.LastName;
                        textBoxPnumber.Text = emp.PhoneNumber;
                        textBoxEmail.Text = emp.Email;

                    }
                    else
                    {
                        MessageBox.Show("Employee not found");
                    }
                    break;
                case 1://search by name
                   var listEmp= dbEntities.Employees.Where(fn => fn.FirstName == textBoxSearch.Text.Trim()).ToList<Employee>();
                    if(listEmp.Count != 0)
                    {
                        foreach (var oneEmp in listEmp)
                        {
                            MessageBox.Show(oneEmp.EmployeeId.ToString() + "," + oneEmp.FirstName.ToString() + "," + oneEmp.PhoneNumber.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("Employee not found");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
