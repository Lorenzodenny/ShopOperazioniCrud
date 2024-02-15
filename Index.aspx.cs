using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SHOP
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            // Connessione al DataBase
            string connectionString = ConfigurationManager.ConnectionStrings["DbShopConnectionString"].ToString();
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();

                //Recuperare i dati dal DataBase
                SqlCommand cmd = new SqlCommand("SELECT * FROM Products", conn);
                SqlDataReader dataReader = cmd.ExecuteReader();

                // Inizializziamo la variablie htmlContent
                string htmlContent = "";

                if(dataReader.HasRows) // se dataReader.HasRows => se dataReader ha delle righe, ovvero se dataReader esiste, è come il && in react
                {
                    // Cicliamo sulle righe ottenute dal db e aggiungiamo l'html alle cards
                    // è simile al ciclo Map di react, cicla ogni oggetto salvato nel DataBase
                    // di ogni oggetto stampa foto-name-descrizione e alla fine per ogni oggetto crea una pagina details
                    // e riesce a crearla creando una stringa Details.aspx?product= qui inserira l'id di ogni
                    // oggetto salvato nel db, che avrà Id incrementale in automatico (Identity : Yes)
                    while (dataReader.Read())
                    {
                        htmlContent += $@"<div class=""col"">
                            <div class=""card"">
                                <div class=""card h-100"">
                                    <img class=""card-img-top"" src=""{dataReader["Image"]}"" alt=""{dataReader["Name"]}"">
                                        <div class=""card-body"">
                                        <h5 class=""card-title"">{dataReader["Name"]}</h5>
                                         <p class=""card-text"">{dataReader["Description"]}</p>
                                         <a href=""Details.aspx?product={dataReader["Id"]}"" class=""btn btn-primary"">Dettagli</a>
                                        </div>
                                    </div>
                                </div>";
                    }
                }



                //inseriamo in RowCards il contenutore di htmlContent
                RowCards.InnerHtml = htmlContent;

                //RowCards.InnerHtml

            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                if(conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                
            }



        }
    }
}