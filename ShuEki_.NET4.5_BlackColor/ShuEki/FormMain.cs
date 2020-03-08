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

namespace MASAMUNE
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

        // プログラム開始時初期化処理
        public void ShuEkiInit()
        {
            // 
            // exe.configファイルの設定から、黒画面モードか通常画面モードか切り替える
            // 
            if(Settings.Default.BlackColorSettings == true)
            {
                // 黒画面モードの場合、コントロールの色を黒基調に変える

                // コントロールの色をセット
                // 　個別に設定を変更できるようにする
                this.BackColor = Color.Black;

                logwrite_button.BackColor = Color.Black;
                start_button.BackColor = Color.Black;
                BackrollButton.BackColor = Color.Black;

                logwrite_button.ForeColor = Color.Teal;
                start_button.ForeColor = Color.Teal;
                BackrollButton.ForeColor = Color.Teal;

                label1.BackColor = Color.Black;
                label2.BackColor = Color.Black;
                label3.BackColor = Color.Black;
                label4.BackColor = Color.Black;
                label5.BackColor = Color.Black;

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
            ShuEkiLogWriteButtonOffOn(false);

            // 現在筮竹を立てている状況で処理を選択（0:表示消去 → 1:下位 → 2:上位 → 3:変爻 → 0:...）
            zeichiku.target = 0;
        }

        // 占い結果画像クリア処理
        public void ShuEkiClear()
        {
            pictureBox1.Image = yinyan.BLANK_BMP;
            pictureBox2.Image = yinyan.BLANK_BMP;
            pictureBox3.Image = yinyan.BLANK_BMP;
            pictureBox4.Image = yinyan.BLANK_BMP;
            pictureBox5.Image = yinyan.BLANK_BMP;
            pictureBox6.Image = yinyan.BLANK_BMP;
        }

        // テキストボックスクリア処理
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

        // テキスト出力（原文）処理
        public void ShuEkiDispTextChinensis( byte kekka, byte henkou )
        {
            textBox1.Text = rikujusika.EkiNoGokui_Taii[kekka];
            textBox2.Text = rikujusika.genbun_taii[kekka];

            // 現状、引数のhenkou（変爻）は参照予定無し。
        }

        // 本卦之卦切り替えボタンの表示／非表示処理
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

        // ログ出力ボタンの表示／非表示処理
        public void ShuEkiLogWriteButtonOffOn(bool offon)
        {
            if (offon)
            {
                logwrite_button.Enabled = true;
                logwrite_button.Visible = true;
            }
            else
            {
                logwrite_button.Enabled = false;
                logwrite_button.Visible = false;
            }
        }

        // 爻位を求める処理
        public int ShuEkiHenkou(byte koui)
        {
            int onmyou;                 // 各爻の陰陽を判断・記憶
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

        // 占いの実体【略筮法】処理
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

                    for (loop = 0; loop <= 2; loop++) // 下から、3本の筮竹を積んでいく
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
                        if (zeichiku.target == 1)    // 下卦の表示
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
                        else                       // 上卦の表示
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
//                    zeichiku.take += 1;                                           // 人策を天策に加えるのだが、本処理ではビットシフトの都合上、すでに加わっている

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
                    ShuEkiLogWriteButtonOffOn(true);
                    zeichiku.target = 0;// 下位→上位→変爻
                }
            }
            else
            {
                ShuEkiClear();
                ShuEkiClearText();
                ShuEkiBackrollButtonOffOn(false);
                ShuEkiLogWriteButtonOffOn(false);
                zeichiku.target++;
            } 
        }

        // 後戻りボタン（本卦・之卦表示切替）を押された時、表示を切り替える処理
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

        private void logwrite_button_Click(object sender, EventArgs e)
        {
            this.WriteLogTexts();
            this.ShuEkiLogWriteButtonOffOn(false);
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