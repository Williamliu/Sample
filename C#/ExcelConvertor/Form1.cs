using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace ExcelConvertor
{
    public partial class Form1 : Form
    {
        public OleDbConnection ocon;
        public OleDbDataAdapter oadp_input;
        public OleDbDataAdapter oadp_output;
        public DataTable iTable;
        public DataTable oTable;
        public DataSet ds;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btn_file_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDiag.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                openFileDiag.FileName = "";
                openFileDiag.Filter = "All Files|*.*|Excel xls|*.xls|Excel xlsx|*.xlsx";
                openFileDiag.CheckFileExists = true;
                openFileDiag.CheckPathExists = true;
                openFileDiag.DefaultExt = ".xls";

                if (this.openFileDiag.ShowDialog() == DialogResult.OK)
                {
                    this.resetControl();
                    this.txt_file.Text = openFileDiag.FileName;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format("Error: {0}\nMessage: {1}", err.Message, err.StackTrace));
            }

        }

        private void btn_load_Click(object sender, EventArgs e)
        {
            try
            {
                this.resetControl();

                ocon = new OleDbConnection();
                string fileExtension = Path.GetExtension(this.openFileDiag.FileName);
                if (fileExtension == ".xls")
                    ocon.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.openFileDiag.FileName + ";" + "Extended Properties='Excel 8.0;HDR=Yes;'";
                if (fileExtension == ".xlsx")
                    ocon.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + this.openFileDiag.FileName + ";" + "Extended Properties='Excel 12.0 Xml;HDR=YES;'";


                ds = new DataSet();

                oadp_input = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}$]", this.txt_input.Text), ocon);
                oadp_input.TableMappings.Add("Table", "InputTable");
                oadp_input.Fill(ds);
                dgv_input.DataSource = ds.Tables["InputTable"];
                iTable = ds.Tables["InputTable"];

                    
                foreach( DataColumn dc in iTable.Columns) {
                    this.ckl_input.Items.Add(dc.ColumnName);
                    this.ckl_valinput.Items.Add(dc.ColumnName);
                }
                //ocon.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format("Error: {0}\nMessage: {1}", err.Message, err.StackTrace));
            }
        }

        private void btn_process_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, string> valueObj = new Dictionary<string, string>();
                DataRow vdr = iTable.Rows[0];
                foreach (string colName in this.ckl_valinput.CheckedItems)
                {
                    valueObj.Add(colName, vdr[colName].ToString());
                }

                oTable = new DataTable("Output");
                foreach (string colName in this.ckl_input.CheckedItems)
                {
                    DataColumn dc = new DataColumn(colName);
                    oTable.Columns.Add(dc);
                }
                DataColumn dcode = new DataColumn("Code");
                oTable.Columns.Add(dcode);

                DataColumn dval = new DataColumn("Value");
                oTable.Columns.Add(dval);

                for (int i = 1; i < iTable.Rows.Count; i++)
                {
                    foreach (string valName in this.ckl_valinput.CheckedItems)
                    {
                        if (!string.IsNullOrEmpty(iTable.Rows[i][valName].ToString()))
                        {
                            DataRow ndr = oTable.NewRow();
                            foreach (string colName in this.ckl_input.CheckedItems)
                            {
                                ndr[colName] = iTable.Rows[i][colName];
                            }
                            ndr["Code"] = valueObj[valName];
                            ndr["Value"] = iTable.Rows[i][valName];
                            oTable.Rows.Add(ndr);
                        }
                    }
                }
                oTable.AcceptChanges();

                this.dgv_output.DataSource = oTable;
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format("Error: {0}\nMessage: {1}", err.Message, err.StackTrace));
            }
        }

        private void resetControl()
        {
            this.iTable = null;
            this.oTable = null;
            dgv_input.DataSource = null;
            this.ckl_input.Items.Clear();
            this.ckl_valinput.Items.Clear();
        }

        private void btn_output_Click(object sender, EventArgs e)
        {
            try
            {
                string np = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Output");
                if (!Directory.Exists(np)) Directory.CreateDirectory(np);
                string nf = Path.Combine(np, string.Format("{0}.csv", Path.GetFileNameWithoutExtension(openFileDiag.FileName)));
                
                StreamWriter sw = new StreamWriter(nf);

                var col_str = "";
                foreach (DataColumn dc in oTable.Columns)
                {
                    col_str += (col_str == "" ? "" : ",") + dc.ColumnName;
                }
                sw.WriteLine(col_str);
                
                foreach(DataRow dr in oTable.Rows)
                {
                    var row_str = "";
                    foreach (DataColumn dc in oTable.Columns)
                    {
                        row_str += (row_str == "" ? "" : ",") + dr[dc.ColumnName].ToString();
                    }
                    sw.WriteLine(row_str);
                }
                sw.Flush();
                sw.Close();

                MessageBox.Show("Done Success.\nResult output to :\n" + nf);
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format("Error: {0}\nMessage: {1}", err.Message, err.StackTrace));
            }

        }
    }
}
