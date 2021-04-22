﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gewinnspiel.Classes;

namespace Gewinnspiel.Forms
{
    public partial class frmTeilnehmerListe : Form
    {
        public frmTeilnehmerListe()
        {
            InitializeComponent();
        }

        #region Variabeln
        ListViewItem lvItem;
        #endregion

        private void frmTeilnehmerListe_Load(object sender, EventArgs e)
        {
            listView1.FullRowSelect = true;
            einlesenTeilnehmer();
        }

        #region Methoden
        internal void einlesenTeilnehmer()
        {
            foreach(Teilnehmer t in frmLogin.frmLog.TeilnehmerListe)
            {
                lvItem = new ListViewItem(t.TeilnehmerID.ToString());
                lvItem.SubItems.Add(t.Vorname);
                lvItem.SubItems.Add(t.Nachname);
                lvItem.SubItems.Add(t.GebDatum.ToShortDateString());
                lvItem.SubItems.Add(t.Geschlecht.ToString());
                lvItem.SubItems.Add(t.Email);
                lvItem.SubItems.Add(t.Deaktiviert.ToString());
                lvItem.SubItems.Add(t.Admin.ToString());
                lvItem.SubItems.Add(t.AnzahlSpiele.ToString());
                listView1.Items.Add(lvItem);
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        }
        #endregion

        private void teilnehmerHinzufügenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTeilnehmerAdd frmTeilAdd = new frmTeilnehmerAdd();
            frmTeilAdd.Text = "Teilnehmer hinzufügen";
            frmTeilAdd.txtTeilnehmerID.Text = Teilnehmer.Autonum.ToString();
            frmTeilAdd.ShowDialog();
            einlesenTeilnehmer();
        }

        private void teilnehmerBearbeitenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count==0)
            {
                MessageBox.Show("Bitte wählen Sie einen Teilnehmer aus!");
                return;
            }
            lvItem = listView1.SelectedItems[0];
            frmTeilnehmerAdd frmTeilAdd = new frmTeilnehmerAdd();
            frmTeilAdd.Text = "Teilnehmer bearbeiten";
            frmTeilAdd.txtTeilnehmerID.Text = lvItem.SubItems[0].Text;
            frmTeilAdd.txtNachname.Text = lvItem.SubItems[1].Text;
            frmTeilAdd.txtVorname.Text = lvItem.SubItems[2].Text;
            frmTeilAdd.txtEmail.Text = lvItem.SubItems[5].Text;
            frmTeilAdd.cmGeschlecht.Text = lvItem.SubItems[4].Text;
            frmTeilAdd.dtpGeDatum.Value = Convert.ToDateTime(lvItem.SubItems[3].Text);
            frmTeilAdd.cbAdmin.Checked = Convert.ToBoolean(lvItem.SubItems[6].Text);
            frmTeilAdd.cbDeaktiviert.Checked = Convert.ToBoolean(lvItem.SubItems[7].Text);
            frmTeilAdd.ShowDialog();
            einlesenTeilnehmer();
        }
    }
}
