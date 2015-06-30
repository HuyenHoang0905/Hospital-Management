using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class DataConnect
    {
        private string source;
        private SqlDataAdapter da;
        public SqlConnection conn;
        private DataTable dt;
        private string erro;

        public string Erro
        {
            get { return erro; }
            set { erro = value; }
        }

        public DataConnect()
        {
            source = @"Data Source=HUYEN\HOANGHUYEN;Initial Catalog=DBBenhVienYHocCoTruyen;Integrated Security=True";
            conn = new SqlConnection(source);
        }
        public SqlConnection con()
        {
            return conn;
        }
        //Hàm mở kết nối
        public void openConnect()
        {
            if ((conn == null) || (conn.State == ConnectionState.Closed))
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    Erro = ex.Message;
                    //MessageBox.Show(ex.Message);
                }

            }
        }
        
        public void closeConnect()
        {
            if ((conn != null) || (conn.State == ConnectionState.Open))
            {
                conn.Close();
                //conn.Dispose();
            }
        }
        
        public DataTable getData(string sql)
        {
            da = new SqlDataAdapter(sql, conn);
            dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        
        public bool ExcuteQuery(string sql)
        {
            int numberRecords = 0;
            try
            {
                openConnect();
                SqlCommand cmd = new SqlCommand(sql, conn);
                numberRecords = cmd.ExecuteNonQuery();
                closeConnect();

            }
            catch (Exception e)
            {
                Erro = e.Message;
                //MessageBox.Show(Erro, "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (numberRecords > 0)
            {
                return true;
            }
            return false;
        }
        
        public string getLastID(string nameTable, string nameSelectColumn)
        {
            string sql = "SELECT TOP 1" + nameSelectColumn + " FROM " + nameTable + " ORDER BY " + nameSelectColumn + " DESC";
            
            getData(sql);
            return dt.Rows[0][nameSelectColumn].ToString();
        }
        
        public int demGiaTri(string sql)
        {
            openConnect();
            SqlCommand cmd = new SqlCommand(sql, conn);
            return (Int32)(cmd.ExecuteScalar());
        }  
    }
}
