using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SHOP
{
    public partial class Details : System.Web.UI.Page
    {
        // Inizializzo la stringa ( variabile di tipo string ) in modo globale per l'intera pagina
        private string ProductId;
        protected void Page_Load(object sender, EventArgs e)
        {

            Db.conn.Open();
            // Recuperare l'id dal prodotto della query string Details.aspx?prodotto= Id <-- anche se la stringa sarà Details.aspx?product=10 (esempio Id=10)
            // Salviamo dentro ProductId l'id, in questo caso salviamo l'ipotetico 10, quindi facendo cosi possiamo renderlo dinamico
            if (Request.QueryString["product"] == null)
            { 
                Response.Redirect("Index.aspx");
            }

            ProductId = Request.QueryString["product"].ToString();  // settato product nella pagina index (  <a href=""Details.aspx?product={dataReader["Id"]} ) qui creo il QueryString ?product= che poi recuperero il valore dell'Id in questo caso e lo inserisco all'interno della variabile ProductId = Request.QueryString["product"].ToString();

            // Ancora link - creo il collegamento di tipo link, definendo il percorso e dandogli come Id l'Id preso in modo dinamico grazie al Request.QueryString
            BtnAnchorEdit.HRef = "MasterEdit.aspx?product=" + ProductId;

            Db.conn.Close();
            try
            {
                // Apro la connessione
                Db.conn.Open();

                // Recuperò i dati con il Select, prendendoli tutti grazie alla Join. Rinomino Categories.Name, perche avendo Name sia in Categories che in Product
                // cosi facendo posso decidere a quale dei due mi riferisco

                SqlCommand cmd = new SqlCommand($@"
                    SELECT Products.*, Categories.Name As CategoryName  
                    FROM Products 
                    JOIN Categories ON Products.CategoryId = Categories.Id 
                    WHERE Products.Id={ProductId} ", Db.conn);
                
                SqlDataReader dataReader = cmd.ExecuteReader();

                // Controllo se i dati dentro dataReader esistono grazie al .HasRows che dicendomi se esistono " righe " mi dà un booleano
                // Dicendomi "esiste almeno una riga di dati dentro il DataBase: TRUE or FALSE ", e se esistono li prendo e ci popolo
                // le Label (LblCategory), gli H2/H3 (TtlName), i <p> (ParContent) e le img (ImgProduct) 

                if (dataReader.HasRows)
                {
                    dataReader.Read();
                    TtlName.InnerText = dataReader["Name"].ToString();
                    LblCategory.InnerText = dataReader["CategoryName"].ToString(); 
                    ImgProduct.Src = dataReader["Image"].ToString();
                    ParContent.InnerHtml = dataReader["Description"].ToString();

                }
                else
                {
                    Response.Redirect("NotFound.aspx");
                    //Server.Transfer("NotFound.asxp"); 
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

        protected void BtnEdit_Click(object sender, EventArgs e)
        {
            // Ridireziona nella pagina MasterEdit, creando la URL dandogli come indirizzo MasterEdit.aspx?product= e passandagli l'id in modo dinamico
            // recuperato in alto grazie a ProductId = Request.QueryString["product"].ToString();

            Response.Redirect("MasterEdit.aspx?product=" + ProductId);
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            // Eliminare la riga dal DataBase
            // Eseguo il comando Sql per fare il delete, a cui passerò l'id che voglio eliminare in modo dinamico 

            try
            {
                Db.conn.Open();
                SqlCommand cmd = new SqlCommand($@" DELETE FROM Products WHERE Products.Id={ProductId}", Db.conn);
                int affectedROws = cmd.ExecuteNonQuery();

                if (affectedROws != 0)
                {
                    Response.Redirect("Index.aspx");
                }
                else
                {
                    Response.Write("Eliminazione non riuscita");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

        }
            // Riderizionare index
        }
    }
