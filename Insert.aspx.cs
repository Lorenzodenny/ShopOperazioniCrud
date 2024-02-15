using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SHOP
{
    public partial class Insert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Faccio il count per sapere quanti Prodotti ci sono nel DataBase 
            // Faccio una Select ( un richiesta Get del CRUD ) 
            LblProdottiTotali.Text = "";

            Db.conn.Open();
            SqlCommand prodottiCount = new SqlCommand("SELECT COUNT(*) FROM Products", Db.conn);
            string totaleProdotti = prodottiCount.ExecuteScalar().ToString();
            LblProdottiTotali.Text = $"I Prodotti Totali Nel DataBase sono: {totaleProdotti} ";
            Db.conn.Close();
        }

        protected void BtnCrea_Click(object sender, EventArgs e)
        {
           
          
                try
                {

                    // Recuper il valore degli input dell'utente e faccio un INSERT ( Richiesta Push All'interno del DataBase )
                    Db.conn.Open();
                    SqlCommand saveProduct = new SqlCommand(@"INSERT INTO Products (Name, Description, Image, CategoryId)
                                                             VALUES 
                                                            (@Name, @Description, @Image, @CategoryId)", Db.conn);
                                                    saveProduct.Parameters.AddWithValue("@Name", TxtName.Text);
                                                    saveProduct.Parameters.AddWithValue("@Description", TxtDescription.Text);
                                                    saveProduct.Parameters.AddWithValue("@Image", TxtImage.Text);
                    // Converto il testo di TxtCategoryId in un valore int
                    if (int.TryParse(TxtCategoryId.Text, out int categoryId))
                    {
                        saveProduct.Parameters.AddWithValue("@CategoryId", categoryId);
                    }
                    else
                    {
                        // Se la conversione fallisce, gestisci l'errore o fornisci un valore predefinito
                        // In questo esempio, si potrebbe restituire un messaggio di errore
                        Response.Write("CategoryId non è un numero valido.");
                        return;
                    }

                    int affectedRows = saveProduct.ExecuteNonQuery();

                    if (affectedRows != 0)
                    {
                        Response.Write("Dati Inseriti Con Successo");
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("Qualcosa non va: " + ex.Message);
                }
                finally
                {
                    Db.conn.Close();
                }
            }

        
    }
}