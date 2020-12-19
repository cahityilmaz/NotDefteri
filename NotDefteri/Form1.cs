using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BasicNotepad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //------------------------ DEFINE VARIABLE ------------------------------------
        
        string fileName = null;
        string newFileName = "Adsız";
        DialogResult drCikis;
        bool degisiklik;
        bool kayitli;
        int satir, sutun;
        
        //-----------------------------------------------------------------------------
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            degisiklik = true;
            toolStripStatusLabelsayKarakter.Text = richTextBox1.TextLength.ToString();
            if (richTextBox1.TextLength == 0)
            {
                toolStripStatusLabelsayKarakter.Text = "0";
                degisiklik = false;
                toolStripButtonSil.Enabled = false;
            }
            satir = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
            sutun = richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexFromLine(satir);
            toolStripStatusLabelSatir.Text = (satir + 1).ToString();
            toolStripStatusSutun.Text = (sutun + 1).ToString();
            toolStripButton2.Enabled = richTextBox1.CanUndo;
            toolStripButtonYinele.Enabled = richTextBox1.CanRedo;
            toolStripButtonSil.Enabled = true;
            toolStripButtonYapistir.Enabled = Clipboard.GetText().Length > 0;
            toolStripButtonKes.Enabled = richTextBox1.SelectionLength > 0;
            toolStripButtonKopyala.Enabled = richTextBox1.SelectionLength > 0;
        }

        //--------------------------- FORM LOAD ------------------------------------

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = newFileName + " - " + "Note";
            kayitli = false;
            degisiklik = false;
            satir = 1;
            sutun = 1;
        }

        //-------------------------- DOSYA MENÜSÜ --------------------------------------

        //YENI
        private void yeniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((kayitli == true) && (degisiklik == false)) || ((kayitli == false) && (degisiklik == false)))
            {
                richTextBox1.Clear();
                this.Text = newFileName + " - Not Defteri";
            }

            else if ((kayitli == false) && (degisiklik == true))
            {
                drCikis = MessageBox.Show("Dosyada değişiklik yaptınız." + "\n" + "\n" + "Değişiklikleri kaydetmek istiyor musunuz?", "Not Defteri", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (drCikis == DialogResult.Yes)
                {
                    farklıKaydetToolStripMenuItem_Click(sender, e);
                    richTextBox1.Clear();
                    this.Text = newFileName + " - Not Defteri";
                }
            }

            else if ((kayitli == true) && (degisiklik == true))
            {
                drCikis = MessageBox.Show("Dosyada değişiklik yaptınız." + "\n" + "\n" + "Değişiklikleri kaydetmek istiyor musunuz?", "Not Defteri", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (drCikis == DialogResult.Yes)
                {
                    kaydetToolStripMenuItem_Click(sender, e);
                    richTextBox1.Clear();
                    this.Text = newFileName + " - Not Defteri";
                } 
            } 
        }

        //AÇ
        private void açToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog1.SafeFileName;
                StreamReader dosyaAc = new StreamReader(fileName, Encoding.GetEncoding(1254));
                richTextBox1.Text = dosyaAc.ReadToEnd();
                dosyaAc.Close();
                this.Text = (fileName + " - Not Defteri");
                toolStripStatusLabelsayKarakter.Text = richTextBox1.TextLength.ToString();
                kayitli = true;
                degisiklik = false;
            }

            

        }

        //KAYDET
        private void kaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (kayitli)
            {
                StreamWriter kaydet = new StreamWriter(fileName, false, Encoding.GetEncoding(1254));
                kaydet.Write(richTextBox1.Text);
                kaydet.Flush();
                kaydet.Close();
                degisiklik = false;
                kayitli = true;
                
            }
            else
            {
                farklıKaydetToolStripMenuItem_Click(sender, e);
            }
        }

        //FARKLI KAYDET
        private void farklıKaydetToolStripMenuItem_Click(object sender, EventArgs e)
      
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SaveFile(saveFileDialog1.FileName);
                fileName = saveFileDialog1.FileName;
                this.Text = (fileName + " - Not Defteri");
                kayitli = true;
                degisiklik = false;
            }
            
        }

        // YAZDIR
        private void yazdirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.printDialog1.ShowDialog();
        }

        // SAYFA YAPISI
        private void sayfaYapisiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.pageSetupDialog1.ShowDialog();
        }

        // ÇIKIŞ
        private void cikisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //----------------------------- FORM CLOSING ----------------------------------------

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (degisiklik)
            {
                drCikis = MessageBox.Show("Dosyada değişiklik yaptınız." + "\n" + "\n" + "Değişiklikleri kaydetmek istiyor musunuz?", "Not Defteri", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (drCikis == DialogResult.No)
                {
                    Application.ExitThread();
                }
                else if (drCikis == DialogResult.Yes)
                {
                    kaydetToolStripMenuItem_Click(sender, e);
                }
                else if (drCikis==DialogResult.Cancel)
                {
                    e.Cancel = true;
                }      
            }
        }

        // ------------------ DÜZEN MENÜSÜ --------------------------------------
        //GERİ AL
        private void geriAltoolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        //KES
        private void kesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        //KOPYALA
        private void kopyalaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        //YAPIŞTIR
        private void yapıştırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        // SİL
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        //TÜMÜNÜ SEÇ
        private void tümünüSeçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        // SAAT/TARİH
        private void saatTarihToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += " " + DateTime.Now;
        }

        // ------------------------ BİÇİM MENÜSÜ --------------------------------

        private void sözcükKaydırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sözcükKaydırToolStripMenuItem.Checked == false)
            {
                sözcükKaydırToolStripMenuItem.Checked = true;
                toolStripButtonSozcukKaydir.CheckState = CheckState.Checked;
            }
            else
            {
                sözcükKaydırToolStripMenuItem.Checked = false;
                toolStripButtonSozcukKaydir.CheckState = CheckState.Unchecked;
            }
            
            richTextBox1.WordWrap = sözcükKaydırToolStripMenuItem.Checked;
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = richTextBox1.Font;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionFont = (fontDialog1.Font);
            }
        }

        private void renkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.SelectionColor = (colorDialog1.Color);
            }
        }

        //---------------------------- GÖRÜNÜM MENÜSÜ ---------------------------------

        private void durumÇubuğuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (durumÇubuğuToolStripMenuItem.Checked == false)
            {
                durumÇubuğuToolStripMenuItem.Checked = true;
            }
            else
            {
                durumÇubuğuToolStripMenuItem.Checked = false;
            }

            statusStrip1.Visible = durumÇubuğuToolStripMenuItem.Checked;
        }

        //--------------------------- YARDIM MENÜSÜ -----------------------------------

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormPopup();
            form.ShowDialog();
        }

        //--------------------------- TOOLSTRIP -----------------------------------

        // YENİ
        private void toolStripButtonYeni_Click(object sender, EventArgs e)
        {
            yeniToolStripMenuItem_Click(sender, e);
        }

        // AÇ
        private void toolStripButtonAc_Click(object sender, EventArgs e)
        {
            açToolStripMenuItem_Click(sender, e);
        }

        // KAYDET
        private void toolStripButtonKaydet_Click(object sender, EventArgs e)
        {
            kaydetToolStripMenuItem_Click(sender, e);
        }

        // FARKLI KAYDET
        private void toolStripButtonFarkliKaydet_Click(object sender, EventArgs e)
        {
            farklıKaydetToolStripMenuItem_Click(sender, e);
        }

        // UNDO
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
        }

        // REDO
        private void toolStripButtonYinele_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
        }

        // KES
        private void toolStripButtonKes_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }

        // KOPYALA
        private void toolStripButtonKopyala_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
        }

        // YAPIŞTIR
        private void toolStripButtonYapistir_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        // SİL
        private void toolStripButtonSil_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        // SÖZCÜK KAYDIR
        private void toolStripButtonSozcukKaydir_Click(object sender, EventArgs e)
        {
            sözcükKaydırToolStripMenuItem_Click(sender, e);
        }
       
    }
}
