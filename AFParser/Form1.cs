using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using AFParser.Services;
using System.Threading;
using System.Drawing.Drawing2D;
using AFParser.DataModel;

namespace AFParser
{
    public partial class Form1 : Form
    {
        //Rectangle recordFormFrame=new Rectangle();
        TextParser textParser;
        KeyboardService keyboardService;
        RecordParser recordParser;
        FrameworkScanner frameworkScanner;
        BitmapProvider bitmapProvider;
        Sleeper sleeper;
        List<Record> records = new List<Record>();
        Scheduler scheduler = new Scheduler();

        public Form1()
        {
            InitializeComponent();
            textParser=new TextParser();
            keyboardService = new KeyboardService();
            var textBoxFrameColors = new Color[]{
                //Color.FromArgb(184, 184, 184),
                Color.FromArgb(160, 160, 160),
                Color.FromArgb(185, 185, 185),
            };
            var recordFormFrame=new Rectangle(7, 71, 603, 546);
            recordParser = new RecordParser(recordFormFrame, textBoxFrameColors, keyboardService, textParser);

            var triangleBorderMaxColor=Color.FromArgb(15,15,15);
            var selectedColor = Color.FromArgb(0, 136, 221);
            frameworkScanner = new FrameworkScanner(triangleBorderMaxColor, selectedColor);
            bitmapProvider = new BitmapProvider();
            sleeper = new Sleeper();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            button1.Enabled = false;

            var bitmap = bitmapProvider.GetBitmap();
            bitmap.Save("bitmap.jpg");


            /*recordParser.FindTextBoxes(bitmap);
            int idx;
            var cllN = recordParser.GetValue("Call n", bitmap, out idx,0);
            button1.Text = idx.ToString();
            MessageBox.Show("callN = *" + cllN + "*");*/

            var bounds=new Rectangle(171,120,1077,352);
            frameworkScanner.Scan(bitmap,bounds);

            Console.WriteLine("frameworks found = {0}, selected = {1}", frameworkScanner.FrameworkLinesCenterY.Count, frameworkScanner.SelectedIndex);

            if (frameworkScanner.FrameworkLinesCenterY.Count > 0)
            {
                this.BackColor = Color.Red;
                openCurrentRecord();
                scheduler.Execute(2000, () =>
                {
                    this.BackColor = Color.Blue;
                    keyboardService.CtrlA();
                    scheduler.Execute(2000, () =>
                    {
                        this.BackColor = Color.Cyan;
                        keyboardService.CtrlC();
                        scheduler.Execute(500, () =>
                        {
                            var record = new Record();
                            //keyboardService.CtrlUp();
                            record.Title = Clipboard.GetText();
                            //MessageBox.Show(Clipboard.GetText());
                            bitmap = bitmapProvider.GetBitmap();
                            bitmap.Save("bitmap.jpg");
                            recordParser.FindTextBoxes(bitmap);

                            int index;
                            var editeur = recordParser.GetValue("Editeur",bitmap, out index, 1);
                            if (editeur.Length > 0)
                            {
                                record.Editeur = editeur;
                            }
                            button1.Text = index.ToString();
                            //MessageBox.Show("editeur = *" + editeur + "*");

                            var publicationDate = recordParser.GetValue("Publication date",bitmap,out index,index);
                            if (publicationDate.Length > 0)
                            {
                                if (publicationDate != editeur)
                                {
                                    record.PublicationDate = publicationDate;
                                }
                            }
                            button1.Text = index.ToString();

                            var pagesNumber = recordParser.GetValue("Page nbr", bitmap, out index, index);
                            if (pagesNumber.Length > 0)
                            {
                                if (pagesNumber != publicationDate)
                                {
                                    record.PagesNumber = pagesNumber;
                                }
                            }

                            var callN=recordParser.GetValue("Call n",bitmap,out index,index);
                            if (callN.Length > 0)
                            {
                                if (callN != pagesNumber)
                                {
                                    record.CallN = callN;
                                }
                            }

                            var commentary = recordParser.GetValue("Commentary", bitmap, out index, index);
                            if (commentary.Length > 0)
                            {
                                if (commentary != callN)
                                {
                                    record.Commentary = commentary;
                                }
                            }

                            var authors = recordParser.GetValue("Authors", bitmap, out index, index);
                            if (authors.Length > 0)
                            {
                                if (authors != commentary)
                                {
                                    record.Authors = authors;
                                }
                            }

                            var isbn = recordParser.GetValue("ISBN", bitmap, out index, index);
                            if (isbn.Length > 0)
                            {
                                if (isbn != authors)
                                {
                                    record.ISBN = isbn;
                                }
                            }

                            //MessageBox.Show(record.ToString());

                            clickManagementDatas();
                            scheduler.Execute(200, () =>
                            {
                                clickDoNotSave();
                                scheduler.Execute(500, () =>
                                {
                                    var numberOfCopies=recordParser.GetTextBoxValue(new Rectangle(358, 333, 82, 15));
                                    button1.Text = "*" + numberOfCopies + "*";
                                    this.BackColor = Color.Black;
                                    MessageBox.Show("numberOfCopies = *" + numberOfCopies + "*");
                                    /*clickNumberOfCopies();
                                    this.BackColor = Color.Magenta;
                                    Thread.Sleep(100);
                                    clickNumberOfCopies();
                                    this.BackColor = Color.Wheat;
                                    scheduler.Execute(5000, () =>
                                    {
                                        keyboardService.CtrlA();
                                        this.BackColor = Color.Violet;
                                        scheduler.Execute(5000, () =>
                                        {
                                            keyboardService.CtrlC();
                                            this.BackColor = Color.Tomato;
                                            scheduler.Execute(5000, () =>
                                            {
                                                var numberOfCopies = Clipboard.GetText();
                                                button1.Text = "*"+numberOfCopies+"*";
                                                this.BackColor = Color.Black;
                                                MessageBox.Show("numberOfCopies = *" + numberOfCopies + "*");
                                            });
                                        });
                                    });*/
                                });
                                //keyboardService.Tab();
                                //keyboardService.Tab();
                                /*sleeper.Delay(200, () =>
                                {
                                    clickDoNotSave();
                                });*/
                            });
                        });
                    });
                });
            }

            //button1.Enabled = true;
        }

        void clickNumberOfCopies()
        {
            var point = new MouseOperations.MousePoint(398, 340);
            MouseOperations.LeftMouseClick(point);
        }

        void clickDoNotSave()
        {
            var point = new MouseOperations.MousePoint(283, 307);
            MouseOperations.LeftMouseClick(point);
            /*Thread.Sleep(500);
            MouseOperations.LeftMouseClick(point);
            Thread.Sleep(500);
            MouseOperations.LeftMouseClick(point);*/
        }

        void clickManagementDatas()
        {
            MouseOperations.LeftMouseClick(228, 654);
        }

        void openCurrentRecord()
        {
            var index = frameworkScanner.SelectedIndex;
            var point = new Point(frameworkScanner.ControlX, frameworkScanner.FrameworkLinesCenterY[index]);
            point.X += 100;
            MouseOperations.LeftMouseClick(point.X, point.Y);
            Thread.Sleep(50);
            MouseOperations.LeftMouseClick(point.X, point.Y);
            Thread.Sleep(50);
            MouseOperations.LeftMouseClick(point.X, point.Y);
        }

        /*Bitmap getBitmap()
        {
        }*/

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }
    }
}
