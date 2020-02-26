using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Resources;

using ShuEki;
using ShuEki.Properties;
using System.IO;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        zeichiku zeichiku = new zeichiku();         // 筮竹クラス
        yinyan yinyan = new yinyan();               // 陰陽クラス
        Image yin_yan;                              // 画像データバッファ
        Random rnd = new Random();                  // 占い用ランダム関数

        public Form1()
        {
            InitializeComponent();
            ShuEkiInit();
        }

        public void ShuEkiInit()
        {
            // exe.configファイルの設定から、黒画面モードか通常画面モードか切り替える
            // 
            if(Settings.Default.BlackColorSettings == true)
            {
                // 黒画面モードの場合、コントロールの色を黒基調に変える

                // コントロールの色をセット
                this.BackColor = Color.Black;

                logout_button.BackColor = Color.Black;
                start_button.BackColor = Color.Black;
                BackrollButton.BackColor = Color.Black;

                logout_button.ForeColor = Color.Teal;
                start_button.ForeColor = Color.Teal;
                BackrollButton.ForeColor = Color.Teal;

                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                label5.ForeColor = Color.Black;

                label1.ForeColor = Color.Teal;
                label2.ForeColor = Color.Teal;
                label3.ForeColor = Color.Teal;
                label4.ForeColor = Color.Teal;
                label5.ForeColor = Color.Teal;

                textBox1.BorderStyle = BorderStyle.FixedSingle;
                textBox2.BorderStyle = BorderStyle.FixedSingle;
                textBox3.BorderStyle = BorderStyle.FixedSingle;
                textBox4.BorderStyle = BorderStyle.FixedSingle;
                textBox5.BorderStyle = BorderStyle.FixedSingle;
                textBox6.BorderStyle = BorderStyle.FixedSingle;
                textBox7.BorderStyle = BorderStyle.FixedSingle;
                textBox8.BorderStyle = BorderStyle.FixedSingle;
                textBox9.BorderStyle = BorderStyle.FixedSingle;

                textBox1.BackColor = Color.Black;
                textBox2.BackColor = Color.Black;
                textBox3.BackColor = Color.Black;
                textBox4.BackColor = Color.Black;
                textBox5.BackColor = Color.Black;
                textBox6.BackColor = Color.Black;
                textBox7.BackColor = Color.Black;
                textBox8.BackColor = Color.Black;
                textBox9.BackColor = Color.Black;

                textBox1.ForeColor = Color.Teal;
                textBox2.ForeColor = Color.Teal;
                textBox3.ForeColor = Color.Teal;
                textBox4.ForeColor = Color.Teal;
                textBox5.ForeColor = Color.Teal;
                textBox6.ForeColor = Color.Teal;
                textBox7.ForeColor = Color.Teal;
                textBox8.ForeColor = Color.Teal;
                textBox9.ForeColor = Color.Teal;

                // 画像ファイルロード 
                yinyan.YIN_BMP = ShuEki.Properties.Resources.yin_another;
                yinyan.YAN_BMP = ShuEki.Properties.Resources.yan_another;
                yinyan.YIN_CHANGE_BMP = ShuEki.Properties.Resources.yin_change_another;
                yinyan.YAN_CHANGE_BMP = ShuEki.Properties.Resources.yan_change_another;
                yinyan.BLANK_BMP = ShuEki.Properties.Resources.blank_another;
            }
            else
            {
                // 通常画面モードの場合、コントロールの色はデフォルトのシステム設定とする

                // 画像ファイルロード 
                yinyan.YIN_BMP = ShuEki.Properties.Resources.yin;
                yinyan.YAN_BMP = ShuEki.Properties.Resources.yan;
                yinyan.YIN_CHANGE_BMP = ShuEki.Properties.Resources.yin_change;
                yinyan.YAN_CHANGE_BMP = ShuEki.Properties.Resources.yan_change;
                yinyan.BLANK_BMP = ShuEki.Properties.Resources.blank;
            }

            // 結果クリア
            ShuEkiClear();

            // テキストボックスクリア
            ShuEkiClearText();

            // 後戻りボタン非表示・無効
            ShuEkiBackrollButtonOffOn(false);

            // ログ出力ボタン非表示・無効
            ShuEkiLogoutButtonOffOn(false);
        }

        // 占い結果画像をクリアする。
        public void ShuEkiClear()
        {
            pictureBox1.Image = yinyan.BLANK_BMP;
            pictureBox2.Image = yinyan.BLANK_BMP;
            pictureBox3.Image = yinyan.BLANK_BMP;
            pictureBox4.Image = yinyan.BLANK_BMP;
            pictureBox5.Image = yinyan.BLANK_BMP;
            pictureBox6.Image = yinyan.BLANK_BMP;
        }

        // テキストボックスクリア
        public void ShuEkiClearText()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
        }

        // テキスト出力（原文）
        public void ShuEkiDispTextChinensis( byte kekka, byte henkou )
        {
            textBox2.Text = rikujusika.genbun_taii[kekka];
            textBox4.Text = rikujusika.genbun_tanden[henkou];
        }

        // 後戻りボタンの表示・有効／非表示・無効切替
        public void ShuEkiBackrollButtonOffOn(bool offon)
        {
            if (offon)
            {
                BackrollButton.Enabled = true;
                BackrollButton.Visible = true;
                BackrollButton.Text = "本卦を見る。";
            }
            else
            {
                BackrollButton.Enabled = false;
                BackrollButton.Visible = false;
            }
        }

        // ログ出力ボタンの表示・有効／非表示・無効切替
        public void ShuEkiLogoutButtonOffOn(bool offon)
        {
            if (offon)
            {
                logout_button.Enabled = true;
                logout_button.Visible = true;
            }
            else
            {
                logout_button.Enabled = false;
                logout_button.Visible = false;
            }
        }

        // 爻位を求める。
        public int ShuEkiHenkou(byte koui)
        {
            int onmyou;                 // 陰陽
            Image tmpimg;               // 画像バッファ

            onmyou = (byte)((zeichiku.honka >> koui) & 0x01);

            if (onmyou == yinyan.YIN)
            {
                tmpimg = yinyan.YAN_CHANGE_BMP;
            }
            else
            {
                tmpimg = yinyan.YIN_CHANGE_BMP;
            }
            switch (koui)
            {
                case 0:
                    pictureBox1.Image = tmpimg;
                    zeichiku.shika = (zeichiku.honka ^= 0x01);
                    break;
                case 1:
                    pictureBox2.Image = tmpimg;
                    zeichiku.shika = (zeichiku.honka ^= 0x02);
                    break;
                case 2:
                    pictureBox3.Image = tmpimg;
                    zeichiku.shika = (zeichiku.honka ^= 0x04);
                    break;
                case 3:
                    pictureBox4.Image = tmpimg;
                    zeichiku.shika = (zeichiku.honka ^= 0x08);
                    break;
                case 4:
                    pictureBox5.Image = tmpimg;
                    zeichiku.shika = (zeichiku.honka ^= 0x10);
                    break;
                case 5:
                    pictureBox6.Image = tmpimg;
                    zeichiku.shika = (zeichiku.honka ^= 0x20);
                    break;
            }
            return onmyou;
        }

        // 占いの実体【略筮法】
        private void button1_Click(object sender, EventArgs e)
        {
            int loop;
            int temp;
            byte baratsuki;
            
            switch (zeichiku.target)
            {
                case 0:
                    this.start_button.Text = "下卦を求める。";
                    break;
                case 1:
                    this.start_button.Text = "上卦を求める。";
                    break;
                case 2:
                    this.start_button.Text = "爻位を求める。";
                    break;
                default:
                    break;
            }
            if( zeichiku.target > 0 )
            {
                // 下卦、上卦を求める
                baratsuki = (byte)rnd.Next( 8 );
                zeichiku.take = 49;                                              // 太極を立てる。
                if (baratsuki >= 5)                                              // 天策と地策に分ける（この時、大体半分で分けるが、±４本ばらつかせる。天策を保持する）
                {
                    baratsuki -= 4;
                    zeichiku.take = (byte)((zeichiku.take / 2) + baratsuki);     // +4本くらいのばらつき
                }
                else
                {
                    zeichiku.take = (byte)((zeichiku.take / 2) - baratsuki);     // -4本くらいのばらつき
                }

                if( zeichiku.target <= 2 )                                                       
                {
                    zeichiku.take %= 8;                                          // 略筮法の下卦・上卦は、8で割った余りを求める
                    zeichiku.take += 1;                                          // 人策を天策に加える

                    switch (zeichiku.take)
                    {
                    case 1:
                        zeichiku.ka = hakka.KEN;
                        break;
                    case 2:
                        zeichiku.ka = hakka.DA;
                        break;
                    case 3:
                        zeichiku.ka = hakka.RI;
                        break;
                    case 4:
                        zeichiku.ka = hakka.SHIN;
                        break;
                    case 5:
                        zeichiku.ka = hakka.SON;
                        break;
                    case 6:
                        zeichiku.ka = hakka.KAN;
                        break;
                    case 7:
                        zeichiku.ka = hakka.GON;
                        break;
                    case 8:
                        zeichiku.ka = hakka.KON;
                        break;
                    default:
                        zeichiku.ka = 0;
                        break;
                    }
                    // 下卦代入
                    if (zeichiku.target == 1)
                    {
                        zeichiku.kai = zeichiku.ka;
                        textBox5.Text = rikujusika.youso[zeichiku.take];    // 筮竹の本数から下卦をテキスト表示
                    }
                    // 上卦代入
                    else
                    {
                        zeichiku.joui = zeichiku.ka;
                        textBox6.Text = rikujusika.youso[zeichiku.take];    // 筮竹の本数から上卦をテキスト表示
                    }

                    for (loop = 0; loop <= 2; loop++)
                    {
                        temp = (byte)((zeichiku.ka >> loop) & 0x01);
                        if (temp == yinyan.YIN)
                        {
                            yin_yan = yinyan.YIN_BMP;

                        }
                        else
                        {
                            yin_yan = yinyan.YAN_BMP;
                        }
                        if (zeichiku.target == 1)
                        {
                            switch (loop)
                            {
                                case 0:
                                    pictureBox1.Image = yin_yan;
                                    break;
                                case 1:
                                    pictureBox2.Image = yin_yan;
                                    break;
                                case 2:
                                    pictureBox3.Image = yin_yan;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (loop)
                            {
                                case 0:
                                    pictureBox4.Image = yin_yan;
                                    break;
                                case 1:
                                    pictureBox5.Image = yin_yan;
                                    break;
                                case 2:
                                    pictureBox6.Image = yin_yan;
                                    // 下卦と上卦をまとめて六十四卦を求める。
                                    zeichiku.honka = (byte)((zeichiku.joui << 3) | zeichiku.kai);
                                    textBox7.Text = rikujusika.all[zeichiku.honka]; // 文字列表示
                                    ShuEkiDispTextChinensis(zeichiku.honka, 0);     //test
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    zeichiku.take = 49;                                             // 天策、地策、人策をまとめる
                    zeichiku.target++;
                }
                // 爻位を求める
                else
                {
                    zeichiku.take %= 6;                                             // 略筮法の爻位は、6で割った余りを求める
//                    zeichiku.take += 1;                                           // 人策を天策に加えるのが正式だが、ビットシフトの都合上、加えない。占いの結果は変わらない。

                    temp = ShuEkiHenkou(zeichiku.take);                             // 変爻を反映する
                    if((zeichiku.take == 0) || (zeichiku.take == 5))
                    {
                        textBox8.Text = rikujusika.henkou[zeichiku.take] + rikujusika.kuroku[temp]; 
                    }
                    else
                    {
                        textBox8.Text = rikujusika.kuroku[temp] + rikujusika.henkou[zeichiku.take];
                    }
                    textBox9.Text = rikujusika.all[zeichiku.shika];                 // 之卦を表示
                    ShuEkiDispTextChinensis(zeichiku.shika, zeichiku.take);         // 之卦の本文を表示

                    this.start_button.Text = "再度占筮を行う。";
                    ShuEkiBackrollButtonOffOn(true);
                    ShuEkiLogoutButtonOffOn(true);
                    zeichiku.target = 0;// この値だと下位→上位→変爻で永久ループ
                }
            }
            else
            {
                ShuEkiClear();
                ShuEkiClearText();
                ShuEkiBackrollButtonOffOn(false);
                ShuEkiLogoutButtonOffOn(false);
                zeichiku.target++;
            } 
        }

        // 後戻りボタン（本卦・之卦表示切替）
        // 設計ミス。動きはOKだけど変数の使い方がだめ。
        private void BackrollButton_Click(object sender, EventArgs e)
        {
            if (BackrollButton.Text == "本卦を見る。")
            {
                ShuEkiHenkou(zeichiku.take);
                ShuEkiDispTextChinensis(zeichiku.honka, 0);                         // 本卦の本文を表示
                BackrollButton.Text = "之卦を見る。";
            }
            else
            {
                ShuEkiHenkou(zeichiku.take);
                ShuEkiDispTextChinensis(zeichiku.honka, 0);                         // 之卦の本文を表示
                BackrollButton.Text = "本卦を見る。";
            }
        }

        private void logout_button_Click(object sender, EventArgs e)
        {
            this.WriteLogTexts();
            this.ShuEkiLogoutButtonOffOn(false);
        }

        private void WriteLogTexts()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DateTime now = DateTime.Now;
            Encoding encoding = Encoding.GetEncoding("Shift-JIS");
            string path = currentDirectory + @"\MASAMUNE.log";
            string contents = now.Year.ToString("D04") + "." + now.Month.ToString("D02") + "." + now.Day.ToString("D02") + "_" + now.Hour.ToString("D02") + ":" + now.Minute.ToString("D02") + " （" + this.textBox8.Text + "）" + this.textBox7.Text + "が" + this.textBox9.Text + "に之く。\r\n";
            try
            {
                File.AppendAllText(path, contents, encoding);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "err");
            }
        }
    }
}