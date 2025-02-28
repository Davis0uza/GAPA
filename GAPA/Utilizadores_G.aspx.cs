﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GAPA
{
    public partial class Utilizadores_G : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["tipo"] == null || !Session["tipo"].Equals("Administrador"))
                Response.Redirect("login.aspx");

            //consulta a bd
            DataTable dados = BaseDados.Instance.devolveDadosUtilizador();
            if (dados == null || dados.Rows.Count == 0) return;
         //   atualizarGv(dados);
            
           gvUsers.RowDeleting += new GridViewDeleteEventHandler(this.gvUsers_RowDeleting);
            //gvUsers.RowEditing += new GridViewEditEventHandler(this.gvUsers_RowEdditing);
            //  gvUsers.Sorting += new GridViewSortEventHandler(this.gvUsers_Sorting);
            gvUsers.RowCommand += new GridViewCommandEventHandler(this.gvUsers_RowCommand);

        }

        private void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
        {
            //Retrieve the table from the session object.
            DataTable dt = Session["TaskTable"] as DataTable;

            if (dt != null)
            {

                //Sort the data.
                gvUsers.Columns.Clear();
                gvUsers.AutoGenerateDeleteButton = true;
                gvUsers.AutoGenerateEditButton = true;
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvUsers.DataSource = Session["TaskTable"];

               
                //coluna ID
                BoundField bfID = new BoundField();
                bfID.DataField = "id";
                bfID.HeaderText = "ID";
                bfID.SortExpression = "ID";
                gvUsers.Columns.Add(bfID);
                //coluna Username
                BoundField bfusername = new BoundField();
                bfusername.DataField = "Username";
                bfusername.HeaderText = "Username";
                bfusername.SortExpression = "Username";
                gvUsers.Columns.Add(bfusername);
                //coluna Nome
                BoundField bfNome = new BoundField();
                bfNome.DataField = "Nome";
                bfNome.HeaderText = "Nome";
                bfNome.SortExpression = "Nome";
                gvUsers.Columns.Add(bfNome);
                //coluna Email
                BoundField bfEmail = new BoundField();
                bfEmail.DataField = "Email";
                bfEmail.HeaderText = "Email";
                bfEmail.SortExpression = "Email";
                gvUsers.Columns.Add(bfEmail);
                //coluna Tipo
                BoundField bfTipo = new BoundField();
                bfTipo.DataField = "tipo";
                bfTipo.HeaderText = "Tipo";
                bfTipo.SortExpression = "Tipo";
                gvUsers.Columns.Add(bfTipo);
                //coluna Ativo(estado)
                BoundField bfAtivo = new BoundField();
                bfAtivo.DataField = "ativo";
                bfAtivo.HeaderText = "Ativo";
                bfAtivo.SortExpression = "Ativo";
                gvUsers.Columns.Add(bfAtivo);
                //coluna Perfil
                BoundField bfPerfil = new BoundField();
                bfPerfil.DataField = "perfil";
                bfPerfil.HeaderText = "Perfil";
                bfPerfil.SortExpression = "Perfil";
                gvUsers.Columns.Add(bfPerfil);

                gvUsers.DataBind();
               
            }
        }

        private void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int linha = int.Parse(e.CommandArgument as string);
               
               
                if (e.CommandName == "Editar")
                {

                    /* ScriptManager.RegisterStartupScript(this, typeof(string), "confirm",
                     "myTestFunction();", true);*/


                    string id = gvUsers.Rows[linha].Cells[2].Text.ToString();
                    Response.Redirect("Utilizadores_editar.aspx?id=" + id);


                }
            }
            catch { }
        }

  

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        private void gvUsers_RowEdditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                int linha = int.Parse(e.NewEditIndex.ToString());

                string id = gvUsers.Rows[linha].Cells[1].Text.ToString();
                Response.Redirect("Utilizadores_editar.aspx?id=" + id);
            }
            catch { }
        }

        private void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int linha = int.Parse(e.RowIndex.ToString());
                int id = int.Parse(gvUsers.Rows[linha].Cells[2].Text.ToString());
                BaseDados.Instance.deleteUtilizador(id);
                DataTable dados = BaseDados.Instance.devolveDadosUtilizador();
                if (dados == null || dados.Rows.Count == 0) return;
                pesquisa();
                //   atualizarGv(dados);
            }
            catch { }
        }
        

        private void atualizarGv(DataTable dados)
        {
            //limpar grid
          /*  gvUsers.Columns.Clear();
       
            DataColumn dcEditar = new DataColumn();
            dcEditar.ColumnName = "Editar";
            dcEditar.DataType = Type.GetType("System.String");
            dados.Columns.Add(dcEditar);

            gvUsers.AutoGenerateDeleteButton = true;
            gvUsers.AutoGenerateEditButton = true;
            

            //configurar os campos da grid
            gvUsers.DataSource = dados;
            gvUsers.AutoGenerateColumns = false;
            
            //coluna ID
            BoundField bfID = new BoundField();
            bfID.DataField = "id";
            bfID.HeaderText = "ID";
            bfID.SortExpression = "ID";
            gvUsers.Columns.Add(bfID);
            //coluna Username
            BoundField bfusername = new BoundField();
            bfusername.DataField = "Username";
            bfusername.HeaderText = "Username";
            bfusername.SortExpression = "Username";
            gvUsers.Columns.Add(bfusername);
            //coluna Nome
            BoundField bfNome = new BoundField();
            bfNome.DataField = "Nome";
            bfNome.HeaderText = "Nome";
            bfNome.SortExpression = "Nome";
            gvUsers.Columns.Add(bfNome);
            //coluna Email
            BoundField bfEmail = new BoundField();
            bfEmail.DataField = "Email";
            bfEmail.HeaderText = "Email";
            bfEmail.SortExpression = "Email";
            gvUsers.Columns.Add(bfEmail);
            //coluna Tipo
            BoundField bfTipo = new BoundField();
            bfTipo.DataField = "tipo";
            bfTipo.HeaderText = "Tipo";
            bfTipo.SortExpression = "Tipo";
            gvUsers.Columns.Add(bfTipo);
            //coluna Ativo(estado)
            BoundField bfAtivo = new BoundField();
            bfAtivo.DataField = "ativo";
            bfAtivo.HeaderText = "Ativo";
            bfAtivo.SortExpression = "Ativo";
            gvUsers.Columns.Add(bfAtivo);
            //coluna Perfil
            BoundField bfPerfil = new BoundField();
            bfPerfil.DataField = "perfil";
            bfPerfil.HeaderText = "Perfil";
            bfPerfil.SortExpression = "Perfil";
            gvUsers.Columns.Add(bfPerfil);

            Session["TaskTable"] = dados;
            //refresh
            gvUsers.DataBind();*/ 
           
        }

        protected void btPesquisa_Click(object sender, EventArgs e)
        {
            string text = tbPesquisa.Text;
            DataTable dados = BaseDados.Instance.pesquisarUtilizador(text);
            gvUsers.DataSourceID = "";
            gvUsers.DataSource = dados;
            gvUsers.DataBind();

        }
        protected void pesquisa()
        {
            
                string text = tbPesquisa.Text;
            DataTable dados = BaseDados.Instance.pesquisarUtilizador(text);
            if (dados.Rows.Count == 0 || dados == null)
            {
                dados = BaseDados.Instance.devolveDadosUtilizador();
                tbPesquisa.Text = "";
            }
            gvUsers.DataSourceID = "";
                gvUsers.DataSource = dados;
                gvUsers.DataBind();
            }

        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvUsers.PageIndex = e.NewPageIndex;
            DataTable dados = BaseDados.Instance.devolveDadosUtilizador();
            atualizarGv(dados);
        }
    }
}