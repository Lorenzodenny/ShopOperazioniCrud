using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace SHOP
{
    public partial class MasterEdit : System.Web.UI.Page
    {
        private string ProductId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Recuperare l'Id del prodotto dalla query string dalla pagina Index.aspx.cs  <a href=""Details.aspx?product={dataReader["Id"]} 
                if (Request.QueryString["product"] == null)
                {
                    Response.Redirect("Index.aspx");
                }
                else
                {
                    ProductId = Request.QueryString["product"].ToString();
                   

                    // Popolare la DropDown con tutte le categorie
                    try
                    {
                        Db.conn.Open();
                        SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Categories", Db.conn);
                        DataTable tbCategories = new DataTable();
                        dataAdapter.Fill(tbCategories);

                        foreach (DataRow row in tbCategories.Rows)
                        {
                            ListItem listItem = new ListItem();
                            listItem.Text = row["Name"].ToString();
                            listItem.Value = row["Id"].ToString();
                            DrpCategories.Items.Add(listItem);
                        }

                        // Popolare i campi del form con i dati relativi al prodotto in base all'Id
                        // nella query string
                        SqlCommand cmd = new SqlCommand($"SELECT * FROM Products WHERE Id={ProductId}", Db.conn);
                        SqlDataReader dataReader = cmd.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            dataReader.Read();
                            TxtName.Text = dataReader["Name"].ToString();
                            TxtImage.Text = dataReader["Image"].ToString();
                            TxtDescription.Text = dataReader["Description"].ToString();
                            DrpCategories.SelectedValue = dataReader["CategoryId"].ToString();
                        }
                        dataReader.Close(); // Chiudi il DataReader dopo averlo utilizzato
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.ToString());
                    }
                    finally
                    {
                        Db.conn.Close();
                    }
                }
            }
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            ProductId = Request.QueryString["product"].ToString();

            if (ProductId == null)
            {
                // Se ProductId è ancora null, potrebbe esserci un problema
                Response.Write("ProductId non è stato inizializzato correttamente.");
                return;
            }

            try
            {

                // Apro la connessione che ho creato in modo centralizzato nella classe Db
                Db.conn.Open();

                // Creo la query per l'Update
                string updateQuery = $@"UPDATE Products
                                       SET Name = @Name,
                                           Description = @Description,
                                           Image = @Image,
                                           CategoryId = @CategoryId
                                       WHERE Id = {ProductId}";


                // Inserisco la query per l'Update e ne vado a modificare i valori in modo dinamico
                // prendendole dagli input dell'Html (TexBox con Id Txt.Qualcosa ) 
                using (SqlCommand cmd = new SqlCommand(updateQuery, Db.conn))
                {
                    cmd.Parameters.AddWithValue("@Name", TxtName.Text);
                    cmd.Parameters.AddWithValue("@Description", TxtDescription.Text);
                    cmd.Parameters.AddWithValue("@Image", TxtImage.Text);
                    cmd.Parameters.AddWithValue("@CategoryId", DrpCategories.SelectedValue);
                  
                    // Controllo se all'interno di rowAffected c'è qualche dato
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected != 0)
                    {
                        Response.Write("Aggiornamento Effettuato");
                    }
                    else
                    {
                        Response.Write("Problemi durante l'aggiornamento");
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
            finally
            {
                Db.conn.Close();
            }
        }
    }
}

