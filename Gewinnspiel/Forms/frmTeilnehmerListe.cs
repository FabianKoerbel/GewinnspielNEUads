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
            foreach (Teilnehmer t in frmLogin.frmLog.TeilnehmerListe)
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

        private void hinzufuegenTeilnehmerGewinnspiel()
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bitte wählen Sie einen Teilnehmer aus!");
                return;
            }
            lvItem = listView1.SelectedItems[0];
            if (lvItem.SubItems[7].Text.Equals("True"))
            {
                MessageBox.Show("Dieser Teilnehmer ist deaktiviert und kann daher nicht hinzugefügt werden!");
                return;
            }
            foreach (Teilnehmer t in frmLogin.frmLog.TeilnehmerListe)
            {
                if (t.TeilnehmerID == Convert.ToInt32(lvItem.SubItems[0].Text))
                {
                    t.AnzahlSpiele++;
                    bool gefunden = false; //Teilnehmer in der Gewinnspielliste
                    foreach (Teilnehmer teil in frmLogin.frmLog.frmAd.spielaktuell.ListeTeilnehmer)
                    {
                        if (teil.TeilnehmerID == t.TeilnehmerID)
                            gefunden = true;
                    }
                    if (gefunden)
                    {
                        MessageBox.Show("Dieser Teilnehmer ist bereits Teilnehmer in diesem Gewinnspiel!");
                        break;
                    }
                    else // Teilnehmer noch nicht vorhanden in diesem Gewinnspiel --> hinzufügen
                    {
                        frmLogin.frmLog.frmAd.spielaktuell.ListeTeilnehmer.Add(t);
                        frmLogin.frmLog.frmAd.einlesenTeilnehmer();
                        break;
                    }
                }
            }
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
            if (listView1.SelectedItems.Count == 0)
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

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            hinzufuegenTeilnehmerGewinnspiel();
        }

        private void teilnehmerDeaktivierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bitte wählen Sie einen Teilnehmer aus!");
                return;
            }
            DialogResult dr = MessageBox.Show("Wollen Sie diesen Teilnehmer wirklich de/aktivieren?", "ACHTUNG:", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                lvItem = listView1.SelectedItems[0];
                if (lvItem.SubItems[6].Text.Equals("True"))
                {
                    MessageBox.Show("Admins können nicht deaktiviert werden, sie können max. zu einem Teilnehmer herabgestuft werden!");
                    return;
                }
                foreach (Teilnehmer t in frmLogin.frmLog.TeilnehmerListe)
                {
                    if (t.TeilnehmerID == Convert.ToInt32(lvItem.SubItems[0].Text))
                    {
                        if (t.Inaktiv)
                            t.Inaktiv = false;
                        else
                            t.Inaktiv = true;
                        break;
                    }
                }
                einlesenTeilnehmer();
                frmLogin.frmLog.serialisierenTeiln();
            }
        }

        private void teilnehmerZumGewinnspielHinzufügenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hinzufuegenTeilnehmerGewinnspiel();
        }
    }
}
