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
    public partial class frmAdmin : Form
    {
        public frmAdmin()
        {
            InitializeComponent();
        }

        #region Variablen
        ListViewItem lvItem;
        ListViewItem lvItemT;
        internal Spiel spielaktuell;
        #endregion

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            lvGewinnspiele.FullRowSelect = true;
            lvTeilnehmer.FullRowSelect = true;
            rbAktive.Checked = true;
            einlesenGewinnspiele();
        }

        #region Methoden

        private void teilnehmerListe()
        {
            frmTeilnehmerListe frmTeilList = new frmTeilnehmerListe();
            frmTeilList.Text = "Liste aller registrierten Teilnehmer";
            frmTeilList.ShowDialog();
        }

        internal void einlesenTeilnehmer()
        {
            lvTeilnehmer.Items.Clear();
            foreach (Teilnehmer t in spielaktuell.ListeTeilnehmer)
            {
                lvItemT = new ListViewItem(t.TeilnehmerID.ToString());
                lvItemT.SubItems.Add(t.Nachname);
                lvItemT.SubItems.Add(t.Vorname);
                lvItemT.SubItems.Add(t.Email);
                lvItemT.SubItems.Add(t.Geschlecht.ToString());
                lvItemT.SubItems.Add(t.GebDatum.ToShortDateString());
                lvItemT.SubItems.Add(t.Admin.ToString());
                lvItemT.SubItems.Add(t.Inaktiv.ToString());
                lvTeilnehmer.Items.Add(lvItemT);
            }
        }

        private void hinzufuegenTeilnehmer() //einem Gewinnspiel
        {
            if (rbAbgeschlossen.Checked)
            {
                MessageBox.Show("Einem abgeschlossenen Gewinnspiel können keine Teilnehmer mehr hinzugefügt werden!");
                return;
            }
            if (lvGewinnspiele.SelectedItems.Count==0)
            {
                MessageBox.Show("Bitte wählen Sie ein Gewinnspiel aus!");
                return;
            }
            //Gewinnspiel ist bereits durch lvGewinnspiel_Click ausgewählt und
            // in spielAktuell gespeichert
            //lvItem = lvGewinnspiele.SelectedItems[0];
            teilnehmerListe();
            frmLogin.frmLog.serialisierenSpiel();

        }

        private void einlesenGewinnspiele()
        {
            lvGewinnspiele.Items.Clear();
            for (int i=0; i<frmLogin.frmLog.gewinnspielListe.Count; i++)
            {
                if(rbAktive.Checked)
                {
                    if(!frmLogin.frmLog.gewinnspielListe[i].Ausgelost)
                    {
                        lvItem = new ListViewItem();
                        lvItem.ImageIndex = i;
                        lvItem.SubItems.Add(frmLogin.frmLog.gewinnspielListe[i].SpielID.ToString());
                        lvItem.SubItems.Add(frmLogin.frmLog.gewinnspielListe[i].Bezeichnung);
                        lvItem.SubItems.Add(frmLogin.frmLog.gewinnspielListe[i].Gewinn);
                        lvItem.SubItems.Add(frmLogin.frmLog.gewinnspielListe[i].Wert.ToString());
                        lvItem.SubItems.Add(frmLogin.frmLog.gewinnspielListe[i].Ausgelost.ToString());
                        lvItem.SubItems.Add(frmLogin.frmLog.gewinnspielListe[i].Bildpfad);
                        lvGewinnspiele.Items.Add(lvItem);
                    }
                }
                if(rbAbgeschlossen.Checked)
                {
                    if (frmLogin.frmLog.gewinnspielListe[i].Ausgelost)
                    {
                        lvItem = new ListViewItem();
                        lvItem.ImageIndex = i;
                        lvItem.SubItems.Add(frmLogin.frmLog.gewinnspielListe[i].SpielID.ToString());
                        lvItem.SubItems.Add(frmLogin.frmLog.gewinnspielListe[i].Bezeichnung);
                        lvItem.SubItems.Add(frmLogin.frmLog.gewinnspielListe[i].Gewinn);
                        lvItem.SubItems.Add(frmLogin.frmLog.gewinnspielListe[i].Wert.ToString());
                        lvItem.SubItems.Add(frmLogin.frmLog.gewinnspielListe[i].Ausgelost.ToString());
                        lvItem.SubItems.Add(frmLogin.frmLog.gewinnspielListe[i].Bildpfad);
                        lvGewinnspiele.Items.Add(lvItem);
                    }
                }
            }
            lvGewinnspiele.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            lvGewinnspiele.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        
        private void hinzufuegenGewinnspiel()
        {
            frmGewinnspiel frmGew = new frmGewinnspiel();
            frmGew.Text = "Gewinnspiel hinzufügen";
            frmGew.txtGewinnspielID.Text = Spiel.Autonum.ToString();
            frmGew.ShowDialog();
            einlesenGewinnspiele();
        }

        private void bearbeitenGewinnspiel()
        {
            if(lvGewinnspiele.SelectedItems.Count==0)
            {
                MessageBox.Show("Bitte wählen Sie ein Gewinnspiel aus!");
                return;
            }
            frmGewinnspiel frmGew = new frmGewinnspiel();
            frmGew.Text = "Gewinnspiel bearbeiten";
            lvItem = lvGewinnspiele.SelectedItems[0];
            frmGew.txtGewinnspielID.Text = lvItem.SubItems[1].Text;
            frmGew.txtBezeichnung.Text = lvItem.SubItems[2].Text;
            frmGew.txtGewinn.Text = lvItem.SubItems[3].Text;
            frmGew.txtWert.Text = lvItem.SubItems[4].Text;
            frmGew.cbAusgelost.Checked = Convert.ToBoolean(lvItem.SubItems[5].Text);

            try
            {
                if (!lvItem.SubItems[6].Text.Equals(""))
                {
                    frmGew.pictureBox1.Image = Image.FromFile(lvItem.SubItems[6].Text);
                    frmGew.txtBildpfad.Text = lvItem.SubItems[6].Text;
                    frmGew.bildpfad = lvItem.SubItems[6].Text;
                }
                else
                {
                    frmGew.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\img\\default.jpg");
                    frmGew.bildpfad = Application.StartupPath + "\\img\\default.jpg";
                }
            }
            catch
            {
                MessageBox.Show("Das Bild konnte nicht gefunden werden!");
                frmGew.pictureBox1.Image = Image.FromFile(Application.StartupPath + "\\img\\default.jpg");
                frmGew.bildpfad = Application.StartupPath + "\\img\\default.jpg";
            }
            frmGew.ShowDialog();
            einlesenGewinnspiele();

        }
        #endregion

        #region Radio Buttons einlesen
        private void rbAktive_CheckedChanged(object sender, EventArgs e)
        {
            einlesenGewinnspiele();
        }

        private void rbAbgeschlossen_CheckedChanged(object sender, EventArgs e)
        {
            einlesenGewinnspiele();
        }
        #endregion

        #region Gewinnspiel hinzufügen/bearbeiten
        private void hinzufügenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hinzufuegenGewinnspiel();
        }

        private void bearbeitenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bearbeitenGewinnspiel();
        }

        private void hinzufügenToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            hinzufuegenGewinnspiel();
        }

        private void bearbeitenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            bearbeitenGewinnspiel();
        }
        #endregion

        private void teilnehmerlisteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            teilnehmerListe();
        }

        private void bearebeitenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            teilnehmerListe();
        }

        private void deaktivierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            teilnehmerListe();
        }

        private void hinzufügenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void lvGewinnspiele_Click(object sender, EventArgs e)
        {
            lvTeilnehmer.Items.Clear();
            if (lvGewinnspiele.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bitte wählen Sie ein Gewinnspiel aus!");
                return;
            }
            lvItem = lvGewinnspiele.SelectedItems[0];
            foreach (Spiel s in frmLogin.frmLog.gewinnspielListe)
            {
                if (s.SpielID == Convert.ToInt32(lvItem.SubItems[1].Text))
                {
                    spielaktuell = s;
                    foreach (Teilnehmer t in s.ListeTeilnehmer)
                    {
                        lvItemT = new ListViewItem(t.TeilnehmerID.ToString());
                        lvItemT.SubItems.Add(t.Nachname);
                        lvItemT.SubItems.Add(t.Vorname);
                        lvItemT.SubItems.Add(t.Email);
                        lvItemT.SubItems.Add(t.Geschlecht.ToString());
                        lvItemT.SubItems.Add(t.GebDatum.ToShortDateString());
                        lvItemT.SubItems.Add(t.Admin.ToString());
                        lvItemT.SubItems.Add(t.inaktiv.ToString());
                        lvTeilnehmer.Items.Add(lvItemT);
                    }
                    break;
                }
            }
        }

        private void einemGewinnpielHinzufügenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hinzufuegenTeilnehmer();
        }

        private void teilnehmerHinzufügenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hinzufuegenTeilnehmer();
        }

        private void alleTeilnehmerEntfernenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < spielaktuell.ListeTeilnehmer.Count; i++)
            {
                spielaktuell.ListeTeilnehmer[i].AnzahlSpiele--;
            }
            spielaktuell.ListeTeilnehmer.Clear();
            lvTeilnehmer.Items.Clear();
            frmLogin.frmLog.serialisierenSpiel();
        }
    }
}
